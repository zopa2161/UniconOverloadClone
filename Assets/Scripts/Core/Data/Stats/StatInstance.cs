using System;
using Core.Enums;
using UnityEngine;

namespace Core.Data.Stats
{
    [Serializable]
    public class StatInstance
    {
        private const float _maxValue = 999f;
        private const float _minValue = 0f;

        [SerializeField] private StatType _type;

        [SerializeField] private float _value;

        public StatInstance(StatInstance original)
        {
            _type = original._type;
            _value = original._value;
        }


        public StatType Type => _type;

        public float Value
        {
            get => _value;
            set
            {
                // 1. 값 가두기 (Clamp): value가 0보다 작으면 0, 999보다 크면 999로 만듭니다.
                var clampedValue = Mathf.Clamp(value, _minValue, _maxValue);

                // 2. 값 변경 체크: 이전 값과 다를 때만 덮어씌웁니다. (최적화)
                if (_value != clampedValue) _value = clampedValue;
                // 3. (추후 확장) 값이 변했을 때 이벤트를 쏠 수 있는 명당자리입니다.
                // 예: OnValueChanged?.Invoke(_type, _value); 
                // -> 체력바 UI 업데이트나 사망 판정 로직을 여기서 연결하면 아주 깔끔합니다.
            }
        }
    }
}