using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class CharacterDeathLog : BattleLogEvent
    {
        public CharacterDeathLog(CharacterInstance actor) : base(BattleLogType.Death, actor,null)
        {
            Actor = actor;
        }
    }
}