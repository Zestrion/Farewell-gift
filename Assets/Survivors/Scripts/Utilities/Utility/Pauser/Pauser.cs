using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Gameplay
{
    public class Pauser : UtilityBase<Pauser>
    {
        private List<IPausable> pausables;
        private bool isGamePaused;

        public override void Init()
        {
            pausables = new List<IPausable>();
        }

        public bool IsGamePaused()
        {
            return isGamePaused;
        }

        public void AddClass(IPausable _class)
        {
            if (pausables.Contains(_class))
            {
                Debug.Log("Already updating this class " + _class.ToString());
                return;
            }
            pausables.Add(_class);
        }

        public void RemoveClass(IPausable _class)
        {
            if (!pausables.Contains(_class))
            {
                Debug.Log("Class not updating to be removed");
                return;
            }
            pausables.Remove(_class);
        }

        private void OnEnable()
        {
            if (GameplayManager.Instance != null)
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
            for (int i = 0; i < pausables.Count; i++)
            {
                pausables[i].SetPauseState(_isPaused);
            }
        }
    }
}
