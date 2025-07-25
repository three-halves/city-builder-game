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
        [NonSerialized] public Stats LocalStats;
        [NonSerialized]public float timer;
        // From battle database
        [SerializeField] private int _aiIndex;
        [SerializeField] private bool _startAtFullHealth = true;

        [NonSerialized] public bool showStatusIcon = false;
        [NonSerialized] public int statusSpriteIndex = 0;

        [field: SerializeField] public Sprite Sprite {get; private set;}

        private CharacterAI _ai;

        public float TurnInterval 
        {
            get{
                return Mathf.Max(3f - Stats.Spd * 0.04f, 0.3f);
                }
        }

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
            LocalStats = new(Stats);
            _ai = (CharacterAI)Activator.CreateInstance(BattleManager.Instance.BattleDatabase.AITypes[_aiIndex].GetType());
            _ai.Setup(this);
            if (_startAtFullHealth) HP = Stats.MaxHP;
            timer = TurnInterval + timerOffset;
        }

        // Called each turn in battle
        public void DoTurn(List<BattleCharacter> targetableAllies, List<BattleCharacter> targetableFoes)
        {
            // Next turn timer is determined by AI 
            timer = _ai.DoTurn(this, targetableAllies, targetableFoes);
            // timer = Mathf.Max(3f - Stats.Spd * 0.04f, 0.3f);
        }

        public void Attack(BattleCharacter target, List<BattleCharacter> targetParty)
        {
            target.Damage(LocalStats.Str, this);

            if (LocalStats.SpreadDmg > 0) 
                targetParty.ForEach(x => x.Damage(LocalStats.SpreadDmg, null));

            if (LocalStats.HealOnHit > 0)
                Heal(LocalStats.HealOnHit);
        }

        private void Damage(int damage, BattleCharacter attacker)
        {
            // try dodge
            if (UnityEngine.Random.Range(0f, 1f) < LocalStats.DodgeChance)
            {
                return;
            }

            // Remove HP based on damage
            int effectiveCon = Mathf.Max(LocalStats.Con - ((attacker == null) ? 0 : attacker.LocalStats.IgnoreCon), 0);
            HP = Mathf.Max(HP - Mathf.Max(damage - effectiveCon, 0), 0);

            // Apply thorns
            if (LocalStats.ThornDmg > 0 && attacker != null) 
                attacker.Damage(LocalStats.ThornDmg, null);

            _ai.OnDamage(this);
        }

        public void Heal(int hp)
        {
            if (!BattleManager.Instance.IsBattleOngoing)
            {
                HP = Mathf.Min(HP + hp, Stats.MaxHP);
            }
            else
            {
                HP = Mathf.Min(HP + hp, LocalStats.MaxHP);
            }
        }

        public void Stun(float stunAmount)
        {
            Debug.Log(name + " Stun");
            timer += stunAmount;
        }
    }
}

