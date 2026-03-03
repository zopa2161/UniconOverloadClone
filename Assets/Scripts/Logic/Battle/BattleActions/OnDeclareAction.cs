using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Interfaces;

namespace Logic.Battle.BattleActions
{
    public class OnDeclareAction : BattleAction
    {
        public CharacterInstance Caster;
        private bool declared;

        private int executeIndex;
        public SkillInstance Skill;
        public List<CharacterInstance> Targets;

        public OnDeclareAction(SkillExecutionContext skillContext) : base(skillContext)
        {
        }

        public OnDeclareAction(CharacterInstance caster, List<CharacterInstance> targets, SkillInstance skill,
            SkillExecutionContext skillContext) : base(skillContext)
        {
            Caster = caster;
            Targets = targets;
            Skill = skill;
        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            if (_skillContext != null && _skillContext.caster != null && _skillContext.caster.IsDead)
            {
                _isFinished = true;
                return;
            }

            // 한번 선언되었다면 다시 선언 x
            if (!declared)
            {
                requester.RecordEvent(new SkillDeclareLog(Caster, Targets, Skill));
                declared = true;
            }

            //스킬을 순회하면서 스킬 액션을 뿌려주면 됩니다.
            //1. 스킬액션 꺼내기
            var skillAction = Skill.Data.Actions[executeIndex];

            //2. 컨텍스트 만들기.
            var target = skillAction.GetTarget(Caster, Targets, battleContext, _skillContext);
            //Debug.Log($"Target : {target}");
            var context = new SkillExecutionContext(Skill.Data, skillAction, Caster, target, _skillContext);
            //3.액션 푸쉬하기.
            requester.PushSkillSequence(context);
            executeIndex++;
            if (executeIndex == Skill.Data.Actions.Count) _isFinished = true;
        }
    }
}