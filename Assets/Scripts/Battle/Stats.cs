using System;

namespace Battle
{
    public enum StatType 
    {
        MaxHP,
        Str,
        Con,
        Wis,
        Spd,
        Exp
    }

    [Serializable]
    public class Stats
    {
        public int MaxHP;
        public int Str;
        public int Con;
        public int Wis;
        public int Spd;
        [NonSerialized] public int Exp = 0;

        public Stats(Stats stats)
        {
            MaxHP = stats.MaxHP;
            Str = stats.Str;
            Con = stats.Con;
            Wis = stats.Wis;
            Spd = stats.Spd;
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