using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goodbye
{
    public class ButtonWithText : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Text text;

        private Action OnButtonClick;

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            OnButtonClick?.Invoke();
        }

        public void Show(string _message, Action _callback)
        {
            gameObject.SetActive(true);
            text.text = _message;
            OnButtonClick = _callback;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
