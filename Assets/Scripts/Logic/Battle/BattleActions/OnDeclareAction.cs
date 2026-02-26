using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.BattleActions
{
    public class OnDeclareAction : BattleAction
    {
        private bool consumeSkillPoint = false;
        public CharacterInstance Caster;

        private int executeIndex =0;
        public SkillInstance Skill;
        public List<CharacterInstance> Targets;

        public OnDeclareAction(SkillExecutionContext ctx) : base(ctx)
        {
        }

        public OnDeclareAction(CharacterInstance caster, List<CharacterInstance> targets, SkillInstance skill)
        {
            Caster = caster;
            Targets = targets;
            Skill = skill;

        }

        public override void Execute(IBattleAPI requester, IBattleContext battleContext)
        {
            //0. 스킬 타입을 확인해서 caster의 ap,pp를 소모 시킨다.
            if (!consumeSkillPoint)
            {
                if (Skill.Data.Type == SkillType.Active)
                {
                    Caster.StatSystem.ConsumeSkillPoint(StatType.AP, 1);
                    consumeSkillPoint = true;
                }
                else if (Skill.Data.Type == SkillType.Passive)
                {
                    Caster.StatSystem.ConsumeSkillPoint(StatType.PP, 1);
                    consumeSkillPoint = true;
                }
            }
           
            Debug.Log($"Execute Declare Action Caster : {Caster.Faction}, {Caster.Data.CodeName} SKill :  {Skill.Data.CodeName}");
            //스킬을 순회하면서 스킬 액션을 뿌려주면 됩니다.
            //1. 스킬액션 꺼내기
            var skillAction = Skill.Data.Actions[executeIndex];

            //2. 컨텍스트 만들기.
            //이때는 부모 컨텍스트는 null
            var target = skillAction.GetTarget(Caster, Targets, battleContext);
            //Debug.Log($"Target : {target}");
            var context = new SkillExecutionContext(Skill.Data, skillAction, Caster, target, _ctx);

            //3.7개의 액션 푸쉬하기.
            requester.PushSkillSequence(context);
            executeIndex++;
            if(executeIndex == Skill.Data.Actions.Count ) _isFinished = true;
            
        }
    }
}