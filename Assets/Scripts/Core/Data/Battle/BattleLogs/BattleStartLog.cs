using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class BattleStartLog : BattleLogEvent
    {
        public BattleStartLog(BattleLogType type, CharacterInstance actor,List<CharacterInstance> targets) : base(type, actor, targets)
        {
        }
    }
}