using UnityEngine;
using Survivors.AI;

namespace Survivors.Gameplay
{
    public class AttackState : State
    {
        [SerializeField] private int damage;
        [SerializeField] private int criticalHitChance;
        [SerializeField] private float intervalBetweenAttacks;

        private Settings settings;
        private float nextAttackTime;

        public override void SetSettings(StateSettingsBase _stateSettings)
        {
            settings = (Settings)_stateSettings;
        }

        public override bool CanEnter()
        {
            return Time.time > nextAttackTime;
        }

        public override bool IsStateWorking()
        {
            return false;
        }

        public override void OnEnter()
        {
            nextAttackTime = Time.time + intervalBetweenAttacks;
            settings.TargetToAttack.OnHit(GetDamage());
        }

        public override void OnExit()
        {
        }

        public override void Continue()
        {
            throw new System.NotImplementedException();
        }

        public override void Pause()
        {
            throw new System.NotImplementedException();
        }

        private Damage GetDamage()
        {
            bool _criticalHit = Random.Range(0f, 1f) < criticalHitChance;
            int _damage = damage;
            _damage = _criticalHit ? _damage * 2 : _damage;
            return new Damage(_damage, _criticalHit, transform.position);
        }

        public class Settings : StateSettingsBase
        {
            public IAttackable TargetToAttack;
        }
    }
}
