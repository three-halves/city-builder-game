using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CharacterAIMultiHit : CharacterAI
    {
        private int _attackCount;
        public override float DoTurn(BattleCharacter character, List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            float timer = base.DoTurn(character, targetableAllies, targetableFoes);
            if (targetableFoes.Count > 0)
            {
                BattleCharacter target = targetableAllies[Random.Range(0, targetableAllies.Count)];
                character.Attack(target, targetableAllies);
                _attackCount++;
            }

            return (_attackCount % 3 > 0) ? 0.2f : timer;
        }
    }
}