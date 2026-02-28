using Core.Enums;

namespace Core.Data.Battle
{
    public class SquadData
    {
        private string _squadName;

        public SquadData(CharacterBoard board)
        {
            Board = board;
        }

        public CharacterBoard Board { get; }

        public CharacterFaction Faction { get; set; }
        public string SquadName { get; set; }
    }
}