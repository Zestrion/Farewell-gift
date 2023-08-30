using System;
using UnityEngine;
using Survivors.AI;

namespace Survivors.Gameplay
{
    public class DeadState : State
    {
        [SerializeField] private GameObject[] objectsToHide;
        [SerializeField] private MonoBehaviour[] scriptsToDisable;
        private Settings settings;

        public override void SetSettings(StateSettingsBase _settings)
        {
            settings = (Settings)_settings;
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override bool IsStateWorking()
        {
            return true;
        }

        public override void OnEnter()
        {
            SetObjectsToHideState(false);
            SetScriptsState(false);
            settings.OnDead?.Invoke();
        }

        public override void OnExit()
        {
            SetObjectsToHideState(true);
            SetScriptsState(true);
        }

        public override void Pause() { }

        public override void Continue() { }

        private void SetObjectsToHideState(bool _enabled)
        {
            for (int i = 0; i < objectsToHide.Length; i++)
            {
                objectsToHide[i].SetActive(_enabled);
            }
        }

        private void SetScriptsState(bool _enabled)
        {
            for (int i = 0; i < scriptsToDisable.Length; i++)
            {
                scriptsToDisable[i].enabled = _enabled;
            }
        }

        public class Settings : StateSettingsBase
        {
            public Action OnDead;
        }
    }
}
