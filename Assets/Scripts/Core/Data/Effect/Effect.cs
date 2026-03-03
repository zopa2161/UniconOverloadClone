using Core.Enums;
using UnityEngine;

namespace Core.Data.Effect
{
    public class Effect : IdentifiedObject
    {
        [SerializeField] private EffectType _type;
        [SerializeField] private EffectRangeType _rangeType;

        [SerializeReference] [SubclassSelector]
        private EffectAction _action;

        public EffectType Type => _type;
        public EffectAction Action => _action;
        public EffectRangeType RangeType => _rangeType;
    }
}