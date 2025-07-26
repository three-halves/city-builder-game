using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public class CharacterAIRouge : CharacterAI
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

            // level 4: +50% dodge chance
            if (level >= 4) character.LocalStats.DodgeChance += 50;

            // level 5: Ignore 40 CON
            if (level >= 5) character.LocalStats.IgnoreCon += 40;
        }

        public override float DoTurn(BattleCharacter character, List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            float timer = base.DoTurn(character, targetableAllies, targetableFoes);
            if (targetableFoes.Count <= 0) return timer; 

            // target lowest health foe
            if (_target == null || _target.HP <= 0)
                _target = targetableFoes.OrderBy(x => x.HP).First();

            // Level 1: basic attack
            if (level < 3)
            {
                character.Attack(_target, targetableFoes);
            }
            // level 3: attack all
            else 
            {
                targetableFoes.ForEach(foe => character.Attack(foe, targetableFoes));
            }

            // level 2: stun target on hit
            if (level >= 2)
                _target.Stun(character.TurnInterval * 0.5f);
            
            _attackCount++;

            return timer;
        }

        public override void OnDamage(BattleCharacter character)
        {
            base.OnDamage(character);
            _takenDamageCount++;

        }
    }
}