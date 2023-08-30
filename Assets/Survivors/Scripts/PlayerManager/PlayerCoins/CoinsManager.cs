using System;
using UnityEngine;

namespace Survivors
{
    public class CoinsManager : MonoBehaviour
    {
        internal static Action<int> OnCoinsCountChanged;
        internal CoinsSettings CoinsSettings => coinsSettings;

        [SerializeField] private CoinsSettings coinsSettings;
        private const string SAVE_KEY = "player_coins_save_key";

        public void AddCoins(int _coinsCount)
        {
            SetCoins(GetCoins() + _coinsCount);
        }

        public void RemoveCoins(int _coinsCount)
        {
            int _coins = GetCoins() - _coinsCount;
            _coins = _coins > 0 ? _coins : 0;
            SetCoins(_coins);
        }

        public int GetCoins()
        {
            return PlayerPrefs.GetInt(SAVE_KEY, 0); 
        }

        public bool IsEnoughCoins(int _price)
        {
            return GetCoins() >= _price;
        }

        private void SetCoins(int _coins)
        {
            PlayerPrefs.SetInt(SAVE_KEY, _coins);
            OnCoinsCountChanged?.Invoke(_coins);
        }
    }
}
