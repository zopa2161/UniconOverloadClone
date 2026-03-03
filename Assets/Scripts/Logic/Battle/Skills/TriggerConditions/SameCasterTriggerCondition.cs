using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.TriggerConditions
{
    [Serializable]
    [ConditionDescription("스킬을 사용한 캐릭터가 패시브 보유자 본인인지, 혹은 타인인지에 따라 발동 여부를 결정합니다.")]
    public class SameCasterTriggerCondition : ITriggerCondition
    {
        [Tooltip("True: 본인이 스킬을 썼을 때 발동 / False: 타인이 스킬을 썼을 때 발동")] [SerializeField]
        private bool _sameCaster = true;

        public bool IsSatisfiedBy(CharacterInstance caster, SkillExecutionContext ctx)
        {
            // caster: 이 패시브/트리거를 보유하고 검사 중인 주체 (나)
            // ctx.Caster: 현재 이벤트(스킬)를 실제로 발생시킨 주체

            var isSame = caster == ctx.caster;

            // 💡 핵심: 플래그와 일치하는지만 비교하면 완벽하게 작동합니다.
            // - isSame(true) == _sameCaster(true) -> true 반환
            // - isSame(false) == _sameCaster(false) -> true 반환 (남이 썼을 때 발동하는 조건 만족!)
            return isSame == _sameCaster;
        }
    }
}