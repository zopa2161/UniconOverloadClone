using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class AfterHitLog : BattleLogEvent
    {
        public AfterHitLog(BattleLogType type, CharacterInstance actor, List<CharacterInstance> targets) : base(type, actor,targets)
        {
        }
    }
}