using System;
using System.Collections.Generic;
using Core.Data.Targeting;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.Tactics
{
    [Serializable]
    public class HighestHpTargetTactic : ITactic
    {
        public List<TargetGroup> EvaluateAndFilterTargetGroups(List<TargetGroup> candidates, IBattleContext ctx)
        {
            if (candidates == null || candidates.Count == 0)
                return new List<TargetGroup>();

            var best = float.NegativeInfinity;

            // 1) 최댓값 찾기
            for (var i = 0; i < candidates.Count; i++)
            {
                var g = candidates[i];
                if (g == null) continue;

                var v = EvaluateGroup(g);
                if (v > best) best = v;
            }

            if (best == float.NegativeInfinity)
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