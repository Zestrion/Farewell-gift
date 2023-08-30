using UnityEngine;

namespace Survivors.Equipment
{
    public abstract class GameplayEquipment : MonoBehaviour
    {
        public EquipmentData Data => data;

        protected EquipmentData data;
        protected bool isInitialized;

        public virtual void Init(EquipmentData _data)
        {
            data = _data;
            isInitialized = true;
        }
    }
}