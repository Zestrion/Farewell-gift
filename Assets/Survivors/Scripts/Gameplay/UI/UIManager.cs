using System;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.Gameplay
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button pauseButton;

        internal void SetOnPauseButtonClickListener(Action _onPauseClicked)
        {
        }
    }
}
