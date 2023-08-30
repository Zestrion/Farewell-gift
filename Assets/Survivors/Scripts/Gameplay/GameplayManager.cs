using System;
using UnityEngine;

namespace Survivors.Gameplay
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        public static InputManager InputManager => Instance.inputManager;
        public Action<bool> OnGamePauseStateChanged;
        public Player Player => player;
        public bool IsGamePaused => isGamePaused;

        [SerializeField] private UIManager uiManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private Player player;

        private bool isGamePaused;

        private void Start()
        {
            player.Init();
            uiManager.SetOnPauseButtonClickListener(PauseRequest);
        }

        //private void PlayerDied() {
        //    ShowResultsPopup(GoToMainMenu);
        //    //player.OnPlayerDied -= PlayerDied;
        //}

        //private void GoToMainMenu() {
        //  //  GameManager.Instance.ChangeScene(SceneType.MainMenu);
        //}

        //private void ShowResultsPopup(Action _onComplete)
        //{
        //    ResultsPopup.Settings _popupSettings = new ResultsPopup.Settings();
        //    _popupSettings.StageNumber = GetLevelData().StagesReferences.Length;
        //    _popupSettings.ChapterNumber = GetLevelIndex() + 1;
        //    _popupSettings.CollectedEquipments = equipmentDropper.GetCollectedEquipment();
        //    _popupSettings.CollectedCoins = coinsDropper.CollectedCoinsCount;
        //    _popupSettings.OnComplete = _onComplete;
        //    UIManager.ShowResultsPopup(_popupSettings);
        //}

        public void PauseRequest()
        {
            if(isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        [ContextMenu("Pause Game")]
        private void PauseGame()
        {
            Debug.Log("Pause game");
            isGamePaused = true;
            OnGamePauseStateChanged?.Invoke(true);
        }

        [ContextMenu("Resume Game")]
        private void ResumeGame()
        {
            Debug.Log("Resume game");
            isGamePaused = false;
            OnGamePauseStateChanged?.Invoke(false);
        }
    }
}
