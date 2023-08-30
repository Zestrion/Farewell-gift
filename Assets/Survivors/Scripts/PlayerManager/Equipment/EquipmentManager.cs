using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Equipment
{
    public class EquipmentManager : MonoBehaviour
    {
        public RaritySettingsData RaritySettings => raritySettings;

        [SerializeField] private EquipmentConstantDataHolder equipmentConstantDataHolder;
        [SerializeField] private RaritySettingsData raritySettings;

        private CollectedEquipmentSaver collectedEquipmentSaver;
        private UsedEquipmentSaver usedEquipmentSaver;

        private void Awake()
        {
            collectedEquipmentSaver = new CollectedEquipmentSaver();
            usedEquipmentSaver = new UsedEquipmentSaver();
        }

        /// <summary>
        /// Check if the player contains the equipment.
        /// </summary>
        public bool ContainsEquipment(UniqueEquipmentData _dataToCheck)
        {
            return IsUniqueDataEquipped(_dataToCheck) || IsUniqueDataInCollection(_dataToCheck);
        }

        public void AddEquipmentToCollection(UniqueEquipmentData _collectedData)
        {
            if (!IsUniqueDataInCollection(_collectedData))
            {
                collectedEquipmentSaver.AddEquipment(_collectedData);
            }
        }

        public void EquipEquipment(UniqueEquipmentData _data)
        {
            // check if already equipped
            if (IsUniqueDataEquipped(_data))
            {
                return;
            }
            // try removing from collection to add into equipment
            TryRemoveEquipmentFromCollection(_data);
            // try removing already equipped equipment from slot
            ConstantEquipmentData _equipmentToEquipConstantData = equipmentConstantDataHolder.GetData(_data.ConstantDataIndentifier);
            TryRemoveEquipmentFromSlot(_equipmentToEquipConstantData.SlotType);
            // equip new equipment
            usedEquipmentSaver.AddEquipment(_data);
        }

        public void UnequipEquipment(UniqueEquipmentData _data)
        {
            usedEquipmentSaver.RemoveEquipment(_data);
            AddEquipmentToCollection(_data);
        }

        public bool CanLevelUpEquipment(EquipmentData _data)
        {
            return !IsMaxLevelEquipment(_data) && IsEnoughCoins(GetLevelUpCoinsPrice(_data));
            bool IsEnoughCoins(int _price) => PlayerManager.Coins.IsEnoughCoins(_price);
        }

        public bool IsMaxLevelEquipment(EquipmentData _data)
        {
            return _data.ConstantEquipmentData.LevelUpPrices.Prices.Length <= _data.UniqueEquipmentData.Level
                || raritySettings.GetRarityMaxLevel(_data.UniqueEquipmentData.Rarity) <= _data.UniqueEquipmentData.Level;
        }

        public int GetLevelUpCoinsPrice(EquipmentData _equipmentData)
        {
            return _equipmentData.ConstantEquipmentData.LevelUpPrices.Prices[_equipmentData.UniqueEquipmentData.Level].CoinsPrice;
        }

        public bool TryLevelUpEquipment(EquipmentData _dataToLevelUp, out EquipmentData _newLevelData)
        {
            if (CanLevelUpEquipment(_dataToLevelUp))
            {
                RemoveCoins(GetLevelUpCoinsPrice(_dataToLevelUp));
                UniqueEquipmentData _updatedUniqueData = _dataToLevelUp.UniqueEquipmentData;
                _updatedUniqueData.Level++;
                UpdateEquipmentData(_updatedUniqueData);
                _newLevelData = _dataToLevelUp;
                _newLevelData.UniqueEquipmentData = _updatedUniqueData;
                return true;
            }
            _newLevelData = null;
            return false;

            void RemoveCoins(int _coinsCount) => PlayerManager.Coins.RemoveCoins(_coinsCount);
        }

        /// <summary>
        /// Updates existing equipment data, used when equipment level changes
        /// </summary>
        public void UpdateEquipmentData(UniqueEquipmentData _data)
        {
            if(ContainsEquipment(_data))
            {
                if (IsUniqueDataEquipped(_data))
                {
                    usedEquipmentSaver.UpdateEquipment(_data);
                }
                else if (IsUniqueDataInCollection(_data))
                {
                    collectedEquipmentSaver.UpdateEquipment(_data);
                }
            }
            else
            {
                Debug.LogError("EquipmentManager: can't update equipment, player don't contains this equipment " + _data.ConstantDataIndentifier + " " + _data.EquipmentId);
            }
        }

        /// <summary>
        /// Get all equipped or collected equipment data
        /// </summary>
        public List<EquipmentData> GetEquipmentData(DataType _dataType)
        {
            List<EquipmentData> equipmentData = new List<EquipmentData>();
            List<UniqueEquipmentData> _uniqueData = _dataType == DataType.Equipped ? GetEquippedEquipmentUniqueData() : GetEquipmentCollectionUniqueData();
            ConstantEquipmentData _temporaryConstantData;
            EquipmentData _temporaryEquipmentData;
            for (int i = 0; i < _uniqueData.Count; i++)
            {
                _temporaryConstantData = equipmentConstantDataHolder.GetData(_uniqueData[i].ConstantDataIndentifier);
                if (_temporaryConstantData != null)
                {
                    _temporaryEquipmentData = new EquipmentData()
                    {
                        ConstantEquipmentData = _temporaryConstantData,
                        UniqueEquipmentData = _uniqueData[i],
                    };
                    equipmentData.Add(_temporaryEquipmentData);
                }
            }
            return equipmentData;
        }

        public EquipmentData CreateNewEquipmentData(string _constantDataIndentifier, Rarity _rarity, int _level)
        {
            ConstantEquipmentData _constantData = equipmentConstantDataHolder.GetData(_constantDataIndentifier);
            if (_constantData == null)
            {
                Debug.LogError("Unable to create new equipment data, constant data not found: " + _constantDataIndentifier);
                return null;
            }
            UniqueEquipmentData _uniqueData = new UniqueEquipmentData()
            {
                ConstantDataIndentifier = _constantDataIndentifier,
                Rarity = _rarity,
                Level = _level
            };
            EquipmentData _equipmentData = new EquipmentData()
            {
                ConstantEquipmentData = _constantData,
                UniqueEquipmentData = _uniqueData
            };
            return _equipmentData;
        }

        private List<UniqueEquipmentData> GetEquippedEquipmentUniqueData()
        {
            return usedEquipmentSaver.GetEquipment();
        }

        private List<UniqueEquipmentData> GetEquipmentCollectionUniqueData()
        {
            return collectedEquipmentSaver.GetEquipment();
        }

        private bool IsUniqueDataEquipped(UniqueEquipmentData _data)
        {
            return usedEquipmentSaver.ContainsEquipment(_data);
        }

        private bool IsUniqueDataInCollection(UniqueEquipmentData _data)
        {
            return collectedEquipmentSaver.ContainsEquipment(_data);
        }

        private void TryRemoveEquipmentFromCollection(UniqueEquipmentData _data)
        {
            if (collectedEquipmentSaver.ContainsEquipment(_data))
            {
                collectedEquipmentSaver.RemoveEquipment(_data);
            }
        }

        private void TryRemoveEquipmentFromSlot(EquipmentSlotType _slotType)
        {
            List<UniqueEquipmentData> _equippedEquipmentUniqueData = GetEquippedEquipmentUniqueData();
            ConstantEquipmentData _temporaryConstantData;
            for (int i = 0; i < _equippedEquipmentUniqueData.Count; i++)
            {
                _temporaryConstantData = equipmentConstantDataHolder.GetData(_equippedEquipmentUniqueData[i].ConstantDataIndentifier);
                if (_temporaryConstantData.SlotType == _slotType)
                {
                    UnequipEquipment(_equippedEquipmentUniqueData[i]);
                    break;
                }
            }
        }

        public enum DataType
        {
            Equipped,
            Collected
        }
    }
}
