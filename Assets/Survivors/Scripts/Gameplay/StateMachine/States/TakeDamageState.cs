using UnityEngine;
using Survivors.AI;

namespace Survivors.Gameplay
{
    public class TakeDamageState : State
    {
        [SerializeField] private ParticleSystem hitParticlesPrefab;
        private Settings settings;
        private bool isWorking;

        public override void SetSettings(StateSettingsBase _settings)
        {
            settings = (Settings)_settings;
        }

        public override bool CanEnter()
        {
            return settings.DamageTaker.damages.Count > 0;
        }

        public override bool IsStateWorking()
        {
            return settings.DamageTaker.damages.Count > 0;
        }

        public override void OnEnter()
        {
            isWorking = true;
        }

        private void SolveDamages()
        {
            int _damagesCount = settings.DamageTaker.damages.Count;
            Damage _tempDamage;
            for (int i = 0; i < _damagesCount; i++)
            {
                _tempDamage = settings.DamageTaker.damages.Dequeue();
                if (hitParticlesPrefab != null)
                {
                    Instantiate(hitParticlesPrefab, _tempDamage.HitPoint, Quaternion.identity, null);
                }
                settings.Health.TakeDamage(_tempDamage.DamageAmount, _tempDamage.CriticalHit);
            }
        }

        public override void OnExit()
        {
        }

        public override void Continue()
        {
        }

        public override void Pause()
        {
        }

        private void Update()
        {
            if (isWorking)
            {
                SolveDamages();
            }
        }

        public class Settings : StateSettingsBase
        {
            internal CharacterHealth Health;
            public DamageTaker DamageTaker;
        }
    }
}
