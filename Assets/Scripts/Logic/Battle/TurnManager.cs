using System.Collections.Generic;
using System.Linq;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle
{
    public class TurnManager
    {
        private readonly Queue<CharacterInstance> _activeQueue;

        public TurnManager()
        {
            _activeQueue = new Queue<CharacterInstance>();
        }


        public void SetupNewBattle(IBattleContext context)
        {
            _activeQueue.Clear();

            // 1. 피아 식별 없이 모든 캐릭터를 모아 속도(Speed) 내림차순으로 정렬 (체이닝으로 간결화)
            var sortedCharacters = context.friendlyBoard.GetAllCharacters()
                .Concat(context.enemyBoard.GetAllCharacters())
                .OrderByDescending(c => c.GetStatValue(StatType.Initiative))
                .ToList();

            // 안전장치: 전장에 아무도 없다면 즉시 종료
            if (sortedCharacters.Count == 0) return;

            // 2. 현재 전장에서 가장 높은 AP 수치 탐색 (최대 사이클 횟수)
            var maxAP = (int)sortedCharacters.Max(c => c.GetStatValue(StatType.AP));

            // 3. 1 AP부터 최대 AP 사이클까지 순회 (플래그 변수 제거!)
            for (var currentCycleAP = 1; currentCycleAP <= maxAP; currentCycleAP++)
            {
                // 이미 속도순으로 정렬된 리스트에서, 
                // 현재 사이클을 소화할 수 있는(AP가 충분한) 캐릭터만 필터링
                var capableCharacters = sortedCharacters
                    .Where(c => c.GetStatValue(StatType.AP) >= currentCycleAP);

                // 큐에 순서대로 삽입
                foreach (var character in capableCharacters) _activeQueue.Enqueue(character);
            }

            ;
        }

        public CharacterInstance GetNextActiveCharacter()
        {
            if (_activeQueue.Count == 0) return null;

            return _activeQueue.Dequeue();
        }
    }
}