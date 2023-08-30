using System;
using Survivors.Equipment;

namespace Survivors
{
    [Serializable]
    internal struct PlayerStats
    {
        public StatsType StatsType;
        public int Stats;
    }
}
