using System;
using UnityEngine;

namespace Battle
{
    public enum StatType 
    {
        MaxHP,
        Str,
        Con,
        Wis,
        Spd,
        Exp,
        DodgeChance,
        SpreadDmg,
        IgnoreDefense,
        HealOnHit,
        ThornDmg,
        RewardMultiplier,
    }

    // TODO: Stats should be stored in a dict with field getters/setters. Currently done this way to make unity serialization more convinent
    [Serializable]
    public class Stats
    {
        [Header("Main Stats")]
        public int MaxHP;
        public int Str;
        public int Con;
        public int Wis;
        public int Spd;
        [NonSerialized] public int Exp = 0;

        [Header("Special Stats")]
        [Range(0f, 1f)] public float DodgeChance;
        public int SpreadDmg;
        public int IgnoreCon;
        public int HealOnHit;
        public int ThornDmg;
        public float RewardMultiplier = 1f;

        public Stats(Stats stats)
        {
            MaxHP = stats.MaxHP;
            Str = stats.Str;
            Con = stats.Con;
            Wis = stats.Wis;
            Spd = stats.Spd;
            DodgeChance = stats.DodgeChance;
            SpreadDmg = stats.SpreadDmg;
            IgnoreCon = stats.IgnoreCon;
            HealOnHit = stats.HealOnHit;
            ThornDmg = stats.ThornDmg;
            RewardMultiplier = stats.RewardMultiplier;
        }

        public int Level {get{
            return GetLevel();
        }}

        public void ChangeStat(StatType stat, int delta)
        {
            switch(stat)
            {
                case StatType.MaxHP: MaxHP += delta; break;
                case StatType.Str: Str += delta; break;
                case StatType.Con: Con += delta; break;
                case StatType.Wis: Wis += delta; break;
                case StatType.Spd: Spd += delta; break;
                case StatType.Exp: Exp += delta; break;
                case StatType.DodgeChance: DodgeChance += delta; break;
                case StatType.SpreadDmg: SpreadDmg += delta; break;
                case StatType.IgnoreDefense: IgnoreCon += delta; break;
                case StatType.HealOnHit: HealOnHit += delta; break;
                case StatType.ThornDmg: ThornDmg += delta; break;
                case StatType.RewardMultiplier: RewardMultiplier += delta; break;
            }
        }

        public int GetLevel()
        {
            int level = 1;
            foreach (int threshold in BattleManager.Instance.BattleDatabase.levelUpThresholds)
            {
                if (Exp >= threshold) level++;
            }
            return level;
        }
    }
}