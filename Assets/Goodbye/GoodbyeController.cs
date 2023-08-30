using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goodbye
{
    [Serializable]
    public class Response
    {
        public string response;
    }

    [Serializable]
    public class ComplexResponse : Response
    {
        public int nextTextIndex = -1;
    }

    [Serializable]
    public class GoodbyeText<T> where T : Response
    {
        [TextArea(1, 10)] public string text;
        public bool hasResponse;
        public T firstResponse;
        public bool hasAnotherResponse;
        public T secondResponse;
        public float delayBeforeNext;
        public bool isLastGoodbye;
    }

    [Serializable]
    public class GoodbyeTextAdvanced : GoodbyeText<ComplexResponse>
    {
        public int nextIndexIfNoResponse = -1;
    }
    
    public class GoodbyeController : MonoBehaviour
    {
        [SerializeField] private Text mainText;
        [SerializeField] private ButtonWithText firstButton, secondButton;

        [Space]
        [SerializeField] private GoodbyeText<Response> startQuestion;
        [SerializeField] private List<GoodbyeTextAdvanced> firstSequence, secondSequence;

        private WaitForSeconds charPrintInterval = new WaitForSeconds(CHAR_PRINT_INTERVAL);

        private const float DELAY_BEFORE_RESTART = 3f;
        private const float CHAR_PRINT_INTERVAL = 0.04f;

        private void Start()
        {
            StartQuestion();
        }

        private void StartQuestion()
        {
            GoodbyeTextSequence(startQuestion, () => StartSequence(firstSequence), () => StartSequence(secondSequence));
        }

        private void GoodbyeTextSequence<T>(GoodbyeText<T> _goodbye, Action _firstCallback, Action _secondCallback, Action _noResponseCallback = null) where T : Response
        {
            HideButtons();
            StartCoroutine(MainTextRoutine(_goodbye.text, _goodbye.delayBeforeNext, TextCallback));

            void TextCallback()
            {
                if (_goodbye.isLastGoodbye)
                {
                    Invoke(nameof(ShowRestartButton), DELAY_BEFORE_RESTART);
                }
                else
                {
                    if (_goodbye.hasResponse)
                    {
                        firstButton.Show(_goodbye.firstResponse.response, _firstCallback);
                    }

                    if (_goodbye.hasAnotherResponse)
                    {
                        secondButton.Show(_goodbye.secondResponse.response, _secondCallback);
                    }

                    if(!_goodbye.hasResponse && !_goodbye.hasAnotherResponse)
                    {
                        _noResponseCallback?.Invoke();
                    }
                }
            }
        }

        private void StartSequence(List<GoodbyeTextAdvanced> _sequence)
        {
            int _index = -1;
            NextGoodbye();

            void NextGoodbye()
            {
                _index++;
                if(_index < _sequence.Count)
                {
                    Goodbye(_index);
                }
                else
                {
                    HideButtons();
                    ClearMainText();
                    Invoke(nameof(ShowRestartButton), DELAY_BEFORE_RESTART);
                }
            }

            void Goodbye(int _ind)
            {
                _index = _ind;
                GoodbyeTextAdvanced _goodbye = _sequence[_ind];
                GoodbyeTextSequence(_goodbye, () =>
                {
                    if(_goodbye.firstResponse.nextTextIndex > 0)
                    {
                        Goodbye(_goodbye.firstResponse.nextTextIndex);
                    }
                    else if(!_goodbye.isLastGoodbye)
                    {
                        NextGoodbye();
                    }
                }, () =>
                {
                    if (_goodbye.secondResponse.nextTextIndex > 0)
                    {
                        Goodbye(_goodbye.secondResponse.nextTextIndex);
                    }
                    else if (!_goodbye.isLastGoodbye)
                    {
                        NextGoodbye();
                    }
                }, () => 
                {
                    if (_goodbye.nextIndexIfNoResponse > 0)
                    {
                        Goodbye(_goodbye.nextIndexIfNoResponse);
                    }
                    else
                    {
                        NextGoodbye();
                    }
                });
            }
        }

        private void ClearMainText()
        {
            mainText.text = "";
        }

        private IEnumerator MainTextRoutine(string _message, float _delayBeforeCallback, Action _callback)
        {
            ClearMainText();

            string _text = "";
            foreach(char _c in _message)
            {
                _text += _c;
                mainText.text = _text;
                yield return charPrintInterval;
            }

            yield return new WaitForSeconds(_delayBeforeCallback);

            _callback?.Invoke();
        }

        private void HideButtons()
        {
            firstButton.Hide();
            secondButton.Hide();
        }

        private void ShowRestartButton()
        {
            firstButton.Show("Restart?", StartQuestion);
        }
    }
}
