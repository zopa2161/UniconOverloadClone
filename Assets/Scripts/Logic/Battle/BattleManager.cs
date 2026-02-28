using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Battle.BattleLogs;
using Core.Enums;
using Core.Interfaces;
using Logic.Battle.BattleActions;
using UnityEngine;

namespace Logic.Battle
{
    public class BattleManager : IBattleAPI
    {
        private readonly PassiveManager _passiveManager;

        private readonly TurnManager _turnManager;
        private readonly Stack<IBattleAction> actionStack = new();


        public BattleManager(IBattleContext battleContext)
        {
            BattleContext = battleContext;

            _turnManager = new TurnManager();
            _passiveManager = new PassiveManager(battleContext);
        }

        public IBattleContext BattleContext { get; }

        public List<BattleLogEvent> BattleLogEvents { get; } = new();

        public void PushSkillSequence(SkillExecutionContext ctx)
        {
            //일련의 스킬 액션들을 역순으로 밀어넣기.

            PushAction(new EffectApplyAction(ctx));
            PushAction(new OnBeforeHitAction(ctx));
            PushAction(new OnTargetingAction(ctx));
        }

        public void PushAction(IBattleAction action)
        {
            actionStack.Push(action);
            //Debug.Log($"Action Stack Push {action.GetType().Name}");
        }

        public List<SkillExecutionData> RequestPassive(SkillTiming timing, SkillExecutionContext ctx)
        {
            return _passiveManager.GetPassiveReact(timing, ctx, BattleContext);
        }

        public void RecordEvent(BattleLogEvent log)
        {
            BattleLogEvents.Add(log);
            //Debug.Log(log.log);
        }


        //여기선 queue의 캐릭터를 가져와서 
        public void StartBattle()
        {
            var safetyCount = 0;

            _turnManager.SetupNewBattle(BattleContext);

            while (true)
            {
                if (actionStack.Count == 0)
                {
                    var caster = _turnManager.GetNextActiveCharacter();
                    if (caster == null)
                    {
                        Debug.Log("Battle End");
                        break; // 전투종료
                    }

                    // 여기서 선택된 캐스터가 액티브 스킬을 사용할 수 있는 상태인지 판단해야함.
                    //사용할 수 없는 상태라면 pass.
                    if (caster.StatSystem.GetStatValue(StatType.AP) < 1)
                    {
                        Debug.Log($"{caster.Faction} do not have ap. skip Active Skill");
                        continue;
                    }

                    //타겟팅 작업 해주기. -> 액티브 스킬 전처리
                    foreach (var skillTacticData in caster.SkillSystem.ActiveSkillTacticQueue)
                    {
                        var target = skillTacticData.GetTarget(caster, BattleContext);
                        if (target.Count == 0 || target == null) continue;
                        //Debug.Log($"Target : {target[0]}");
                        var declareAction = new OnDeclareAction(caster, target, skillTacticData.Skill);
                        //리액션 스택에 넣고 break
                        actionStack.Push(declareAction);
                        break;
                    }
                }

                while (actionStack.Count > 0)
                {
                    safetyCount++;
                    if (safetyCount > 100) break;
                    var lastStack = actionStack.Peek();
                    if (lastStack.IsFinished)
                        //Debug.Log($"Action Stack Pop {lastStack.GetType().Name}");
                        actionStack.Pop();
                    else
                        lastStack.Execute(this, BattleContext);
                }
            }
        }
    }
}