using System.Linq;
using System.Collections.Generic;
using Survivors.Equipment;

namespace Survivors
{
    internal class PlayerStatsGetter
    {
        private readonly PlayerStats[] baseStats;
        private readonly StatsType[] statsTypesToGive = new StatsType[] {
            StatsType.Health,
            StatsType.Damage
        };

        internal PlayerStatsGetter(PlayerStats[] _baseStats)
        {
            baseStats = _baseStats;
        }

        internal PlayerStats[] GetStats()
        {
            List<EquipmentData> _equippedEquipment = GetEquippedEquipmentData();
            PlayerStats[] _playerStats = new PlayerStats[statsTypesToGive.Length];
            for (int i = 0; i < statsTypesToGive.Length; i++)
            {
                _playerStats[i] = new PlayerStats()
                {
                    StatsType = statsTypesToGive[i],
                    Stats = GetBasePlayerStatsValue(statsTypesToGive[i])
                        + GetStatsFromEquippedEquipment(_equippedEquipment, statsTypesToGive[i])
                };
            }
            return _playerStats;
        }

        private List<EquipmentData> GetEquippedEquipmentData()
        {
            return PlayerManager.Equipment.GetEquipmentData(EquipmentManager.DataType.Equipped);
        }

        private int GetBasePlayerStatsValue(StatsType _statsType)
        {
            return baseStats.FirstOrDefault(stats => stats.StatsType == _statsType).Stats;
        }

        private int GetStatsFromEquippedEquipment(List<EquipmentData> _equipment, StatsType _statsType)
        {
            int _stats = 0;
            for (int i = 0; i < _equipment.Count; i++)
            {
                _stats += GetStatsFromEquipment(_equipment[i], _statsType);
            }
            return _stats;
        }

        private int GetStatsFromEquipment(EquipmentData _data, StatsType _statsType)
        {
            int _statsValue = 0;
            int _equipmentLevel = _data.UniqueEquipmentData.Level;
            EquipmentStats[] _equipmentStats = _data.ConstantEquipmentData.Stats;
            for (int i = 0; i < _equipmentStats.Length; i++)
            {
                if (_equipmentStats[i].StatsType == _statsType
                    && _equipmentStats[i].RequiredItemRarity <= _data.UniqueEquipmentData.Rarity)
                {
                    _statsValue += _equipmentStats[i].BaseStats;
                    if(_equipmentStats[i].IsLevelUpAllowed)
                    {
                        _statsValue += GetStatsIncreaseForEachEquipmentLevel(_equipmentLevel, _equipmentStats[i].StatsIncreasePerLevelData);
                    }
                }
            }
            return _statsValue;
        }

        private int GetStatsIncreaseForEachEquipmentLevel(int _equipmentLevel, StatsIncreasePerLevelData _statsIncreaseData)
        {
            return _statsIncreaseData.GetStatsIncrease(_equipmentLevel);
        }
    }
}
