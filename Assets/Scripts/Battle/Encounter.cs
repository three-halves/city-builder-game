using System;
using System.Collections.Generic;

namespace Battle 
{
    [Serializable]
    public class Encounter
    {
        // Index from database
        public List<BattleCharacter> foes;
    }

    [Serializable]
    public class EncounterGroup
    {
        public List<Encounter> encounters;
    }
    
}