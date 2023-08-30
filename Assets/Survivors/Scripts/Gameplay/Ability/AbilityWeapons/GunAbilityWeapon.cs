using UnityEngine;
using Survivors.Grid;

namespace Survivors.Gameplay
{
    public class GunAbilityWeapon : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletSpawnPosition;

        private Vector3 previousPlayerMovementDirection;
        private Transform weaponTransform;
        private float nextAttackTime;
        private const float ATTACK_INTERVAL = 1f;

        private void Start()
        {
            nextAttackTime = 1f;
            weaponTransform = transform;
            player = GameplayManager.Instance.Player;
            previousPlayerMovementDirection = new Vector3(0f, 0.1f, 0f);
        }

        [ContextMenu("Kill nearest enemy")]
        private void KillNearestEnemy()
        {
            Enemy _closestEnemy = GridManager.Instance.GetClosestMovable<Enemy>(transform.position);
            if (_closestEnemy != null)
            {
                transform.LookAt(_closestEnemy.Transform.position);
                Bullet _bullet = MainFactory.Instance.GetPrefabProduct<Bullet>(bulletPrefab);
                _bullet.transform.position = bulletSpawnPosition.position;
                Vector3 _movementDirection = (_closestEnemy.Transform.position - bulletSpawnPosition.position).normalized;
                _movementDirection.y = 0f;
                _bullet.Shoot(new Bullet.Settings()
                {
                    Damage = 100,
                    Speed = 10,
                    MovementDirection = _movementDirection,
                    EnemyDetectionSqrDistance = 0.5f,
                    OnHit = () => { MainFactory.Instance.MakeProductFree(_bullet); }
                });
                //_closestEnemy.OnHit(new Damage()
                //{
                //    DamageAmount = 100,
                //    CriticalHit = false,
                //});
            }
        }

        private void RotateTowardsPlayerRotation()
        {
            Quaternion targetRotation;
            if (player.MoveDir != Vector3.zero)
            {
                previousPlayerMovementDirection = player.MoveDir;
                targetRotation = Quaternion.LookRotation(player.MoveDir);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(previousPlayerMovementDirection);
            }
            weaponTransform.rotation = Quaternion.Lerp(weaponTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void Update()
        {
            if(Time.time >= nextAttackTime)
            {
                KillNearestEnemy();
                nextAttackTime = Time.time + ATTACK_INTERVAL;
            }
            else
            {
                RotateTowardsPlayerRotation();
            }
        }
    }
}
