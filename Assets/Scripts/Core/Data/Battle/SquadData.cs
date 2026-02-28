using Core.Enums;
using UnityEngine;

namespace Core.Data.Battle
{
    public class SquadData
    {
        private string _squadName;
        private CharacterBoard _board;

        public CharacterBoard Board => _board;
        public CharacterFaction Faction { get; set; }
        public string SquadName { get; set; }
        
        public SquadData(CharacterBoard board)
        {
            _board = board;
        }
    }
}