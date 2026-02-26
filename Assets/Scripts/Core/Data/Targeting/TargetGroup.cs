using System.Collections.Generic;
using Core.Data.Character;

namespace Core.Data.Targeting
{
    public class TargetGroup
    {
        public TargetGroup(List<CharacterInstance> targets)
        {
            Targets = targets;
            Score = 0f;
        }

        public List<CharacterInstance> Targets { get; private set; }
        public float Score { get; set; }
    }
}