using Core.Data.Battle.BattleLogs;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnActiveSkillEndAction :BattleAction
    {
        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            requester.RecordEvent(new ActiveSkillEndLog());
            _isFinished = true;
        
        }
    }
}