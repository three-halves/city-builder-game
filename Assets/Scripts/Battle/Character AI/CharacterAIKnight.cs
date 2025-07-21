using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CharacterAIKnight : CharacterAI
    {
        private int _EXPBuildingIndex = 1;
        private int level = 1;
        private int _attackCount = 0;

        public override void Setup(BattleCharacter character)
        {
            base.Setup(character);
            _attackCount = 0;
            // calculate hero level, get all EXP buildings
            level = BattleManager.Instance.CalculateEXP(_EXPBuildingIndex);
        }

        public override float DoTurn(BattleCharacter character, List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            float timer = base.DoTurn(character, targetableAllies, targetableFoes);

            // Level 1: basic attack
            BattleCharacter target = targetableFoes[Random.Range(0, targetableFoes.Count)];
            target.Damage(character.Stats.Str);
            _attackCount++;

            // Level 2: double attack
            if (level >= 2 && _attackCount % 2 == 1) return 0.2f;
            else return timer;
 
        }
    }
}