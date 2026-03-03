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
    public class AllTargetScope : IScope
    {
        [SerializeField] private bool _isFriendly;

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public List<TargetGroup> GetCandidateGroups(CharacterInstance caster, IBattleContext ctx)
        {
            var casterIsFriendly = caster.Faction == CharacterFaction.Friendly;
            var board = casterIsFriendly == _isFriendly ? ctx.friendlyBoard : ctx.enemyBoard;
            var candidates = new List<TargetGroup>();

            var ch = board.GetAllCharacters();
            candidates.Add(new TargetGroup(ch));
            return candidates;
        }
    }
}