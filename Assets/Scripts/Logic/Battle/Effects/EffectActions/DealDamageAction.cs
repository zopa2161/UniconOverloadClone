using System;
using Core.Attributes;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Effect;
using Core.Enums;
using UnityEngine;

namespace Logic.Battle.Effects.EffectActions
{
    [Serializable]
    public class DealDamageAction : EffectAction
    {
        [SerializeField] [EditorContext(EditorContext.SkillEditor)]
        private float _defaultDamage;

        [SerializeField] [EditorContext(EditorContext.EffectEditor)]
        private StatType _bonusStatType;

        [SerializeField] [EditorContext(EditorContext.SkillEditor)]
        private float _bonusFactor;

        [SerializeField] [EditorContext(EditorContext.EffectEditor)]
        private StatType _defenseStatType;

        public override float ApplyAction(CharacterInstance caster, CharacterInstance target, SkillExecutionContext skillContext)
        {
            //아무튼 복잡한 계산을 해서 최종 데미지를 결정한다
            target.StatSystem.TakeDamage(_defaultDamage);
            Debug.Log($"caster : {caster.Data.CodeName} target : {target.Data.CodeName} Damage Dealt");
            return _defaultDamage;
        }

        public override object Clone()
        {
            var result = new DealDamageAction();
            result._defaultDamage = _defaultDamage;
            result._bonusStatType = _bonusStatType;
            result._bonusFactor = _bonusFactor;
            result._defenseStatType = _defenseStatType;
            return result;
        }
    }
}