using System.Collections.Generic;
using Core.Data.Battle;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IPassiveRequester
    {
        List<SkillExecutionData> RequestPassive(SkillTiming timing, SkillExecutionContext ctx);
        
    }
}