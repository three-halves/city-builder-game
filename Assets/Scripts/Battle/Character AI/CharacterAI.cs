using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CharacterAI
    {
        public virtual void Setup(BattleCharacter character)
        {

        }

        // returns the time delay until the next turn
        public virtual float DoTurn(BattleCharacter character, List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            return Mathf.Max(3f - character.Stats.Spd * 0.04f, 0.3f);
        }
    }
}
