using UnityEngine;

namespace Survivors.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentStats", menuName = "Survivor/Equipment/EquipmentStats", order = 1)]
    public class EquipmentStats : ScriptableObject
    {
        public Rarity RequiredItemRarity { get => requiredItemRarity; }
        public string Description { get => description; }
        public StatsType StatsType { get => statsType; }
        public int BaseStats { get => baseStats; }
        public bool IsLevelUpAllowed { get => isLevelUpAllowed; }
        public StatsIncreasePerLevelData StatsIncreasePerLevelData { get => statsIncreasePerLevelData; }

        [SerializeField] private Rarity requiredItemRarity;
        [SerializeField] private string description;
        [SerializeField] private StatsType statsType;
        [SerializeField] private int baseStats;
        [Header("Level up")]
        [SerializeField] private bool isLevelUpAllowed;
        [SerializeField] private StatsIncreasePerLevelData statsIncreasePerLevelData;
    }
}
