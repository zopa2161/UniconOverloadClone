using Core.Attributes;
using Core.Data.Character;
using Core.Data.Effect;
using Core.Enums;
using UnityEngine;

namespace Logic.Battle.Effects.EffectActions
{
    [System.Serializable]
    public class HealAction : EffectAction
    {
        [SerializeField, EditorContext(EditorContext.SkillEditor)] private float _defaultHealAmount;
        [SerializeField, EditorContext(EditorContext.EffectEditor)] private StatType _bonusStatType;
        [SerializeField, EditorContext(EditorContext.SkillEditor)] private float _bonusFactor;
        public override void ApplyAction(CharacterInstance caster, CharacterInstance target)
        {
            //아무튼 어찌저찌 힐량계산
            target.StatSystem.Heal(_defaultHealAmount);
            //Debug.Log("Heal");
        }
        
        public override object Clone()
        {
            var result = new HealAction();
            result._defaultHealAmount = _defaultHealAmount;
            result._bonusStatType = _bonusStatType;
            result._bonusFactor = _bonusFactor;
            return result;
        
        }

    }
}