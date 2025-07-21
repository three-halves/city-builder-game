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
        [field: SerializeField] public List<BattleCharacter> PlayerCharacters {get; private set;}
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
            GameState.Instance.Cash += Mathf.Pow(5, difficulty * 0.15f + 0.5f);
        }

        public List<BattleCharacter> IntToFoeList(List<int> intList)
        {
            List<BattleCharacter> bcs = new();
            foreach (int i in intList)
            {
                bcs.Add(new BattleCharacter(BattleDatabase.Foes[i]));
            }
            return bcs;

        }

        void Start()
        {
            _healTimer = _healInterval;
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

        public int CalculateEXP(int index)
        {
            int level = 1;
            int buildingCount = BuildingManager.Instance.TotalBuiltCount[index];
            foreach (int threshold in BattleDatabase.levelUpThresholds)
            {
                if (buildingCount >= threshold) level++;
            }
            return level;
        }
    }
}