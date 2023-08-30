using UnityEngine;

namespace Survivors
{
    public interface IInitable
    {
        public void Init();
    }

    public abstract class UtilityBase<T> : MonoBehaviour, IInitable
        where T : MonoBehaviour, IInitable
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = UtilityHolder.Instance.gameObject.AddComponent<T>();
                    instance.Init();
                }
                return instance;
            }
        }

        private static T instance;

        public abstract void Init();

        public static bool InstanceExist()
        {
            return instance != null;
        }
    }
}
