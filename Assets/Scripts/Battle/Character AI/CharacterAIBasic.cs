using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CharacterAIBasic : CharacterAI
    {
        public override float DoTurn(BattleCharacter character, List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            float timer = base.DoTurn(character, targetableAllies, targetableFoes);
            if (targetableFoes.Count > 0)
            {
                BattleCharacter target = targetableAllies[Random.Range(0, targetableAllies.Count)];
                target.Damage(character.Stats.Str);
            }

            return timer;
        }
    }
}