using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Battle.Battle;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance {get; private set;}
        [SerializeField] public BattleDatabase BattleDatabase;
        [SerializeField] private List<BattleCharacter> _playerCharacters;
        public List<BattleCharacter> PlayerCharacters {get; private set;}
        [SerializeField] private BattleView _view;
        // If a battle is currently happening
        public bool IsBattleOngoing {get; private set;}
        [SerializeField] private float _healInterval = 1f;
        private float _healTimer;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                PlayerCharacters = _playerCharacters.ConvertAll(c => Instantiate(c));
            }
            else{
                Destroy(gameObject);
            }
        }

        public delegate void BattleCompleteDelegate(bool won);
        public IEnumerator StartBattle(int difficulty, Tile.Biome biome, Vector3 viewPosition, BattleCompleteDelegate battleCompleteDelegate, List<BattleCharacter> foes)
        {
            IsBattleOngoing = true;

            // Initialize battle
            Battle battle = new(PlayerCharacters, foes);
            BattleStatus status = BattleStatus.Running;
            _view.Setup(battle, viewPosition);
            // Run battle until over
            while (status == BattleStatus.Running)
            {
                _view.Refresh();
                status = battle.Run(Time.deltaTime);
                yield return null;
            }

            // battle over
            battleCompleteDelegate.Invoke(status == BattleStatus.Won);
            _view.Hide();
            IsBattleOngoing = false;
            GiveBattleReward(difficulty);
        }

        public void GiveBattleReward(int difficulty)
        {
            GameState.Instance.Cash += Mathf.Pow(5, difficulty * 0.15f + 0.5f) + difficulty;
        }

        public delegate void OnPartyMemberAdded();
        public event OnPartyMemberAdded PartyMemberAddedListener;
        
        public void AddPartyMember(BattleCharacter character)
        {
            PlayerCharacters.Add(character);
            PartyMemberAddedListener?.Invoke();
        }

        void Start()
        {
            _healTimer = _healInterval;
            IsBattleOngoing = false;
        }

        void Update()
        {
            if (!IsBattleOngoing)
            {
                _healTimer -= Time.deltaTime;
                if (_healTimer <= 0)
                {
                    foreach (BattleCharacter bc in PlayerCharacters)
                    {
                        bc.Heal(1);
                    }
                    _healTimer = _healInterval;
                }

            }
        }
    }
}