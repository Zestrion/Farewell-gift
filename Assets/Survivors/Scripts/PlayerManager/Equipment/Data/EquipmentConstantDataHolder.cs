using System.Linq;
using UnityEngine;

namespace Survivors.Equipment
{
    [CreateAssetMenu(fileName = "EquipmentConstantDataHolder", menuName = "Survivor/Equipment/EquipmentConstantDataHolder", order = 1)]
    internal class EquipmentConstantDataHolder : ScriptableObject
    {
        [SerializeField] private ConstantEquipmentData[] constantEquipmentData;

        internal ConstantEquipmentData GetData(string _uniqueIdentifier)
        {
            ConstantEquipmentData _data = constantEquipmentData.FirstOrDefault(data => data.UniqueIdentifier == _uniqueIdentifier);
            if(_data == null)
            {
                Debug.LogError("Equipment database: data not found, identifier " + _uniqueIdentifier);
            }
            return _data;
        }
    }
}
