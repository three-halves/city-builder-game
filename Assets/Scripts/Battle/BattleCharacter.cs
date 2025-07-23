using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle 
{
    [CreateAssetMenu(fileName = "BattleCharacter", menuName = "BattleCharacter")]
    public class BattleCharacter : ScriptableObject
    {
        public int HP { get; private set; }
        [field: SerializeField] public string CharacterName {get; private set;} = "[UNKNOWN]";
        [SerializeField] public Stats Stats;
        public Stats LocalStats;
        [NonSerialized]public float timer;
        // From battle database
        [SerializeField] private int _aiIndex;
        [SerializeField] private bool _startAtFullHealth = true;

        [NonSerialized] public bool showStatusIcon = false;
        [NonSerialized] public int statusSpriteIndex = 0;

        [field: SerializeField] public Sprite Sprite {get; private set;}

        // copy contructor
        public BattleCharacter(BattleCharacter other)
        {
            HP = other.HP;
            Stats = other.Stats;
            timer = other.timer;
            _aiIndex = other._aiIndex;
            _startAtFullHealth = other._startAtFullHealth;
            Sprite = other.Sprite;
            CharacterName = other.CharacterName;
        }

        // [field: SerializeField] public int SpriteIndex {get; private set;}
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
            showStatusIcon = false;
            BattleManager.Instance.BattleDatabase.AITypes[_aiIndex].Setup(this);
            if (_startAtFullHealth) HP = Stats.MaxHP;
            Debug.Log(name + " " + HP);
            timer = Mathf.Max(3f - Stats.Spd * 0.04f, 0.3f) + timerOffset;
            LocalStats = new(Stats);
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
            HP = Mathf.Max(HP - Mathf.Max(damage - LocalStats.Con, 0), 0);
            BattleManager.Instance.BattleDatabase.AITypes[_aiIndex].OnDamage(this);
        }

        public void Heal(int hp)
        {
            if (BattleManager.Instance.IsBattleOngoing)
            {
                HP = Mathf.Min(HP + hp, Stats.MaxHP);
            }
            else
            {
                HP = Mathf.Min(HP + hp, LocalStats.MaxHP);
            }
        }
    }
}

