using System.Collections.Generic;
using Core.Data.Targeting;

namespace Core.Interfaces
{
    public interface ITactic
    {
        List<TargetGroup> EvaluateAndFilterTargetGroups(List<TargetGroup> candidates, IBattleContext ctx);
    }
}