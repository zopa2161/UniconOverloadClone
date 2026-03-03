using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;
using UnityEngine;

namespace Core.Data.Stats
{
    /// <summary>
    ///     CharacterInstance에서 스탯 시스템을 관리합니다.
    /// </summary>
    public class StatSystem
    {
        public delegate void HpValueHandler(CharacterInstance owner, float value);

        private readonly Dictionary<StatType, float> _originalStats = new();

        private readonly CharacterInstance _owner;

        private readonly Dictionary<StatType, StatInstance> _stats = new();

        public StatSystem(CharacterInstance owner, List<StatInstance> data)
        {
            _owner = owner;
            foreach (var instance in data) _stats[instance.Type] = new StatInstance(instance);
            foreach (var instance in _stats) _originalStats[instance.Key] = instance.Value.Value;
        }

        //복사 생성자
        public StatSystem(StatSystem original)
        {
            foreach (var instance in original._stats) _stats[instance.Key] = new StatInstance(instance.Value);
        }
        //SO로 부터 생성될때

        public event HpValueHandler HpValueChanged;

        public float GetStatValue(StatType stat)
        {
            return _stats[stat].Value;
        }

        public float GetOriginalStatValue(StatType type)
        {
            return _originalStats[type];
        }

        public void TakeDamage(float damage)
        {
            var currentHp = _stats[StatType.HP];
            var newHp = currentHp.Value - damage;
            _stats[StatType.HP].Value = newHp;
            HpValueChanged?.Invoke(_owner, _stats[StatType.HP].Value);
        }

        public void Heal(float heal)
        {
            var currentHP = _stats[StatType.HP];
            var newHP = Mathf.Clamp(currentHP.Value + heal, 0f, _stats[StatType.MaxHP].Value);
            _stats[StatType.HP].Value = newHP;
        }

        public void ConsumeSkillPoint(StatType stat, float point)
        {
            _stats[stat].Value -= point;
        }
    }
}