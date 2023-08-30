using UnityEngine;
using Survivors.AI;
using Survivors.Grid;

namespace Survivors.Gameplay
{
    public class ChaseState : State
    {
        public Transform graphic;
        private Vector3 leftScale;
        private Vector3 rightScale;

        internal class Settings : StateSettingsBase
        {
            internal Transform TargetTransform;
        }

        [SerializeField] private GridMovableBase movement;

        public override void SetSettings(StateSettingsBase _stateSettings)
        {
            Settings _settings = (Settings)_stateSettings;
            movement.SetTarget(_settings.TargetTransform);
        }

        private void Start() {
            leftScale = graphic.localScale;
            rightScale = leftScale;
            rightScale.x = -leftScale.x;
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override bool IsStateWorking()
        {
            return !IsNearTarget();
        }

        public override void OnEnter()
        {
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
        private void Update() {
            if (movement.Target.position.x > transform.position.x) {
                graphic.localScale = rightScale;
            } else {
                graphic.localScale = leftScale;
            }
        }
        internal bool IsNearTarget()
        {
            return movement.IsNearTarget();
        }
    }
}
