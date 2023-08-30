using UnityEngine;

namespace Survivors.AI
{
    public abstract class State : MonoBehaviour
    {
        /// <summary>
        /// This method can be used to set the custom settings to the state
        /// </summary>
        public abstract void SetSettings(StateSettingsBase _stateSettings);
        /// <summary>
        /// Used in state machine
        /// </summary>
        public abstract bool CanEnter();
        public abstract bool IsStateWorking();

        public abstract void Continue();
        public abstract void Pause();

        public abstract void OnEnter();
        public abstract void OnExit();
    }
}
