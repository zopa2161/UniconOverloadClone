using System;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public abstract class BattleLogEvent
    {
        public CharacterInstance Actor;
        public string log;

        public BattleLogEvent(BattleLogType type, CharacterInstance actor)
        {
            Type = type;
            Actor = actor;
        }

        protected BattleLogEvent()
        {
            throw new NotImplementedException();
        }

        public BattleLogType Type { get; protected set; }
    }
}