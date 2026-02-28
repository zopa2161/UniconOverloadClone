using Core.Data.Battle;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace Logic.Battle
{
    public class EncounterManager
    {
        
        public event Action<SquadData, SquadData> OnEncounterTriggered;
        
        public void TriggerEncounter(SquadData attacker, SquadData defender)
        {
            var attackerBoard = new CharacterBoard(attacker.Board);
            var defenderBoard = new CharacterBoard(defender.Board);
            
            var battleContext = new BattleContext(attackerBoard, defenderBoard);
            var battleManager = new BattleManager(battleContext);
            
            OnEncounterTriggered?.Invoke(attacker,defender);
            //EnterStandbyPhase(battleManager);
        }

        private void EnterStandbyPhase(BattleManager battleManager)
        {
            
            
            battleManager.StartBattle();
        }
    }
}