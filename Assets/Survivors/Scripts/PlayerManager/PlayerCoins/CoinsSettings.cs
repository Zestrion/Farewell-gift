using UnityEngine;

namespace Survivors
{
    [CreateAssetMenu(fileName = "CoinsSettings", menuName = "Survivor/Coins/CoinsSettings", order = 1)]
    internal class CoinsSettings : ScriptableObject
    {
        internal Sprite CoinIcon { get => coinIcon; }

        [SerializeField] private Sprite coinIcon;
    }
}
