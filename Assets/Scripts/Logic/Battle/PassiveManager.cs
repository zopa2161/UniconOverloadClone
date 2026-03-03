using System.Collections.Generic;
using System.Linq;
using Core.Data.Battle;
using Core.Data.Character;
using Core.Data.Skills;
using Core.Enums;
using Core.Interfaces;

namespace Logic.Battle
{
    public struct PassivePending
    {
        public CharacterInstance caster;
        public SkillInstance skill;
        public List<CharacterInstance> targets;
    }

    public class PassiveManager
    {
        private readonly List<CharacterInstance> _allCharacters;


        public PassiveManager(IBattleContext battleContext)
        {
            var allCharacters = battleContext.friendlyBoard.GetAllCharacters()
                .Concat(battleContext.enemyBoard.GetAllCharacters());

            _allCharacters = allCharacters.ToList();
        }

        public List<SkillExecutionData> GetPassiveReact(SkillTiming timing, SkillExecutionContext ctx,
            IBattleContext battleContext)
        {
            var intiativeSorted = _allCharacters.OrderByDescending(x => x.GetStatValue(StatType.Initiative));
            var result = new List<SkillExecutionData>();

            foreach (var character in intiativeSorted)
            {
                //0. 스킬을 사용할 수 없는 상태라면 패스.
                if (character.StatSystem.GetStatValue(StatType.PP) < 1) continue;

                var skillSystem = character.SkillSystem;

                foreach (var data in skillSystem.PassiveSkillTacticQueue)
                {
                    var skill = data.Skill.Data;
                    //Debug.Log($"Passive React {skill.CodeName}");
                    //해야할것.
                    //1. 타이밍이 맞는가? 2. 조건이 맞는가?
                    if (timing == skill.Timing && skill.IsSatisfyPassiveCondition(character, ctx))
                    {
                        // //3. 타이밍과 조건이 맞으면 타겟 만들기.
                        var target = data.GetTarget(character, battleContext, ctx);
                        
                        //외부 설정 tactic에 의하여 적절한 타겟이 존재하지 않는다면 pass
                        //만약 외부 설정 tactic이 있더라도, 내부적으로 고정된 타겟이 있다면 무시하고 내부의 타겟으로 사용.
                        if (target != null && target.Count != 0)
                        {
                            result.Add(new SkillExecutionData
                            {
                                caster = character,
                                skill = data.Skill,
                                targets = target
                            });
                            //한 캐릭터당 하나의 패시브 스킬만 허용하므로
                            break;
                        }
                    }
                }
            }

            //Debug.Log($"Passive React {result.Count}");
            return result;
        }
    }
}