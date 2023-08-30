using System;
using UnityEngine;

namespace Survivors.Gameplay
{
    public class MeleeEnemyStateMachine : GameplayStateMachine
    {
        public class Settings : SettingsBase
        {
            public DamageTaker DamageTaker;
            public CharacterHealth Health;
            public Action OnDead;
            public IAttackable AttackTarget;
            public float AttackDistanceFromTarget;
        }

        [SerializeField] private DoNothingState doNothingState;
        [SerializeField] private ChaseState chaseState;
        [SerializeField] private AttackState attackState;
        [SerializeField] private TakeDamageState takeDamageState;
        [SerializeField] private DeadState deadState;

        internal override void Init(SettingsBase _stateMachineSettings)
        {
            // Set states settings
            Settings _settings = (Settings)_stateMachineSettings;
            chaseState.SetSettings(new ChaseState.Settings()
            {
                TargetTransform = _settings.AttackTarget.Transform
            });
            attackState.SetSettings(new AttackState.Settings()
            {
                TargetToAttack = _settings.AttackTarget
            });
            takeDamageState.SetSettings(new TakeDamageState.Settings()
            {
                DamageTaker = _settings.DamageTaker,
                Health = _settings.Health
            });
            deadState.SetSettings(new DeadState.Settings()
            {
                OnDead = _settings.OnDead
            });

            // Set transitions
            AddTransition(doNothingState, chaseState, () => { return _settings.AttackTarget.IsAlive && !chaseState.IsNearTarget(); });
            AddTransition(chaseState, doNothingState, () => { return !chaseState.IsStateWorking(); });

            AddTransition(doNothingState, attackState, () => { return chaseState.IsNearTarget() && _settings.AttackTarget.IsAlive; });
            AddTransition(attackState, doNothingState, () => { return !attackState.IsStateWorking(); });

            AddTransition(takeDamageState, doNothingState, () => { return !takeDamageState.IsStateWorking(); });
            AddTransition(deadState, doNothingState, () => { return _settings.Health.IsAlive(); });

            AddAnyTransition(takeDamageState, () => { return _settings.Health.IsAlive() && _settings.DamageTaker.damages.Count > 0; });
            AddAnyTransition(deadState, () => { return !_settings.Health.IsAlive(); });

            // Start state machine
            StartStateMachine(chaseState);
        }
    }
}
