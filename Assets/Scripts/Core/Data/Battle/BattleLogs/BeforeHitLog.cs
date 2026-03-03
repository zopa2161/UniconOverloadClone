using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class BeforeHitLog : BattleLogEvent
    {
        public Effect.Effect effet;

        public BeforeHitLog(BattleLogType type, CharacterInstance actor, Effect.Effect effect) : base(type, actor)
        {
            effet = effect;
        }
    }
}