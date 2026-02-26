using System.Collections.Generic;
using System.Linq;
using Core.Data.Skills;
using Core.Data.Stats;
using Core.Enums;
using Core.Interfaces;

namespace Core.Data.Character
{
    public class CharacterInstance : IOInstance<CharacterData>
    {
        private StatSystem _statSystem;
        private SkillSystem _skillSystem;
        
        public StatSystem StatSystem => _statSystem;
        public SkillSystem SkillSystem => _skillSystem;

        //From SO
        public CharacterInstance(CharacterData data) : base(data)
        {
            _statSystem = new StatSystem(data.StatData);
            _skillSystem = new SkillSystem(this, data.ActiveSkillData, data.PassiveSkillData);
        }

        //복사 생성자
        public CharacterInstance(CharacterInstance original) : base(original)
        {
            _statSystem = new StatSystem(original._statSystem);
        }

        public CharacterFaction Faction { get; set; }

        

        public bool IsDead => _statSystem.GetStatValue(StatType.HP) <= 0;


        public float GetStatValue(StatType stat)
        {
            return _statSystem.GetStatValue(stat);
        }

      
        
    }
}