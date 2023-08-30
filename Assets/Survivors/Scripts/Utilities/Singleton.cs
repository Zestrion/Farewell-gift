using UnityEngine;

namespace Survivors
{
    public class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        public static T Instance { get { return instance; } }
        private static T instance;

        [SerializeField] bool dontDestroyOnLoad;

        protected virtual void Awake()
        {
            SetInstance();
        }

        protected virtual void OnDestroy()
        {
        }

        private void SetInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
