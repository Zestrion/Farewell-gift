using System.Linq;
using UnityEngine;

namespace Survivors.Equipment
{
    [CreateAssetMenu(fileName = "RaritySettingsData", menuName = "Survivor/Equipment/RaritySettingsData", order = 400)]
    public class RaritySettingsData : ScriptableObject
    {
        [SerializeField] private RarityColor[] rarityColor;
        [SerializeField] private RarityMaxLevel[] rarityMaxLevel;

        public Color GetRarityColor(Rarity _rarity)
        {
            return rarityColor.FirstOrDefault(color => color.Rarity == _rarity).Color;
        }

        public int GetRarityMaxLevel(Rarity _rarity)
        {
            return rarityMaxLevel.FirstOrDefault(maxLevel => maxLevel.Rarity == _rarity).MaxLevel;
        }

        [System.Serializable]
        private struct RarityColor
        {
            public Rarity Rarity;
            public Color Color;
        }

        [System.Serializable]
        private struct RarityMaxLevel
        {
            public Rarity Rarity;
            public int MaxLevel;
        }
    }
}
