using System;
using Core.Interfaces;
using UnityEngine;

namespace Core.Data.Targeting
{
    public class TacticObject : ScriptableObject
    {
        [SerializeReference] private ITactic _tactic;


        public ITactic Tactic => _tactic;
        
    }
}