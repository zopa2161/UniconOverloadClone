using Core.Data.Character;

namespace Core.Data.Skills
{
    public class SkillInstance : IOInstance<Skill>
    {
        public CharacterInstance Owner { get; }
        
        public SkillInstance(Skill data, CharacterInstance owner) : base(data)
        {
            Owner = owner;
        }
    }
}