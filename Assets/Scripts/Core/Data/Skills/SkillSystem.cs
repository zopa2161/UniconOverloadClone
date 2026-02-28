using System.Collections.Generic;
using Core.Data.Character;
using UnityEngine;

namespace Core.Data.Skills
{
    public class SkillSystem
    {
        private List<SkillInstance> _activeSkills;
        private List<SkillInstance> _passiveSkills;

        public List<SkillInstance> ActiveSKills => _activeSkills;
        public List<SkillInstance> PassiveSKills => _passiveSkills;
        
        private List<SkillTacticData> _activeSkillTacticQueue;
        private List<SkillTacticData> _passiveSkillTacticQueue;

        public List<SkillTacticData> ActiveSkillTacticQueue => _activeSkillTacticQueue;
        public List<SkillTacticData> PassiveSkillTacticQueue => _passiveSkillTacticQueue;


        //SO로 부터 생성될때
        public SkillSystem(CharacterInstance owner, List<Skill> activeSkills, List<Skill> passiveSkills)
        {
            _activeSkills = new List<SkillInstance>();
            _passiveSkills = new List<SkillInstance>();

            foreach (var skill in activeSkills)
            {
                _activeSkills.Add(new SkillInstance(skill, owner));
            }
            foreach (var skill in passiveSkills)
            {
                _passiveSkills.Add(new SkillInstance(skill, owner));
            }
        }
        
        //복사 생성자
        public SkillSystem(SkillSystem original, CharacterInstance owner)
        {
            _activeSkills = new List<SkillInstance>();
            _passiveSkills = new List<SkillInstance>();
            
            foreach (var skill in original._activeSkills)
            {
                _activeSkills.Add(new SkillInstance(skill.Data, owner));
            }
            foreach (var skill in original._passiveSkills)
            {
                _passiveSkills.Add(new SkillInstance(skill.Data, owner));
            }
            
            //전술 데이터는 얕은 복사?
            _activeSkillTacticQueue = original.ActiveSkillTacticQueue;  
            _passiveSkillTacticQueue = original.PassiveSkillTacticQueue;
        }

        public void TestingTacticDataSetup()
        {
            
            _activeSkillTacticQueue = new List<SkillTacticData>();
            foreach (var skill in ActiveSKills)
            {
                var tactic = new SkillTacticData(skill,null,null);
                _activeSkillTacticQueue.Add(tactic);
            }
            _passiveSkillTacticQueue = new List<SkillTacticData>();
            foreach (var skill in PassiveSKills)
            {
                var tactic = new SkillTacticData(skill,null,null);
                _passiveSkillTacticQueue.Add(tactic);
            }
            
        }
    }
}