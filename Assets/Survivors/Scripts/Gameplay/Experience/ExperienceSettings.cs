using System.Linq;
using UnityEngine;

namespace Survivors.Gameplay
{
    [CreateAssetMenu(fileName = "ExperienceSettings", menuName = "Survivor/ExperienceSettings", order = 1)]
    internal class ExperienceSettings : ScriptableObject
    {
        public Experience ExperiencePrefab => experiencePrefab;
        public float MovementSpeed => movementSpeed;

        [SerializeField] private Experience experiencePrefab;
        [Tooltip("Experience movement speed to the player when collected")]
        [SerializeField] private float movementSpeed;
        [SerializeField] private ColorSettings[] colorSettings;
        [SerializeField] private LevelUpSettings[] levelUpSettings;

        internal Color GetColorByExperience(int _experienceCount)
        {
            return colorSettings.FirstOrDefault(
                expSettings =>
                    expSettings.MinExperience <= _experienceCount
                    && expSettings.MaxExperience >= _experienceCount).Color;
        }

        internal int GetRequiredExperienceForLevel(int _level)
        {
            int _requiredExperienceCount = 0;
            bool _searchCompleted = false;
            for (int i = 0; i < levelUpSettings.Length; i++)
            {
                int _minLevel = levelUpSettings[i].MinLevel;
                int _maxLevel = levelUpSettings[i].MaxLevel;

                int _experienceInterval = levelUpSettings[i].ExperienceIncreaseInterval;
               
                for (int y = _minLevel; y <= _maxLevel; y++)
                {
                    if(_level >= y)
                    {
                        _requiredExperienceCount += _experienceInterval;
                    }
                    else
                    {
                        _searchCompleted = true;
                        break;
                    }
                }
                if(_searchCompleted)
                {
                    break;
                }
            }
            return _requiredExperienceCount;
        }
        
        [System.Serializable]
        private struct ColorSettings
        {
            public Color Color;
            public int MinExperience;
            public int MaxExperience;
        }

        [System.Serializable]
        private struct LevelUpSettings
        {
            public int MinLevel;
            public int MaxLevel;
            public int ExperienceIncreaseInterval;
        }
    }
}
