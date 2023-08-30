using UnityEngine;

namespace Survivors
{
    internal class UtilityHolder : MonoBehaviour
    {
        internal static UtilityHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("Utility").AddComponent<UtilityHolder>();
                }
                return instance;
            }
        }
        private static UtilityHolder instance;
    }
}
