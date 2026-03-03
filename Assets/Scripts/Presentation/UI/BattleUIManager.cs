using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Presentation.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        private UIDocument _document;
        private Label _passiveLabel;
        private Label _skillNameLabel;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            var root = _document.rootVisualElement;

            // UXML에서 라벨 찾아오기
            _skillNameLabel = root.Q<Label>("skill-name-label");
            _passiveLabel = root.Q<Label>("passive-label");
        }

        // 🌟 외부(대본 리더)에서 호출할 UI 팝업 함수
        public void ShowSkillName(string skillName)
        {
            if (_skillNameLabel == null) return;
            _skillNameLabel.text = skillName;
            _skillNameLabel.style.display = DisplayStyle.Flex;
        }

        public void OffSkillName()
        {
            _skillNameLabel.style.display = DisplayStyle.None;
        }

        public void ShowPassiveSkillName(string name)
        {
            StartCoroutine(ShowPassiveRoutine(name));
        }

        private IEnumerator ShowPassiveRoutine(string skillName)
        {
            // 1. 텍스트를 갈아끼우고 화면에 보이게 켭니다.
            _passiveLabel.text = skillName;
            _passiveLabel.style.display = DisplayStyle.Flex;

            // 2. 1.5초 동안 멋지게 띄워둡니다.
            yield return new WaitForSeconds(1.5f);

            // 3. 다시 숨깁니다.
            _passiveLabel.style.display = DisplayStyle.None;
        }
    }
}