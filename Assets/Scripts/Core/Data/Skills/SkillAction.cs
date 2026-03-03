using System;
using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
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
        [SerializeField] private bool isContinutive;
        public IScope Scope => _targetingScheme.Scope;

        public EffectType EffectType => _effectOverrides[0].Effect.Type;
        public List<EffectOverride> EffectOverrides => _effectOverrides;

        public SkillTiming TriggerTiming => _triggerTiming;
        

        public List<CharacterInstance> GetTarget(CharacterInstance caster, List<CharacterInstance> inheritTarget,
            IBattleContext ctx, SkillExecutionContext skillContext)
        {
            if (_targetingType == SkillActionTargetingType.InheritMacro) return inheritTarget;
            return _targetingScheme.GetTarget(caster, ctx, skillContext);
        }

        public List<ApplyEffectLog> ExecuteSkillAciton(CharacterInstance caster, List<CharacterInstance> targets, SkillExecutionContext skillContext)
        {
            // 💡 모든 이펙트가 뱉어내는 로그들을 한곳에 모을 '빈 대본 꾸러미'를 만듭니다.

            var logs = new List<ApplyEffectLog>();
            foreach (var effectOverride in _effectOverrides)
            {
                var effections = new List<float>();
                Debug.Log($"last caster : {caster.Data.CodeName}");
                Debug.Log($"last target : {targets[0].Data.CodeName}");
                for (int i = targets.Count - 1; i >= 0; i--)
                {
                    var effection = effectOverride.EffectActionOverride.ApplyAction(caster, targets[i], skillContext);
                    effections.Add(effection);
                }
                // foreach (var target in targets)
                // {
                //     var effection = effectOverride.EffectActionOverride.ApplyAction(caster, target, skillContext);
                //     effections.Add(effection);
                // }

                var log = new ApplyEffectLog(effectOverride.Effect, effections, caster, targets );
                logs.Add(log);
            }

            // 3. 병합이 완료된 하나의 거대한 대본을 윗선(Action)으로 반환합니다!
            return logs;
        }
    }
}