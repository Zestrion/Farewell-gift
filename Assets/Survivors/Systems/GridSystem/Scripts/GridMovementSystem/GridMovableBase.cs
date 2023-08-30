using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Grid
{
    public abstract class GridMovableBase : MonoBehaviour, IGridMovable
    {
        public Transform MovableTransform => movableTransform;
        public Transform Target => target;
        
        public virtual MonoBehaviour MonoBehaviour => this;

        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed        = 2f;
        [SerializeField] private float avoidanceRadius  = 2f;
        [SerializeField] private float avoidanceForce   = 2f;

        private float knockbackPower = 10f;
        private bool isKnockback = false;
        private Vector3 knockbackDirection;

        private Vector3 velocity;
        private Transform movableTransform;

        private bool useDebugMode;
        private bool started;

        private const float TARGET_SQUARED_DISTANCE_FROM_TARGET = 1.7f;

        protected virtual void Start()
        {
            movableTransform = transform;
            GridManager.Instance.AddToGridMovement(this);
            started = true;
        }

        protected virtual void OnEnable()
        {
            if (started && GridManager.Instance != null)
            {
                GridManager.Instance.AddToGridMovement(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (started && GridManager.Instance != null)
            {
                GridManager.Instance.RemoveFromGridMovement(this);
            }
        }

        public void ApplyKnockback(Vector3 knockbackSource, float power)
        {
            knockbackPower = power;
            knockbackDirection = (movableTransform.position - knockbackSource).normalized;
            isKnockback = true;
            Invoke(nameof(EndKnockback), 0.5f); // End knockback after 0.5 seconds
        }

        public void SetDebugModeState(bool _enabled)
        {
            useDebugMode = _enabled;
        }

        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        public bool IsNearTarget()
        {
            return (target.position - movableTransform.position).sqrMagnitude <= TARGET_SQUARED_DISTANCE_FROM_TARGET;
        }

        private void EndKnockback()
        {
            isKnockback = false;
        }

        private Vector3 GetDirectionToTarget()
        {
            Vector3 _targetDirection;
            if (IsNearTarget())
            {
                _targetDirection = Vector3.zero;
            }
            else
            {
                _targetDirection = (target.position - movableTransform.position).normalized;
            }
            return _targetDirection;
        }

        private Vector3 GetAvoidanceDirection()
        {
            Vector3 _avoidanceDirection = Vector3.zero;
            List<IGridMovable> _nearbyMovables = GridManager.Instance.GetNearbyMovables(this);

            if (_nearbyMovables != null)
            {
                for (int i = 0; i < _nearbyMovables.Count; i++)
                {
                    if (_nearbyMovables[i] == this)
                    {
                        continue;
                    }

                    Vector3 _directionToEnemy = (movableTransform.position - _nearbyMovables[i].MovableTransform.position);
                    float _distanceToEnemy = _directionToEnemy.sqrMagnitude;
                    _distanceToEnemy = _distanceToEnemy != 0f ? _distanceToEnemy : 0.1f;

                    if (_distanceToEnemy < avoidanceRadius)
                    {
                        _avoidanceDirection += _directionToEnemy.normalized / _distanceToEnemy * avoidanceForce;
                    }
                }
            }
            return _avoidanceDirection;
        }

        private Vector3 GetDirection()
        {
            Vector3 _targetDirection = GetDirectionToTarget();
            Vector3 _avoidanceDirection = GetAvoidanceDirection();
            Vector3 _direction;

            if (isKnockback)
            {
                _direction = knockbackDirection;
            }
            else
            {
                _direction = (_targetDirection + _avoidanceDirection).normalized;
            }

            return _direction;
        }

        private void UpdateMovement()
        {
            velocity = GetDirection() * (isKnockback ? knockbackPower : moveSpeed) * Time.deltaTime;
            movableTransform.position += velocity;
        }

        private void Update()
        {
            UpdateMovement();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (useDebugMode)
            {
                Color _color = Color.red;
                _color.a = 0.3f;
                Gizmos.color = _color;
                Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
            }
        }
#endif
    }
}
