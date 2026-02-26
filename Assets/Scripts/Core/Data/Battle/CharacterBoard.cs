using System.Collections.Generic;
using System.Linq;
using Core.Data.Character;
using Core.Interfaces;

namespace Core.Data.Battle
{
    public class CharacterBoard : IBoard
    {
        public CharacterInstance[] characters;

        public CharacterBoard()
        {
            characters = new CharacterInstance[6];
        }
        public List<CharacterInstance> GetSingleCharacter(int index)
        {
            var result = new List<CharacterInstance>();
            if(characters[index] != null) result.Add(characters[index]);
            return result;
        }

        public List<CharacterInstance> GetAllCharacters()
        {
            return characters.Where(c => c != null).ToList();
        }

        /// <summary>
        ///     가로 행(Row)을 가져옵니다. (상단, 중단, 하단)
        ///     rowIndex: 0(상), 1(중), 2(하)
        /// </summary>
        public List<CharacterInstance> GetCharacterAtRow(int index)
        {
            var result = new List<CharacterInstance>();
            if (index < 0 || index > 2) return result;

            var frontIndex = index;
            var backIndex = index + 3;

            if (characters[frontIndex] != null) result.Add(characters[frontIndex]);
            if (characters[backIndex] != null) result.Add(characters[backIndex]);

            return result;
        }

        /// <summary>
        ///     세로 열(Column)을 가져옵니다. (전열, 후열)
        ///     colIndex: 0(전열), 1(후열)
        /// </summary>
        public List<CharacterInstance> GetCharacterAtColumn(int index)
        {
            var result = new List<CharacterInstance>();
            if (index < 0 || index > 1) return result;

            var startIndex = index * 3;

            for (var i = 0; i < 3; i++)
            {
                var currentIndex = startIndex + i;
                if (characters[currentIndex] != null) result.Add(characters[currentIndex]);
            }

            return result;
        }
    }
}