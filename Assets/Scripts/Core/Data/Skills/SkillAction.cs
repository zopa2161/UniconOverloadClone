using System;
using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Effect;
using Core.Data.Targeting;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data.Skills
{
    [Serializable]
    public class SkillAction
    {
        [SerializeField] private SkillActionTargetingType _targetingType;

        [SerializeField] private TargetingScheme _targetingScheme;

        [SerializeField] private List<EffectOverride> _effectOverrides;

        [SerializeField] private SkillTiming _triggerTiming;

        public IScope Scope => _targetingScheme.Scope;

        public EffectType EffectType => _effectOverrides[0].Effect.Type;

        public SkillTiming TriggerTiming => _triggerTiming;

        public List<CharacterInstance> GetTarget(CharacterInstance caster, List<CharacterInstance> inheritTarget,
            IBattleContext ctx)
        {
            if (_targetingType == SkillActionTargetingType.InheritMacro) return inheritTarget;
            return _targetingScheme.GetTarget(caster, ctx);
        }

        public void ExecuteSkillAciton(CharacterInstance caster, CharacterInstance target)
        {
            Debug.Log($"Applying Effect");
            foreach (var effectOverride in _effectOverrides)
            {
                effectOverride.EffectActionOverride.ApplyAction(caster, target );
            }
        }
    }
}