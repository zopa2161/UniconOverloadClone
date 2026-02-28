using System;
using System.Collections.Generic;
using Core.Attributes;
using Core.Data.Battle.BattleLogs;
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

        public override float ApplyAction(CharacterInstance caster, CharacterInstance target)
        {
            //아무튼 복잡한 계산을 해서 최종 데미지를 결정한다
            target.StatSystem.TakeDamage(_defaultDamage);
            //Debug.Log("Damage Dealt");
            return 0;
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