using System.Linq;
using UnityEngine;

namespace Survivors.Gameplay {

    public class Player : MonoBehaviour, IAttackable, IExperienceCollector {

        public Transform Transform => m_transform;
        public bool IsAlive => health.IsAlive();
        public float ExperienceSquaredCollectionDistance => experienceCollectionDistance;
        internal Vector3 MoveDir => m_translation;

        //[SerializeField] private PlayerSpritesheetAnimator playerSpritesAnimator;
        [SerializeField] private CharacterHealth health;
        [SerializeField] private Characters m_characters;
        [Tooltip("Squared distance")]
        [SerializeField] private float experienceCollectionDistance;
        private Transform m_transform;
        private Vector3 m_translation;
        private float m_maxSpeed = 5.0f;
        private bool m_isInitialized;

        private Vector3 m_acc;
        private Vector3 m_vel;
        private float m_speed;

        private int m_isRunning;

        // todo these need to get out of here

        internal void Init() {
            m_transform = transform;
            //m_boidManager = _boidManager;
            m_characters.Init();
            m_isInitialized = true;
            PlayerStats[] _playerStats = PlayerManager.Instance.GetPlayerStats();
            health.Init(_playerStats.FirstOrDefault(_stats => _stats.StatsType == Equipment.StatsType.Health).Stats);
        }

        public void OnHit(Damage _damage)
        {
            health.TakeDamage(_damage.DamageAmount, _damage.CriticalHit);
            if(!health.IsAlive())
            {
                Debug.Log("Player dead");
            }
        }

        private void Update() {
            if (!m_isInitialized) {
                return;
            }

            HandleInput();
            HandleAnimations();
            HandleMovement();
        }

        private void HandleAnimations() {
            m_characters.SetMove(m_translation);
        }

        private void HandleInput() {

            if (!GameplayManager.InputManager.ContainsMovementInput) {
                return;
            }
            Vector2 input = GameplayManager.InputManager.GetInput();
            SetMove(input.x, input.y);
        }

        protected void SetMove(float _h, float _v) {
            m_translation.x = _h;
            m_translation.y = 0;
            m_translation.z = _v;
        }

        private void HandleMovement() {
            float _dt = Time.deltaTime;
            m_translation = m_translation * m_maxSpeed * _dt;
            m_translation = Vector3.ClampMagnitude(m_translation, m_maxSpeed * _dt);
            m_transform.Translate(m_translation);
        }
    }

}