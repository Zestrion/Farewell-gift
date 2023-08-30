using UnityEngine;

namespace Survivors.Gameplay
{
    public interface IExperienceCollector
    {
        public Transform Transform { get; }
        public float ExperienceSquaredCollectionDistance { get; }
    }
}
