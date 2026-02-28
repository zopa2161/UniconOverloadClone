using System.Collections;
using Core.Data.Character;
using Core.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace Presentation.Battle.Characters
{
    public class CharacterView : MonoBehaviour
    {
        [Header("비주얼 위치")] public Transform visualRoot; // 외형 프리팹이 생성될 위치 (빈 자식 오브젝트)

        // 조종해야 할 애니메이터
        private Animator _animator;
        public Slider hpBar;

        public CharacterInstance LogicData { get; private set; }


        public void Initialize(CharacterInstance character, bool isEnemy = false)
        {
            // 기존 초기화 로직...
            LogicData = character;
            UpdateHPBar();

            if (LogicData.Data.VisualPrefab != null)
            {
                // visualRoot 밑에 실제 도트/모델 프리팹을 자식으로 생성합니다.
                var visualObj = Instantiate(LogicData.Data.VisualPrefab, visualRoot);

                // 생성된 외형 프리팹에 붙어있는 애니메이터를 훔쳐옵니다. (나중에 연출할 때 써야 하니까!)
                _animator = visualObj.GetComponent<Animator>();
            }

            // 🌟 적군일 경우 좌우 반전 처리
            if (isEnemy) visualRoot.localScale = new Vector3(-1, 1, 1);
        }

        // 🌟 뎁스에 맞게 소팅 오더를 맞춰주는 함수
        public void SetSortingOrder(int order)
        {
            // 2. 만약 캐릭터 프리팹 최상단에 SortingGroup 컴포넌트를 달아두셨다면 이게 훨씬 깔끔합니다.
            var sortingGroup = GetComponent<SortingGroup>();
            if (sortingGroup != null) sortingGroup.sortingOrder = order;
        }

        public void UpdateHPBar()
        {
            if (hpBar != null)
                hpBar.value = LogicData.StatSystem.GetStatValue(StatType.HP) /
                              LogicData.StatSystem.GetStatValue(StatType.MaxHP);
        }
        
        public void PlaySimpleActionEffect()
        {
            // 간단하게 위로 살짝 뛰었다가 내려오는 애니메이션 (코루틴)
            StartCoroutine(JumpRoutine());
        }

        private IEnumerator JumpRoutine()
        {
            Vector3 startPos = transform.position;
        
            // 위로 살짝 올라감
            transform.position = startPos + Vector3.up * 0.5f;
            yield return new WaitForSeconds(0.1f);
        
            // 다시 제자리로 복귀
            transform.position = startPos;
        }
        
        public void PlayAttackAnimation()
        {
            // 1. 애니메이터가 있다면 트리거를 작동시킵니다.
            if (_animator != null) _animator.SetTrigger("attack");

            // 2. 애니메이션이 당장 없다면, 앞으로 돌진했다가 돌아오는 코루틴으로 훌륭한 타격감을 낼 수 있습니다.
            StartCoroutine(AttackDashRoutine());
        }

        public void PlayHitEffect()
        {
            // 빨간색으로 깜빡이거나, 뒤로 살짝 밀리는 연출
            StartCoroutine(HitBlinkRoutine());
        }

        private IEnumerator AttackDashRoutine()
        {
            Vector3 startPos = transform.position;
            Vector3 forwardPos = startPos + (transform.right * (LogicData.Faction == CharacterFaction.Friendly ? 1f : -1f));

            // 앞으로 휙!
            transform.position = forwardPos;
            yield return new WaitForSeconds(0.15f);
    
            // 다시 제자리로 복귀
            transform.position = startPos;
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
    }
}