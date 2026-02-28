using System.Collections.Generic;
using Core.Enums;

namespace Core.Data.Stats
{
    /// <summary>
    ///     CharacterInstance에서 스탯 시스템을 관리합니다.
    /// </summary>
    public class StatSystem
    {
        private readonly Dictionary<StatType, StatInstance> _stats = new();

        //SO로 부터 생성될때
        public StatSystem(List<StatInstance> data)
        {
            foreach (var instance in data) _stats[instance.Type] = new StatInstance(instance);
        }

        //복사 생성자
        public StatSystem(StatSystem original)
        {
            foreach (var instance in original._stats) _stats[instance.Key] = new StatInstance(instance.Value);
        }

        public float GetStatValue(StatType stat)
        {
            return _stats[stat].Value;
        }

        public void TakeDamage(float damage)
        {
            var currentHp = _stats[StatType.HP];
            var newHp = currentHp.Value - damage;
            _stats[StatType.HP].Value = newHp;
        }

        public void Heal(float heal)
        {
            var currentHP = _stats[StatType.HP];
            var newHP = currentHP.Value + heal;
            _stats[StatType.HP].Value = newHP;
        }

        public void ConsumeSkillPoint(StatType stat, float point)
        {
            _stats[stat].Value -= point;
        }
    }
}