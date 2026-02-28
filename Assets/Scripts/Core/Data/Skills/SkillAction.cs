using System;
using System.Collections.Generic;
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

        public IScope Scope => _targetingScheme.Scope;

        public EffectType EffectType => _effectOverrides[0].Effect.Type;
        public List<EffectOverride> EffectOverrides => _effectOverrides;

        public SkillTiming TriggerTiming => _triggerTiming;

        public List<CharacterInstance> GetTarget(CharacterInstance caster, List<CharacterInstance> inheritTarget,
            IBattleContext ctx)
        {
            if (_targetingType == SkillActionTargetingType.InheritMacro) return inheritTarget;
            return _targetingScheme.GetTarget(caster, ctx);
        }

        public List<BattleLogEvent> ExecuteSkillAciton(CharacterInstance caster, CharacterInstance target)
        {
            // 💡 모든 이펙트가 뱉어내는 로그들을 한곳에 모을 '빈 대본 꾸러미'를 만듭니다.
            List<BattleLogEvent> totalLogs = new List<BattleLogEvent>();
    
            foreach (var effectOverride in _effectOverrides)
            {
                // 1. 밑바닥에서 계산을 끝내고 올라온 파편화된 대본(로그 리스트)을 받습니다.
                var effectNumber = effectOverride.EffectActionOverride.ApplyAction(caster, target);
                var log = new ApplyEffectLog(effectOverride.Effect, effectNumber, caster, target);
                totalLogs.Add(log);

            }

            // 3. 병합이 완료된 하나의 거대한 대본을 윗선(Action)으로 반환합니다!
            return totalLogs;
        }
    }
}