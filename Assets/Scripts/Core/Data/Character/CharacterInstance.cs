using Core.Data.Skills;
using Core.Data.Stats;
using Core.Enums;

namespace Core.Data.Character
{
    public class CharacterInstance : IOInstance<CharacterData>
    {
        //From SO
        public CharacterInstance(CharacterData data) : base(data)
        {
            StatSystem = new StatSystem(data.StatData);
            SkillSystem = new SkillSystem(this, data.ActiveSkillData, data.PassiveSkillData);
        }

        //복사 생성자
        public CharacterInstance(CharacterInstance original) : base(original)
        {
            StatSystem = new StatSystem(original.StatSystem);
            SkillSystem = new SkillSystem(original.SkillSystem, this);
        }

        public StatSystem StatSystem { get; }

        public SkillSystem SkillSystem { get; }

        public CharacterFaction Faction { get; set; }


        public bool IsDead => StatSystem.GetStatValue(StatType.HP) <= 0;


        public float GetStatValue(StatType stat)
        {
            return StatSystem.GetStatValue(stat);
        }
    }
}