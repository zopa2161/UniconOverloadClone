using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class BeforeHitLog : BattleLogEvent
    {
        public Effect.Effect effet;

        public BeforeHitLog(BattleLogType type, Effect.Effect effect, CharacterInstance actor,List<CharacterInstance> targets) : base(type, actor, targets)
        {
            effet = effect;
        }
    }
}