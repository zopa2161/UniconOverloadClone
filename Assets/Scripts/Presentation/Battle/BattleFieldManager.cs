using UnityEngine;

namespace Presentation.Battle
{
    public class BattleFieldManager : MonoBehaviour
    {
        public static BattleFieldManager Instance { get; private set; }
        
        [Header("Time Management")]
        public float battleTime = 0f;       // 전투가 시작된 후 흐른 총 시간
        public float timeScale = 1.0f;      // 시간 배속 (1=정상, 2=2배속 등)
        public bool isPaused = false;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        private void Update()
        {
            // 일시정지 상태가 아닐 때만 전투 시간을 흘려보냄
            if (!isPaused)
            {
                battleTime += Time.deltaTime * timeScale;
            }
        }
        
        public float GetBattleDeltaTime()
        {
            if (isPaused) return 0f;
            return Time.deltaTime * timeScale;
        }
        
        public void TogglePause()
        {
            isPaused = !isPaused;
            Debug.Log(isPaused ? "전투 일시정지!" : "전투 재개!");
        }
        
    }
}