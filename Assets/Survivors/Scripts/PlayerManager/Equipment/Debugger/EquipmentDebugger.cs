using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Equipment
{
    public class EquipmentDebugger : MonoBehaviour
    {
        [SerializeField] private ConstantEquipmentData dataToAdd;
        [SerializeField] List<EquipmentData> loadedData;

        [ContextMenu("Add item to equipped items")]
        private void AddItemToEquippedItems()
        {
            UniqueEquipmentData _uniqueData = PlayerManager.Equipment.CreateNewEquipmentData(dataToAdd.UniqueIdentifier, Rarity.Common, 1).UniqueEquipmentData;
            PlayerManager.Equipment.EquipEquipment(_uniqueData);
        }

        [ContextMenu("Add item to collection")]
        private void AddItemToCollection()
        {
            UniqueEquipmentData _uniqueData = PlayerManager.Equipment.CreateNewEquipmentData(dataToAdd.UniqueIdentifier, Rarity.Common, 1).UniqueEquipmentData;
            PlayerManager.Equipment.AddEquipmentToCollection(_uniqueData);
        }

        [ContextMenu("LoadCollection")]
        private void LoadCollectedCollection()
        {
            LoadCollection();
        }

        [ContextMenu("Load using equipment")]
        private void LoadUsingEquipment()
        {
            LoadUsing();
        }

        private void LoadUsing()
        {
            loadedData = PlayerManager.Equipment.GetEquipmentData(EquipmentManager.DataType.Equipped);
        }

        private void LoadCollection()
        {
            loadedData = PlayerManager.Equipment.GetEquipmentData(EquipmentManager.DataType.Collected);
        }
    }
}
