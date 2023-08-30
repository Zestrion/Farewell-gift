using System.Collections.Generic;
using UnityEngine;
using Survivors.AI;

namespace Survivors.Gameplay
{
    public abstract class GameplayStateMachine : StateMachine
    {
        private bool isGamePaused;

        internal abstract void Init(SettingsBase _stateMachineSettings);

        private void OnEnable()
        {
            if(GameplayManager.Instance != null)
            {
                isGamePaused = GameplayManager.Instance.IsGamePaused;
                GameplayManager.Instance.OnGamePauseStateChanged += PauseStateChanged;
            }
        }

        private void OnDisable()
        {
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.OnGamePauseStateChanged -= PauseStateChanged;
            }
        }

        private void PauseStateChanged(bool _isPaused)
        {
            isGamePaused = _isPaused;
            if(CurrentState != null)
            {
                if (_isPaused)
                {
                    CurrentState.Pause();
                }
                else
                {
                    CurrentState.Continue();
                }
            }
        }

        protected override void Update()
        {
            if(!isGamePaused)
            {
                base.Update();
            }
        }
    }
}
