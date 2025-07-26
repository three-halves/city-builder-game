using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    [CreateAssetMenu(fileName = "Battle Database", menuName = "Battle Database")]
    public class BattleDatabase : ScriptableObject
    {
        // [SerializeField] public List<BattleCharacter> Foes;
        public List<CharacterAI> AITypes = new()
        {
            new CharacterAIKnight(),
            new CharacterAIBasic(),
            new CharacterAIMultiHit(),
            new CharacterAIRouge()
        };

        [SerializeField] public List<int> levelUpThresholds = new();

        // List of Encounter tables indexed by difficulty
        [field: SerializeField] public List<EncounterGroup> EncounterTable { get; private set; }
    }
}