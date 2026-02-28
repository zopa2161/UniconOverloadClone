using Core.Data.Battle.BattleLogs;

namespace Core.Interfaces
{
    public interface IBattleLogRequester
    {
        void RecordEvent(BattleLogEvent log);
    }
}