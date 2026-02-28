using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Stats;
using Core.Enums;
using Logic.Battle;
using Presentation.UI;
using UnityEngine;

namespace Logic
{
    public class BattleTest : MonoBehaviour
    {
        public StandbyScreenUI standbyScreenUI;
        public CharacterData TestCharacterData1;
        public CharacterData TestCharacterData2;

        public CharacterInstance TestCharacter1;
        public CharacterInstance TestCharacter2;

        private EncounterManager encounterManager;

        public void Start()
        {
            TestCharacter1 = new CharacterInstance(TestCharacterData1);
            TestCharacter2 = new CharacterInstance(TestCharacterData2);
            
            TestCharacter1.SkillSystem.TestingTacticDataSetup();
            TestCharacter2.SkillSystem.TestingTacticDataSetup();

            
            TestCharacter1.Faction = CharacterFaction.Friendly;
            TestCharacter2.Faction = CharacterFaction.Enemy;
            
            var test1Board = new CharacterBoard();
            test1Board.characters[0] = TestCharacter1;
            
            var test2Board = new CharacterBoard();
            test2Board.characters[0] = TestCharacter2;
            
            var squad1 = new SquadData(test1Board);
            squad1.Faction = CharacterFaction.Friendly;
            Debug.Log($"{squad1.Board.characters.Length}");
            var squad2 = new SquadData(test2Board);
            squad2.Faction = CharacterFaction.Enemy;
            
            encounterManager = new EncounterManager();
            encounterManager.OnEncounterTriggered += standbyScreenUI.OpenScreen;
            
            encounterManager.TriggerEncounter(squad1, squad2);
            
            
            var battleContext = new BattleContext(test1Board, test2Board);
            var battleManager = new BattleManager(battleContext);

            //battleManager.StartBattle();

        }



    }
}