using Core.Data.Battle;
using Core.Data.Character;
using Logic.Battle;
using Presentation.Battle;
using UnityEngine;

namespace Presentation.Test
{
    public class BattleInitializer : MonoBehaviour
    {
        [Header("Friendly")] public CharacterData Friendly1;

        public CharacterData Friendly2;
        public CharacterData Friendly3;
        public CharacterData Friendly4;
        public CharacterData Friendly5;
        public CharacterData Friendly6;

        [Header("Enemy")] public CharacterData Enemy1;

        public CharacterData Enemy2;

        public BattleViewManager BattleViewManager;

        public void Start()
        {
            var fr1 = new CharacterInstance(Friendly1);
            var fr2 = new CharacterInstance(Friendly2);

            var en1 = new CharacterInstance(Enemy1);
            var en2 = new CharacterInstance(Enemy2);

            fr1.SkillSystem.TestingTacticDataSetup();
            fr2.SkillSystem.TestingTacticDataSetup();
            en1.SkillSystem.TestingTacticDataSetup();
            en2.SkillSystem.TestingTacticDataSetup();

            var friendlyBoard = new CharacterBoard();
            friendlyBoard.characters[0] = fr1;
            friendlyBoard.characters[1] = fr2;

            var enemyBoard = new CharacterBoard();
            enemyBoard.characters[0] = en1;
            enemyBoard.characters[1] = en2;

            var battleContext = new BattleContext(friendlyBoard, enemyBoard);

            var battleManager = new BattleManager(battleContext);
            battleManager.StartBattle();
            BattleViewManager.Initialize(battleManager);
            StartCoroutine(BattleViewManager.PlayBattleRoutine(battleManager.BattleLogEvents));
        }
    }
}