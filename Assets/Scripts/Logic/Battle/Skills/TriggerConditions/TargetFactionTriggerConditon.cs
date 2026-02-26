using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    [ConditionDescription("컨텍스트의 타겟과 캐스터의 팩션 일치 여부로 패시브 발동 여부를 결정합니다.")]
    public class TargetFactionTriggerCondition : ITriggerCondition
    {
        [SerializeField] private bool _isFriendly = true;

        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            // 타깃이 없으면 조건 불만족(또는 스펙에 따라 true로 바꿔도 됨)
            if (ctx.targets == null || ctx.targets.Count == 0 || ctx.targets[0] == null)
                return false;

            var sameFaction = caster.Faction == ctx.targets[0].Faction;

            // _isFriendly == true  => 같은 진영이어야 함
            // _isFriendly == false => 다른 진영이어야 함
            return _isFriendly ? sameFaction : !sameFaction;
        }
    }
}