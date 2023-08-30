using UnityEngine;
using Survivors.Equipment;

namespace Survivors
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Survivor/Player/PlayerSettings", order = 1)]
    internal class PlayerSettings : ScriptableObject
    {
        internal PlayerStats[] BaseStats => baseStats;
        internal UniqueEquipmentData FirstPlayerWeapon => firstPlayerWeapon;

        [SerializeField] private PlayerStats[] baseStats;
        [Tooltip("This weapon will be used when the player starts the game for the first time")]
        [SerializeField] private UniqueEquipmentData firstPlayerWeapon;
    }
}
