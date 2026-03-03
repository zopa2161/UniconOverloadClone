using System;
using System.Collections.Generic;
using Core.Data.Battle;
using Core.Data.Targeting;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.Tactics
{
    [Serializable]
    public class LowestHpTargetTactic : ITactic
    {
        public List<TargetGroup> EvaluateAndFilterTargetGroups(List<TargetGroup> candidates, IBattleContext ctx,
            SkillExecutionContext skillContext)
        {
            if (candidates == null || candidates.Count == 0)
                return new List<TargetGroup>();

            var best = float.PositiveInfinity;

            // 1) 최솟값 찾기
            for (var i = 0; i < candidates.Count; i++)
            {
                var g = candidates[i];
                if (g == null) continue;

                var v = EvaluateGroup(g);
                if (v < best) best = v;
            }

            if (float.IsPositiveInfinity(best))
                return new List<TargetGroup>();

            // 2) 동점 포함해서 결과 만들기
            var result = new List<TargetGroup>();
            for (var i = 0; i < candidates.Count; i++)
            {
                var g = candidates[i];
                if (g == null) continue;

                var v = EvaluateGroup(g);
                if (Mathf.Approximately(v, best))
                    result.Add(g);
            }

            return result;
        }

        // 그룹 점수: 그룹 내 타깃 중 "최소 HP"가 아니라 "최대 HP"였던 네 기존 정의를 그대로 사용.
        // (즉, 각 그룹에서 HP가 가장 높은 1명을 뽑고, 그 값이 가장 낮은 그룹을 선택)
        private static float EvaluateGroup(TargetGroup group)
        {
            var hp = float.NegativeInfinity;

            var targets = group.Targets;
            if (targets == null) return hp;

            for (var i = 0; i < targets.Count; i++)
            {
                var t = targets[i];
                if (t == null) continue;

                hp = Mathf.Max(hp, t.GetStatValue(StatType.HP));
            }

            return hp;
        }
    }
}