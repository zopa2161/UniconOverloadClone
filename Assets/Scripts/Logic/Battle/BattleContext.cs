using Core.Data.Battle;
using Core.Interfaces;

namespace Logic.Battle
{
    public class BattleContext : IBattleContext
    {
        public CharacterBoard friendlyBoard { get; }
        public CharacterBoard enemyBoard { get; }

        public BattleContext(CharacterBoard board1, CharacterBoard board2)
        {
            friendlyBoard = board1;
            enemyBoard = board2;
        }
    }
}