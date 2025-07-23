using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle
{
    public class Battle 
    {
        public List<BattleCharacter> Allies {get; private set;}
        public List<BattleCharacter> Foes {get; private set;}
        public List<BattleCharacter> AllCharacters {get; private set;}

        public enum BattleStatus
        {
            Running,
            Won,
            Lost
        }

        public Battle(List<BattleCharacter> allies, List<BattleCharacter> foes)
        {
            // Allies = allies.ConvertAll(c => Object.Instantiate(c));
            Allies = allies;
            Foes = foes.ConvertAll(c => Object.Instantiate(c));
            AllCharacters = new(Foes);
            AllCharacters.AddRange(allies);

            foreach (var character in Foes)
            {
                // offset enemy attack timers initially so attacks aren't synced
                character.Setup(Random.Range(0.25f, 0.5f));
            }
            foreach (var character in allies)
            {
                character.Setup();
            }
        }

        /// <summary>
        /// Runs one battle timestep for all characters
        /// </summary>
        /// <param name="dt"></param>
        public BattleStatus Run(float dt)
        {
            // Run all characters turn if applicable
            foreach(var character in AllCharacters)
            {
                if (character.HP <= 0)
                {
                    continue;
                }
                character.timer -= dt;
                if (character.timer <= 0) character.DoTurn(Allies.Where(x => x.HP > 0).ToList(), Foes.Where(x => x.HP > 0).ToList());
            }

            // check if battle is ended
            bool allAlliesDead = true;
            bool allFoesDead = true;

            foreach(var character in Allies)
            {
                allAlliesDead &= character.HP <= 0;
            }

            foreach(var character in Foes)
            {
                allFoesDead &= character.HP <= 0;
            }

            if (!allAlliesDead && !allFoesDead) return BattleStatus.Running;
            else if (allAlliesDead) return BattleStatus.Lost;
            else return BattleStatus.Won;
        }
    }
}