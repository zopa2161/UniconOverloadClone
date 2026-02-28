using System.Collections.Generic;
using Core.Data.Character;

namespace Core.Data.Skills
{
    public class SkillSystem
    {
        //SO로 부터 생성될때
        public SkillSystem(CharacterInstance owner, List<Skill> activeSkills, List<Skill> passiveSkills)
        {
            ActiveSKills = new List<SkillInstance>();
            PassiveSKills = new List<SkillInstance>();

            foreach (var skill in activeSkills) ActiveSKills.Add(new SkillInstance(skill, owner));
            foreach (var skill in passiveSkills) PassiveSKills.Add(new SkillInstance(skill, owner));
        }

        //복사 생성자
        public SkillSystem(SkillSystem original, CharacterInstance owner)
        {
            ActiveSKills = new List<SkillInstance>();
            PassiveSKills = new List<SkillInstance>();

            foreach (var skill in original.ActiveSKills) ActiveSKills.Add(new SkillInstance(skill.Data, owner));
            foreach (var skill in original.PassiveSKills) PassiveSKills.Add(new SkillInstance(skill.Data, owner));

            //전술 데이터는 얕은 복사?
            ActiveSkillTacticQueue = original.ActiveSkillTacticQueue;
            PassiveSkillTacticQueue = original.PassiveSkillTacticQueue;
        }

        public List<SkillInstance> ActiveSKills { get; }

        public List<SkillInstance> PassiveSKills { get; }

        public List<SkillTacticData> ActiveSkillTacticQueue { get; private set; }

        public List<SkillTacticData> PassiveSkillTacticQueue { get; private set; }

        public void TestingTacticDataSetup()
        {
            ActiveSkillTacticQueue = new List<SkillTacticData>();
            foreach (var skill in ActiveSKills)
            {
                var tactic = new SkillTacticData(skill, null, null);
                ActiveSkillTacticQueue.Add(tactic);
            }

            PassiveSkillTacticQueue = new List<SkillTacticData>();
            foreach (var skill in PassiveSKills)
            {
                var tactic = new SkillTacticData(skill, null, null);
                PassiveSkillTacticQueue.Add(tactic);
            }
        }
    }
}