using UnityEngine;
using Survivors.Equipment;

namespace Survivors
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        public static EquipmentManager Equipment => Instance.equipmentManager;
        public static CoinsManager Coins => Instance.coinsManager;

        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private EquipmentManager equipmentManager;
        [SerializeField] private CoinsManager coinsManager;
        
        private PlayerStatsGetter statsGetter;

        internal PlayerStats[] GetPlayerStats()
        {
            statsGetter ??= new PlayerStatsGetter(playerSettings.BaseStats);
            return statsGetter.GetStats();
        }

        internal UniqueEquipmentData GetPlayerFirstPlayerWeapon()
        {
            return playerSettings.FirstPlayerWeapon;
        }
    }
}
