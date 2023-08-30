using System;
using UnityEngine;

namespace Survivors.Equipment
{
    [CreateAssetMenu(fileName = "ConstantEquipmentData", menuName = "Survivor/Equipment/ConstantEquipmentData", order = 1)]
    public class ConstantEquipmentData : ScriptableObject
    {
        public string UniqueIdentifier => uniqueIdentifier;
        public string EquipmentName => equipmentName;
        public string Description => description;
        public Sprite Icon => icon;
        public EquipmentSlotType SlotType => slotType;
        public GameObject ModelPrefab => modelPrefab;
        public GameObject GameplayPrefab => gameplayPrefab;
        public EquipmentValue[] ModelSizes => modelSizes;
        public EquipmentValue[] ModelPositions => modelPositions;
        public EquipmentLevelUpPricesData LevelUpPrices => levelUpPrices;
        public EquipmentStats[] Stats => stats;

        [Tooltip("Must be unique and cannot be changed. Used in save data")]
        [SerializeField] private string uniqueIdentifier;
        [Header("About item")]
        [Tooltip("Must be the same as the equipment type")]
        [SerializeField] private string equipmentName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private EquipmentSlotType slotType;
        [Header("Models")]
        [SerializeField] private GameObject modelPrefab;
        [SerializeField] private GameObject gameplayPrefab;
        [SerializeField] private EquipmentValue[] modelSizes;
        [SerializeField] private EquipmentValue[] modelPositions;
        [Header("Other")]
        [SerializeField] private EquipmentLevelUpPricesData levelUpPrices;
        [SerializeField] private EquipmentStats[] stats;
    }

    [Serializable]
    public class BurnEnemiesStats : EquipmentStats
    {
        [Header("Burn enemies stats")]
        public int EnemiesToBurn;
        public float BurnDuration;
    }

    public enum StatsType
    {
        // base stats
        Damage,
        Health,
        // weapon stats
        AttackSpeed,
        CritChance,
        BurnsEnemies
    }

    public enum EquipmentSlotType
    {
        PrimaryWeapon,
	    SecondaryWeapon,
	    PrimaryArmor,
	    SecondaryArmor
    }

    public enum Rarity
    {
        Common,     // (White)
        Great,      // (Green)
        Rare,       // (Blue)
        Epic,       // (Purple)
        Legendary   // (Orange)
    }

    [Serializable]
    public struct EquipmentValue
    {
        public ValueType ValueType { get => valueType; }
        public Vector3 Value { get => value; }

        [SerializeField] private ValueType valueType;
        [SerializeField] private Vector3 value;
    }

    public enum ValueType
    {
        MainMenuEquipmentScreen,
        DroppedFromEnemy
    }
}
