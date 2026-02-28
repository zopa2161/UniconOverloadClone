using Core.Data.Battle;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI
{
    public class StandbyScreenUI : MonoBehaviour
    {
        private UIDocument _document;
        private VisualElement _root;
        private Label _playerSquadName;
        private Label _enemySquadName;
        private Button _startBattleBtn;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;

            // 1. UI Builder에서 설정한 이름(Name)으로 엘리먼트 쿼리
            _playerSquadName = _root.Q<Label>("player-squad-name");
            _enemySquadName = _root.Q<Label>("enemy-squad-name");
            _startBattleBtn = _root.Q<Button>("start-battle-btn");

            // 2. 처음에는 UI를 숨겨둡니다.
            _root.style.display = DisplayStyle.None;

            // 3. 버튼 클릭 이벤트 연결
            _startBattleBtn.clicked += OnStartBattleClicked;
        }

        // 💡 로직(EncounterManager)의 종소리를 들으면 실행될 함수
        public void OpenScreen(SquadData player, SquadData enemy)
        {
            // UI 텍스트 업데이트
            if (_playerSquadName != null) _playerSquadName.text = player.SquadName;
            if (_enemySquadName != null) _enemySquadName.text = enemy.SquadName;

            // 숨겨둔 UI를 화면에 표시!
            _root.style.display = DisplayStyle.Flex;
            Debug.Log("📺 [UI] 전투 대기(Standby) 화면 팝업 완료!");
        }

        private void OnStartBattleClicked()
        {
            Debug.Log("🔥 [UI] 전투 개시 버튼 클릭됨! (진짜 전투 씬으로 전환 등)");
            _root.style.display = DisplayStyle.None; // UI 다시 숨기기
        }
    }
}