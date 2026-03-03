using System.Collections;
using Core.Data.Character;
using Core.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UIElements.Slider;

namespace Presentation.Battle.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [Header("비주얼 위치")] public Transform visualRoot; // 외형 프리팹이 생성될 위치 (빈 자식 오브젝트)

        [Header("자원 UI")] public Text apText; // 🔴 빨간 다이아몬드 역할
        public Text ppText; // 🔵 파란 다이아몬드 역할
        private Animator _animator;
        private Label _apLabel;
        private VisualElement _rootElement;


        // 조종해야 할 애니메이터
        private Transform _spawnPoint;

        [Header("UI Toolkit")] private UIDocument _uiDocument;

        // 🌟 내부에 '시각적인 현재 자원'을 따로 기억해둡니다 (매우 중요!)
        private int _visualAP;
        private int _visualHP;
        private int _visualPP;
        public Slider hpBar;

        //애니메이션용 상태변수
        private bool isInitialPosition;

        public CharacterInstance LogicData { get; private set; }

        private void LateUpdate()
        {
            if (_rootElement == null || Camera.main == null) return;

            // 1. 내 머리 위 3D 좌표 (Y축으로 살짝 올림)
            var headPos = transform.position + Vector3.up * 1.5f;

            // 2. 3D 좌표를 2D 화면 좌표로 변환!
            var screenPos = RuntimePanelUtils.CameraTransformWorldToPanel(
                _rootElement.panel,
                headPos,
                Camera.main);

            // 3. 내 UXML의 위치를 그곳으로 이동
            _rootElement.transform.position = screenPos;
        }


        public void Initialize(CharacterInstance character, bool isEnemy = false, Transform spawnPoint = null)
        {
            // 기존 초기화 로직...
            _spawnPoint = spawnPoint;
            LogicData = character;


            if (LogicData.Data.VisualPrefab != null)
            {
                // visualRoot 밑에 실제 도트/모델 프리팹을 자식으로 생성합니다.
                var visualObj = Instantiate(LogicData.Data.VisualPrefab, visualRoot);

                // 생성된 외형 프리팹에 붙어있는 애니메이터를 훔쳐옵니다. (나중에 연출할 때 써야 하니까!)
                _animator = visualObj.GetComponent<Animator>();
            }

            // 🌟 적군일 경우 좌우 반전 처리
            if (isEnemy) visualRoot.localScale = new Vector3(-1, 1, 1);


            //===UI세팅===
            _uiDocument = GetComponent<UIDocument>();
            if (_uiDocument != null)
            {
                _rootElement = _uiDocument.rootVisualElement;
                _apLabel = _rootElement.Q<Label>("ap-label");

                _visualAP = (int)LogicData.StatSystem.GetOriginalStatValue(StatType.AP);
                _visualPP = (int)LogicData.StatSystem.GetOriginalStatValue(StatType.PP);
                _visualHP = (int)LogicData.StatSystem.GetOriginalStatValue(StatType.HP);
                // 초기값 세팅
                UpdateResourceUI();
            }
        }

        // 🌟 뎁스에 맞게 소팅 오더를 맞춰주는 함수
        public void SetSortingOrder(int order)
        {
            // 2. 만약 캐릭터 프리팹 최상단에 SortingGroup 컴포넌트를 달아두셨다면 이게 훨씬 깔끔합니다.
            var sortingGroup = GetComponent<SortingGroup>();
            if (sortingGroup != null) sortingGroup.sortingOrder = order;
        }

        public void PlayEffectTriggerAnim(string trigger)
        {
            if (_animator != null) _animator.SetTrigger(trigger);
        }

        public void PlayAttackAnimation()
        {
            // 1. 애니메이터가 있다면 트리거를 작동시킵니다.
            if (_animator != null) _animator.SetTrigger("attack");
        }

        public void PlayCastingAnimation()
        {
            if (_animator != null) _animator.SetTrigger("casting");
        }

        public void PlayHitEffect()
        {
            // 빨간색으로 깜빡이거나, 뒤로 살짝 밀리는 연출
            if (_animator != null) _animator.SetTrigger("hurt");
            StartCoroutine(HitBlinkRoutine());
        }


        private IEnumerator HitBlinkRoutine()
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                sprite.color = Color.white;
            }
        }

        public void PlayHealEffect()
        {
            StartCoroutine(HealBlinkRoutine());
        }

        private IEnumerator HealBlinkRoutine()
        {
            var sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.color = Color.green;
                yield return new WaitForSeconds(0.1f);
                sprite.color = Color.white;
            }
        }

        public void MoveToTarget(Transform targetTransform)
        {
            if (targetTransform == null) return;

            // 이동 코루틴 실행
            StartCoroutine(MoveRoutine(targetTransform.position));
        }

        // 🌟 실제 부드러운 이동을 처리하는 코루틴
        private IEnumerator MoveRoutine(Vector3 targetPos)
        {
            // 1. 달리기 애니메이션 트리거 작동!
            if (_animator != null) _animator.SetTrigger("run");

            var startPos = transform.position;
            var duration = 0.3f; // 💡 이동에 걸리는 시간 (원하는 속도에 맞춰 조절하세요)
            var elapsedTime = 0f;

            // 2. 지정된 시간(duration) 동안 목표 위치로 부드럽게 이동
            while (elapsedTime < duration)
            {
                // Lerp: 시작점과 끝점 사이를 시간에 따라 부드럽게 연결해 줍니다.
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;

                yield return null; // 다음 프레임까지 대기
            }

            transform.position = targetPos;
        }

        public void ReturnToSpawnPoint()
        {
            if (_spawnPoint == null)
            {
                Debug.LogWarning("[CharacterView] _spawnPoint가 설정되지 않아 복귀할 수 없습니다!");
                return;
            }

            StartCoroutine(ReturnRoutine());
        }

        // 🌟 실제 부드러운 복귀를 처리하는 코루틴
        private IEnumerator ReturnRoutine()
        {
            // 1. 복귀 애니메이션 재생 (돌아갈 때도 달리기 모션을 쓴다면 run 트리거 사용)
            if (_animator != null) _animator.SetTrigger("run");

            var startPos = transform.position;
            var targetPos = _spawnPoint.position;
            var duration = 0.25f; // 💡 돌아올 때는 타격 후 딜레이 느낌으로 갈 때보다 살짝 빠른 게 타격감이 좋습니다.
            var elapsedTime = 0f;

            /* * (선택 사항) 2D 게임에서 뒤로 돌아갈 때 스프라이트를 반전시키고 싶다면:
             * visualRoot.localScale = new Vector3(LogicData.Faction == CharacterFaction.Enemy ? 1 : -1, 1, 1);
             */

            // 2. 원래 위치로 부드럽게 이동 (Lerp)
            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 3. 목표 위치에 완벽하게 안착 (오차 보정)
            transform.position = targetPos;

            /* * (선택 사항) 스프라이트를 반전시켰다면 원래대로 복구:
             * visualRoot.localScale = new Vector3(LogicData.Faction == CharacterFaction.Enemy ? -1 : 1, 1, 1);
             */

            // 4. 대기 상태(Idle)로 복귀!
            if (_animator != null) _animator.SetTrigger("idle");
        }

        public void UpdateResourceUI()
        {
            if (_apLabel != null) _apLabel.text = $"AP: {_visualAP} / PP: {_visualPP} n/ HP : {_visualHP}";
        }

        // 🌟 대본(Log) 리더가 스킬을 썼을 때 시각적으로 깎아달라고 요청할 함수
        public void ConsumeVisualAP(int amount = 1)
        {
            _visualAP = Mathf.Max(0, _visualAP - amount);
            UpdateResourceUI();
        }

        public void ConsumeVisualPP(int amount = 1)
        {
            _visualPP = Mathf.Max(0, _visualPP - amount);
            UpdateResourceUI();
        }

        public void TakeVisualHP(int amount = 0)
        {
            _visualHP = Mathf.Clamp(_visualHP + amount, 0,
                (int)LogicData.StatSystem.GetOriginalStatValue(StatType.MaxHP));
            UpdateResourceUI();
        }

        public void CharacterDeath()
        {
            if (_animator != null) _animator.SetTrigger("die");
        }
    }
}