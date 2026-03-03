using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Effect;

namespace Logic.Battle.Effects.EffectActions
{    [System.Serializable]
    public class AlternateTargetAction : EffectAction
    {
       
    
        public override float ApplyAction(CharacterInstance caster, CharacterInstance target, SkillExecutionContext skillContext)
        {
            //만약 다수기 이면 당연히 캔슬 시켜야함.
            if (skillContext.targets.Count > 1) return -1;
            if (target != skillContext.targets[0]) return -1;
            skillContext.ParentContext.targets[0] = caster;
            return 0;
        }
        
        public override object Clone()
        {
            return new AlternateTargetAction();
        }
    }
}