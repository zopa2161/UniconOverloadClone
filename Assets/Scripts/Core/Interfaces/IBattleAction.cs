namespace Core.Interfaces
{
    public interface IBattleAction
    {
        void Execute(IBattleAPI requester, IBattleContext ctx);
         bool IsFinished { get; }
    }
}