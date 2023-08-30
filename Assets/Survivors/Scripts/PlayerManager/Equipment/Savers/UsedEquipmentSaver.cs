using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Equipment
{
    public class UsedEquipmentSaver : EquipmentSaverBase
    {
        protected override string saveKey => "player_used_equipment_save";

        internal override List<UniqueEquipmentData> GetEquipment()
        {
            if (!IsSaveDataExist())
            {
                EquipFirstEquipment();
            }
            return base.GetEquipment();
        }

        private bool IsSaveDataExist()
        {
            return PlayerPrefs.HasKey(saveKey);
        }

        private void EquipFirstEquipment()
        {
            UniqueEquipmentData _firstWeaponUniqueData = PlayerManager.Instance.GetPlayerFirstPlayerWeapon();
            EquipmentData _firstWeaponEquipmentData = PlayerManager.Equipment.CreateNewEquipmentData(
                _firstWeaponUniqueData.ConstantDataIndentifier,
                _firstWeaponUniqueData.Rarity,
                _firstWeaponUniqueData.Level);
            //AddEquipment(_firstWeaponEquipmentData.UniqueEquipmentData);
        }
    }
}
