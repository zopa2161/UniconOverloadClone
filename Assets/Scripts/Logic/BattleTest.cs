using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Stats;
using Core.Enums;
using Logic.Battle;
using UnityEngine;

namespace Logic
{
    public class BattleTest : MonoBehaviour
    {
        
        public CharacterData TestCharacterData1;
        public CharacterData TestCharacterData2;

        public CharacterInstance TestCharacter1;
        public CharacterInstance TestCharacter2;

        public CharacterInstance TestCharacter3;

        public void Start()
        {
            TestCharacter1 = new CharacterInstance(TestCharacterData1);
            TestCharacter2 = new CharacterInstance(TestCharacterData1);
            TestCharacter3 = new CharacterInstance(TestCharacterData2);
            
            TestCharacter1.SkillSystem.TestingTacticDataSetup();
            TestCharacter2.SkillSystem.TestingTacticDataSetup();
            TestCharacter3.SkillSystem.TestingTacticDataSetup();
            
            TestCharacter1.Faction = CharacterFaction.Friendly;
            TestCharacter2.Faction = CharacterFaction.Enemy;
            TestCharacter3.Faction = CharacterFaction.Friendly;
            
            var test1Board = new CharacterBoard();
            test1Board.characters[0] = TestCharacter1;
            test1Board.characters[1] = TestCharacter3;
            var test2Board = new CharacterBoard();
            test2Board.characters[0] = TestCharacter2;
            
            
            
            var battleContext = new BattleContext(test1Board, test2Board);
            var battleManager = new BattleManager(battleContext);

            battleManager.StartBattle();

        }



    }
}