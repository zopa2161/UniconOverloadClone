using Core.Data.Character;

namespace Core.Data.Skills
{
    public class SkillInstance : IOInstance<Skill>
    {
        public SkillInstance(Skill data, CharacterInstance owner) : base(data)
        {
            Owner = owner;
        }

        public CharacterInstance Owner { get; }
    }
}