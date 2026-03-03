using System;
using System.Collections.Generic;
using Core.Data.Character;
using Core.Enums;

namespace Core.Data.Battle.BattleLogs
{
    public abstract class BattleLogEvent
    {
        public CharacterInstance Actor;
        public List<CharacterInstance> Targets;
        public string log;

        public BattleLogEvent(BattleLogType type, CharacterInstance actor, List<CharacterInstance> targets)
        {
            Type = type;
            Actor = actor;
            Targets = targets;
        }

        protected BattleLogEvent()
        {
            throw new NotImplementedException();
        }

        public BattleLogType Type { get; protected set; }
    }
}