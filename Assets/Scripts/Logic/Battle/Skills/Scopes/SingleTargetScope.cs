using System;
using System.Collections.Generic;
using Core.Data.Character;
using Core.Data.Targeting;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Logic.Battle.Skills.Scopes
{
    [Serializable]
    public class SingleTargetScope : IScope
    {
        [SerializeField] private bool _isFriendly;

        public List<TargetGroup> GetCandidateGroups(CharacterInstance caster, IBattleContext ctx)
        {
            var casterIsFriendly = caster.Faction == CharacterFaction.Friendly;

            // 같은 진영을 타깃? (true면 friendlyBoard, false면 enemyBoard)
            // casterIsFriendly == _isFriendly 이면 friendlyBoard, 아니면 enemyBoard
            var board = casterIsFriendly == _isFriendly ? ctx.friendlyBoard : ctx.enemyBoard;

            var candidates = new List<TargetGroup>();
            for (var i = 0; i < 6; i++)
            {
                var ch = board.GetSingleCharacter(i);
                if (ch == null || ch.Count == 0 || ch[0].IsDead ) continue;
                candidates.Add(new TargetGroup(ch));
            }

            //Debug.Log($"singleTargetScope : {candidates.Count}");
            return candidates;
        }

        public object Clone()
        {
            var result = new SingleTargetScope();
            result._isFriendly = _isFriendly;
            return result;
        }
    }
}