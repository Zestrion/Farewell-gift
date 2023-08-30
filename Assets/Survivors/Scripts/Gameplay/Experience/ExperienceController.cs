using System;
using UnityEngine;

namespace Survivors.Gameplay
{
    public class ExperienceController : MonoBehaviour
    {
        public static Action<int> OnLevelChanged;
        /// <summary>
        /// int 1 - current experience;
        /// int 2 - next level required experience
        /// int 3 - previous level experience
        /// </summary>
        public static Action<int, int, int> OnExperienceChanged;

        [SerializeField] private ExperienceSettings experienceSettings;

        private int currentLevel;
        private int experienceCollected;
        private int previousLevelExperience;
        private int requiredExperienceForLevelUp;

        private void OnEnable()
        {
            Enemy.OnDead += EnemyDead;
            Experience.OnExperienceCollected += ExperienceCollected;
        }

        private void OnDisable()
        {
            Enemy.OnDead -= EnemyDead;
            Experience.OnExperienceCollected -= ExperienceCollected;
        }

        private void Awake()
        {
            currentLevel = 1;
            experienceCollected = 0;
            previousLevelExperience = 0;
            requiredExperienceForLevelUp = experienceSettings.GetRequiredExperienceForLevel(currentLevel + 1);
        }

        private void EnemyDead(Enemy _enemy)
        {
            int _experienceCount = _enemy.Experience;
            Experience _experience = MainFactory.Instance.GetPrefabProduct<Experience>(experienceSettings.ExperiencePrefab);
            _experience.Spawn(new Experience.Settings
            {
                Position = _enemy.Transform.position,
                Color = experienceSettings.GetColorByExperience(_experienceCount),
                ExperienceCount = _experienceCount,
                Collector = GameplayManager.Instance.Player,
                MovementSpeed = experienceSettings.MovementSpeed
            });
        }

        private void ExperienceCollected(Experience _experience)
        {
            MainFactory.Instance.MakeProductFree(_experience);
            experienceCollected += _experience.ExperienceCount;
            CheckForLevelUp();
            OnExperienceChanged?.Invoke(experienceCollected, requiredExperienceForLevelUp, previousLevelExperience);
        }

        private void CheckForLevelUp()
        {
            if(experienceCollected >= requiredExperienceForLevelUp)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentLevel++;
            previousLevelExperience = requiredExperienceForLevelUp;
            requiredExperienceForLevelUp = experienceSettings.GetRequiredExperienceForLevel(currentLevel + 1);
            OnLevelChanged?.Invoke(currentLevel);
        }
    }
}
