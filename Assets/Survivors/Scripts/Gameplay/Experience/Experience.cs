using System;
using UnityEngine;

namespace Survivors.Gameplay
{
    public class Experience : MonoBehaviour, IPausable
    {
        public static Action<Experience> OnExperienceCollected;
        public int ExperienceCount => experienceCount;

        [SerializeField] private SpriteRenderer spriteRenderer;

        private Transform experienceTransform;
        private bool isActive;
        private bool isPaused;
        private int experienceCount;
        private ToTargetMover toTargetMover;
        private IExperienceCollector experienceCollector;

        private void OnEnable()
        {
            Pauser.Instance.AddClass(this);
        }

        private void OnDisable()
        {
            if (Pauser.InstanceExist())
            {
                Pauser.Instance.RemoveClass(this);
            }
        }

        internal void Spawn(Settings _settings)
        {
            SetVisualsState(true);
            experienceCount = _settings.ExperienceCount;
            experienceCollector = _settings.Collector;
            spriteRenderer.color = _settings.Color;
            experienceTransform ??= transform;
            experienceTransform.position = _settings.Position;
            toTargetMover ??= new ToTargetMover(experienceTransform, _settings.MovementSpeed);
            isActive = true;
        }

        public void SetPauseState(bool _pauseState)
        {
            isPaused = _pauseState;
            toTargetMover.SetPauseState(_pauseState);
        }

        private void TryDetectCollection()
        {
            if (isActive && !isPaused)
            {
                float _squaredDistanceToCollector = (experienceTransform.position - experienceCollector.Transform.position).sqrMagnitude;
                if (_squaredDistanceToCollector <= experienceCollector.ExperienceSquaredCollectionDistance)
                {
                    isActive = false;
                    CollectExperience();
                }
            }
        }

        private void CollectExperience()
        {
            toTargetMover.StartMovement(experienceCollector.Transform, ExperienceCollected);
        }

        private void ExperienceCollected()
        {
            SetVisualsState(false);
            OnExperienceCollected?.Invoke(this);
        }

        private void SetVisualsState(bool _enabled)
        {
            spriteRenderer.enabled = _enabled;
        }

        private void Update()
        {
            TryDetectCollection();
        }

        public struct Settings
        {
            public Vector3 Position;
            public float MovementSpeed;
            public Color Color;
            public int ExperienceCount;
            public IExperienceCollector Collector;
        }
    }
}
