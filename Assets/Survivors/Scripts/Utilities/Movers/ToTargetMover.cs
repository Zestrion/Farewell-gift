using System;
using UnityEngine;

namespace Survivors.Gameplay
{
    internal class ToTargetMover : IUpdatable
    {
        private Transform targetTransform;
        private Action onMovementCompleted;
        private bool isMoving;
        private bool isPaused;

        private readonly Transform transformToMove;
        private readonly float movementSpeed;

        internal ToTargetMover(Transform _transformToMove, float _movementSpeed)
        {
            movementSpeed = _movementSpeed;
            transformToMove = _transformToMove;
            if (_movementSpeed == 0f)
            {
                Debug.LogError("ToTargetMover: movement speed 0!");
            }
        }

        internal bool IsMoving()
        {
            return isMoving;
        }

        internal void SetPauseState(bool _paused)
        {
            isPaused = _paused;
        }

        internal void StartMovement(Transform _targetTransform, Action _onMovementCompleted)
        {
            if (!isMoving)
            {
                isMoving = true;
                onMovementCompleted = _onMovementCompleted;
                ClassUpdater.Instance.AddClass(this);
                targetTransform = _targetTransform;
            }
        }

        private void MovementCompleted()
        {
            isMoving = false;
            ClassUpdater.Instance.RemoveClass(this);
            onMovementCompleted?.Invoke();
        }

        private void TryMoveToTarget()
        {
            // movement
            transformToMove.position = Vector3.MoveTowards(
                transformToMove.position, targetTransform.position, movementSpeed * Time.deltaTime);

            // check if movement completed
            if (Vector3.Distance(transformToMove.position,
                targetTransform.position) < 0.5f)
            {
                MovementCompleted();
            }
        }

        public void Update()
        {
            if (isMoving && !isPaused)
            {
                TryMoveToTarget();
            }
        }
    }
}
