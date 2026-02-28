using Core.Data.Character;
using Core.Enums;
using UnityEngine;

namespace Core.Data.Battle.BattleLogs
{
    public abstract class BattleLogEvent
    {
        public BattleLogType Type { get; protected set; }
        public CharacterInstance Actor;
        public string log;
        
        public BattleLogEvent(BattleLogType type, CharacterInstance actor)
        {
            Type = type;
            Actor = actor;
        }
    }
}