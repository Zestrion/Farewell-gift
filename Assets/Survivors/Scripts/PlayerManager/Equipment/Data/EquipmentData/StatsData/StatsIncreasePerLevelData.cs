using UnityEngine;

namespace Survivors.Equipment
{
    [CreateAssetMenu(fileName = "StatsIncreasePerLevelData", menuName = "Survivor/Equipment/StatsIncreasePerLevelData", order = 100)]
    public class StatsIncreasePerLevelData : ScriptableObject
    {
        public int[] StatsIncreasePerLevel => statsIncreasePerLevel;
        [SerializeField] private int[] statsIncreasePerLevel;

        public int GetStatsIncrease(int _level)
        {
            int _statsIncrease = 0;
            for (int i = 0; i < _level; i++)
            {
                if(statsIncreasePerLevel.Length > i)
                {
                    _statsIncrease += statsIncreasePerLevel[i];
                }
                else
                {
                    break;
                }
            }
            return _statsIncrease;
        }

        public int TryGetNextLevelStatsIncrease(int _nextLevel, int _defaultValue = 0)
        {
            if (statsIncreasePerLevel.Length > _nextLevel - 1)
            {
                return statsIncreasePerLevel[_nextLevel - 1];
            }
            else
            {
                return _defaultValue;
            }
        }
    }
}
