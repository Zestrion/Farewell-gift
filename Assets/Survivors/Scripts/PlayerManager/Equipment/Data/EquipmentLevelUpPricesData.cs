using UnityEngine;

namespace Survivors.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentLevelUpPricesData", menuName = "Survivor/Equipment/EquipmentLevelUpPricesData", order = 100)]
    public class EquipmentLevelUpPricesData : ScriptableObject
    {
        public EquipmentLevelUpPrice[] Prices => prices;
        [SerializeField] private EquipmentLevelUpPrice[] prices;

        public int GetCoinsPrice(int _level, int _defaultPrice = 0)
        {
            if(ContainsPrice(_level))
            {
                return _defaultPrice;
            }
            else
            {
                return prices[_level].CoinsPrice;
            }
        }

        private bool ContainsPrice(int _level)
        {
            return prices.Length <= _level;
        }
    }

    [System.Serializable]
    public struct EquipmentLevelUpPrice
    {
        public int ScrapPrice;
        public int CoinsPrice;
    }
}
