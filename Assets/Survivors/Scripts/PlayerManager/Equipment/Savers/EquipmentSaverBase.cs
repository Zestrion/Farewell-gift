using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Survivors.Equipment
{
    public abstract class EquipmentSaverBase
    {
        protected abstract string saveKey { get; }

        internal void AddEquipment(UniqueEquipmentData _data)
        {
            string _json = JsonUtility.ToJson(_data);
            StringArraySaver.AddString(_json, saveKey);
        }

        internal void RemoveEquipment(UniqueEquipmentData _dataToRemove)
        {
            if (TryFindEquipment(_dataToRemove.EquipmentId, out UniqueEquipmentData _foundData))
            {
                string _json = JsonUtility.ToJson(_foundData);
                StringArraySaver.RemoveString(_json, saveKey);
            }
            else
            {
                Debug.Log("Equipment saver: can't remove equipment");
            }
        }

        internal void UpdateEquipment(UniqueEquipmentData _dataToUpdate)
        {
            if (TryFindEquipment(_dataToUpdate.EquipmentId, out UniqueEquipmentData _foundData))
            {
                // remove previous data
                RemoveEquipment(_foundData);
                // save new data
                AddEquipment(_dataToUpdate);
            }
            else
            {
                Debug.Log("Equipment saver: can't update equipment");
            }
        }

        internal virtual List<UniqueEquipmentData> GetEquipment()
        {
            List<UniqueEquipmentData> _collectedEquipment = new List<UniqueEquipmentData>();
            List<string> _loadedItemsJson = StringArraySaver.GetStringsArray(saveKey);
            for (int i = 0; i < _loadedItemsJson.Count; i++)
            {
                _collectedEquipment.Add(JsonUtility.FromJson<UniqueEquipmentData>(_loadedItemsJson[i]));
            }
            return _collectedEquipment;
        }

        internal bool ContainsEquipment(UniqueEquipmentData _dataToFind)
        {
            UniqueEquipmentData _foundData = GetEquipment().FirstOrDefault(_data => _data.EquipmentId == _dataToFind.EquipmentId);
            return _foundData.EquipmentId == _dataToFind.EquipmentId;
        }

        internal void ClearAllEquipment()
        {
            StringArraySaver.Clear(saveKey);
        }

        private bool TryFindEquipment(int _equipmentId, out UniqueEquipmentData _foundData)
        {
            _foundData = GetEquipment().FirstOrDefault(_data => _data.EquipmentId == _equipmentId);
            bool _found = _foundData.EquipmentId == _equipmentId;
            if(!_found)
            {
                Debug.LogError("Equipment savers data not found: " + _equipmentId.ToString());
            }
            return _found;
        }
    }
}
