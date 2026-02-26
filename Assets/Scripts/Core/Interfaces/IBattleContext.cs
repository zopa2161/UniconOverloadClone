using Core.Data.Battle;

namespace Core.Interfaces
{
    public interface IBattleContext
    {
        CharacterBoard friendlyBoard { get; }
        CharacterBoard enemyBoard { get; }
    }
}