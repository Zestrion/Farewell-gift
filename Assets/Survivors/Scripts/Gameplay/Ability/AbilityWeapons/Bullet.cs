using System;
using UnityEngine;
using Survivors.Grid;

namespace Survivors.Gameplay
{
    public class Bullet : MonoBehaviour
    {
        public struct Settings
        {
            public int Damage;
            public float Speed;
            public Vector3 MovementDirection;
            public float EnemyDetectionSqrDistance;
            public Action OnHit;
        }

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ParticleSystem onHitParticles;

        private Vector3 movementDirection;
        private float speed;
        private int damage;
        public Action onHit;

        private float enemyDetectionSqrDistance;
        private Transform bulletTransform;

        private bool isActive;

        public void Shoot(Settings _settings)
        {
            enemyDetectionSqrDistance = _settings.EnemyDetectionSqrDistance;
            onHit = _settings.OnHit;
            movementDirection = _settings.MovementDirection;
            speed = _settings.Speed;
            damage = _settings.Damage;
            bulletTransform ??= transform;
            spriteRenderer.enabled = true;
            bulletTransform.rotation = Quaternion.LookRotation(movementDirection);
            isActive = true;
        }

        private void MoveBullet()
        {
            bulletTransform.position += movementDirection.normalized * speed * Time.deltaTime;
        }

        private void TryDetectEnemy()
        {
            Enemy _enemy = GridManager.Instance.GetClosestMovable<Enemy>(bulletTransform.position);
            if(_enemy != null
                && (_enemy.Transform.position - bulletTransform.position).sqrMagnitude <= enemyDetectionSqrDistance)
            {
                HitEnemy(_enemy);
            }
        }

        private void HitEnemy(Enemy _enemy)
        {
            _enemy.OnHit(new Damage()
            {
                DamageAmount = damage,
                CriticalHit = false
            });
            PlayParticles();
            spriteRenderer.enabled = false;
            isActive = false;
            onHit?.Invoke();
        }

        private void PlayParticles()
        {
            onHitParticles.Play();
        }

        private void Update()
        {
            if(isActive)
            {
                MoveBullet();
                TryDetectEnemy();
            }
        }
    }
}
