using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public class ActiveSkillEndLog : BattleLogEvent
    {
        public ActiveSkillEndLog() : base(BattleLogType.Movement, null, null)
        {
            
        }
    }
}