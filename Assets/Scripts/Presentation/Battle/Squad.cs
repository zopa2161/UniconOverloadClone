using Core.Data.Battle;
using UnityEngine;

namespace Presentation.Battle
{
    public class Squad : MonoBehaviour
    {
        private SquadData _squadData;
        
        [Header("Selection State")]
        public bool isSelected = false;
        
        //=== 배틀 필드 관련===
        private float _moveSpeed = 5f;
        private Vector3 _targetPosition;

        public void Initialize(SquadData squadData)
        {
            _squadData = squadData;
        }
        
        private void Update()
        {
            // 1. 매니저에게서 현재 전투의 흐름(시간)을 가져옴
            float currentDelta = BattleFieldManager.Instance.GetBattleDeltaTime();

            // 2. 시간이 흐르고 있다면 (일시정지가 아니라면) 이동
            if (currentDelta > 0)
            {
                MoveToTarget(currentDelta);
            }
        }
        
        private void MoveToTarget(float delta)
        {
            // 목표 지점을 향해 매니저의 시간에 맞춰 이동
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _targetPosition, 
                _moveSpeed * delta
            );
        }
        
        public void SetTarget(Vector3 newTarget)
        {
            _targetPosition = newTarget;
        }

        public void SetSelected(bool selected)
        {
            isSelected = selected;
        }
    }
}