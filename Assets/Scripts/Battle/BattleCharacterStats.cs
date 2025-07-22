using System;

namespace Battle
{
    public enum StatType 
    {
        MaxHP,
        Str,
        Con,
        Wis,
        Spd
    }

    [Serializable]
    public class Stats
    {
        public int MaxHP;
        public int Str;
        public int Con;
        public int Wis;
        public int Spd;

        public void ChangeStat(StatType stat, int delta)
        {
            switch(stat)
            {
                case StatType.MaxHP: MaxHP += delta; break;
                case StatType.Str: Str += delta; break;
                case StatType.Con: Con += delta; break;
                case StatType.Wis: Wis += delta; break;
                case StatType.Spd: Spd += delta; break;
            }
        }
    }
}