using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class BattleStartLog : BattleLogEvent
    {
        public BattleStartLog(BattleLogType type, CharacterInstance actor) : base(type, actor)
        {
        }
    }
}