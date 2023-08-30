using UnityEngine;
using TMPro;

namespace Survivors
{
    public class CoinsBar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

        private void Start()
        {
            SetCoins(PlayerManager.Coins.GetCoins());    
        }

        private void OnEnable()
        {
            CoinsManager.OnCoinsCountChanged += OnCoinsCountChanged;
        }

        private void OnDisable()
        {
            CoinsManager.OnCoinsCountChanged -= OnCoinsCountChanged;
        }

        private void OnCoinsCountChanged(int _coinsCount)
        {
            SetCoins(_coinsCount);
        }

        private void SetCoins(int _coinsCount)
        {
            coinsText.text = _coinsCount.ToString();
        }
    }
}
