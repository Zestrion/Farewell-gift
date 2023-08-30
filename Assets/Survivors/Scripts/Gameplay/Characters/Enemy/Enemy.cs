using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Gameplay
{
    public abstract class Enemy : MonoBehaviour, IAttackable
    {
        internal static Action<Enemy> OnDead;
        public bool IsAlive => health.IsAlive();
        public int Experience => experience;
        public Transform Transform => enemyTransform;
        internal bool IsInitialized => isInitialized;
        //public EquipmentDropSettings EquipmentDropSettings => equipmentDropSettings;
        //public CoinsDropSettings CoinsDropSettings => coinsDropSettings;

        [SerializeField] protected GameplayStateMachine stateMachine;
        [SerializeField] protected CharacterHealth health;
        [SerializeField] protected int maxHealth;
        [SerializeField] protected int experience;
        //[SerializeField] protected StateMachine stateMachine;
        //[SerializeField] protected EnemyStats originalStats;
        //[SerializeField] private EquipmentDropSettings equipmentDropSettings;
        //[SerializeField] private CoinsDropSettings coinsDropSettings;

        protected DamageTaker damageTaker;
        private Transform enemyTransform;
        private bool isInitialized;

        protected virtual void Awake()
        {
            enemyTransform = transform;
            damageTaker = new DamageTaker();
        }

        protected virtual void Start()
        {
            health.Init(maxHealth);
            stateMachine.Init(GetStateMachineSettings());
            isInitialized = true;
        }

        public virtual void OnHit(Damage _damage)
        {
            damageTaker.damages.Enqueue(_damage);
            //GameplayManager.Instance.CameraShaker.Shake(0.05f, 0.05f);
        }

        internal void ReviveEnemy()
        {
            if(!health.IsAlive())
            {
                health.AddToHealth(maxHealth);
            }
            else
            {
                Debug.LogError("Unable to revive the enemy. Enemy is not dead", gameObject);
            }
        }

        protected abstract SettingsBase GetStateMachineSettings();

        protected void EnemyDead()
        {
            OnDead?.Invoke(this);
        }
    }

    [System.Serializable]
    public class DamageTaker
    {
        public Queue<Damage> damages;

        public DamageTaker()
        {
            damages = new Queue<Damage>();
        }
    }

    [System.Serializable]
    public struct Damage
    {
        public int DamageAmount;
        public bool CriticalHit;
        public Vector3 HitPoint;

        public Damage(int _damageAmount, bool _criticalHit, Vector3 _hitPoint)
        {
            DamageAmount = _damageAmount;
            CriticalHit = _criticalHit;
            HitPoint = _hitPoint;
        }
    }

    [Serializable]
    public struct CoinsDropSettings
    {
        public int CoinsAmountToDrop;
        public int OneCoinValue;
    }
}
