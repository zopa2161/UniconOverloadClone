using Core.Enums;

namespace Core.Data.Battle
{
    public class SquadData
    {
        private CharacterBoard _board;
        private CharacterFaction _faction;
        
        public CharacterBoard Board { get; set; }
        public CharacterFaction Faction { get; set; }
        
        public SquadData(CharacterBoard board)
        {
            _board = board;
        }
    }
}