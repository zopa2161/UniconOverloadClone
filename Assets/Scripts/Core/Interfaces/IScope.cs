using System;
using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Targeting;

namespace Core.Interfaces
{
    public interface IScope : ICloneable
    {
        //caster의 진영을 확인하고 리턴하는 식으로 구현해야합니다.
        List<TargetGroup> GetCandidateGroups(CharacterInstance caster, IBattleContext ctx);
    }
}