using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CharacterAIKnight : CharacterAI
    {
        private int level = 1;
        private int _attackCount = 0;
        private int _takenDamageCount = 0;
        private BattleCharacter _target;

        public override void Setup(BattleCharacter character)
        {
            base.Setup(character);
            _attackCount = 0;
            _takenDamageCount = 0;
            level = character.Stats.Level;

            // level 4: heal on hit
            if(level >= 4) character.LocalStats.HealOnHit += 3;
        }

        public override float DoTurn(BattleCharacter character, List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            float timer = base.DoTurn(character, targetableAllies, targetableFoes);
            if (targetableFoes.Count <= 0) return timer; 

            if (_target == null || _target.HP <= 0) _target = targetableFoes[Random.Range(0, targetableFoes.Count)];

            // Level 1: basic attack
            character.Attack(_target, targetableFoes);
            _attackCount++;

            // Level 2: double attack
            // level 5: triple attack
            if ((level >= 2 && level < 5 && _attackCount % 2 == 1) || (level >= 5 && _attackCount % 3 > 0)) return 0.2f;
            else return timer;
        }

        public override void OnDamage(BattleCharacter character)
        {
            base.OnDamage(character);
            _takenDamageCount++;
            // level 3: block every 2nd attack
            if (level >= 3)
            {
                if (_takenDamageCount % 2 == 1)
                {
                    character.showStatusIcon = true;
                    character.statusSpriteIndex = 0;
                    character.LocalStats.Con *= 3;
                }
                else
                {
                    character.showStatusIcon = false;
                    character.LocalStats.Con /= 3;
                };
            }
        }
    }
}