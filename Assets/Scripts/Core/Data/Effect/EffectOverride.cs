using System;
using UnityEngine;

namespace Core.Data.Effect
{
    [Serializable]
    public class EffectOverride
    {
        [SerializeField] private Effect _effect;

        //validation등을 통하여 _data의 effectAction을 깊은 복사한 값 직렬화한다.
        [SerializeReference] private EffectAction _effectActionOverride;

        public Effect Effect => _effect;
        public EffectAction EffectActionOverride => _effectActionOverride;
    }
}