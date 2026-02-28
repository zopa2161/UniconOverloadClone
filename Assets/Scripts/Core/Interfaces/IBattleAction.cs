namespace Core.Interfaces
{
    public interface IBattleAction
    {
        bool IsFinished { get; }
        void Execute(IBattleAPI requester, IBattleContext ctx);
    }
}