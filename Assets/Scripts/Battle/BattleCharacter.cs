using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    [Serializable]
    public class BattleCharacter
    {
        public int HP { get; private set; }
        [SerializeField] public Stats Stats;
        public float timer;
        // From battle database
        [SerializeField] private int _aiIndex;
        [SerializeField] private bool _startAtFullHealth = true;

        // copy contructor
        public BattleCharacter(BattleCharacter other)
        {
            HP = other.HP;
            Stats = other.Stats;
            timer = other.timer;
            _aiIndex = other._aiIndex;
            SpriteIndex = other.SpriteIndex;
            _startAtFullHealth = other._startAtFullHealth;
            EXPBuildingIndex = other.EXPBuildingIndex;
        }

        [field: SerializeField] public int SpriteIndex {get; private set;}
        [field: SerializeField] public int EXPBuildingIndex {get; private set;} = -1;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Start()
        {
            
        }

        // Update is called once per frame
        public void Update()
        {
            
        }

        // Called on battle start
        public void Setup(float timerOffset = 0f)
        {
            BattleManager.Instance.BattleDatabase.AITypes[_aiIndex].Setup(this);
            if (_startAtFullHealth) HP = Stats.MaxHP;
            timer = Mathf.Max(3f - Stats.Spd * 0.04f, 0.3f) + timerOffset;
        }

        // Called each turn in battle
        public void DoTurn(List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            // Next turn timer is determined by AI 
            timer = BattleManager.Instance.BattleDatabase.AITypes[_aiIndex].DoTurn(this, targetableAllies, targetableFoes);
            // timer = Mathf.Max(3f - Stats.Spd * 0.04f, 0.3f);
        }

        public void Damage(int damage)
        {
            HP = Mathf.Max(HP - (damage - Stats.Con), 0);
        }

        public void Heal(int hp)
        {
            HP = Mathf.Min(HP + hp, Stats.MaxHP);
        }
    }
}

