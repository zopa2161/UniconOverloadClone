using System.Collections.Generic;
using Core.Data.Character;

namespace Core.Interfaces
{
    public interface IBoard
    {
        List<CharacterInstance> GetSingleCharacter(int index);
        List<CharacterInstance> GetAllCharacters();
        List<CharacterInstance> GetCharacterAtRow(int index);
        List<CharacterInstance> GetCharacterAtColumn(int index);

        bool IsAllDead();
    }
}