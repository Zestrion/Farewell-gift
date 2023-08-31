using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class MessageController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI messageInput;
        [SerializeField] TextMeshProUGUI senderInput;
        [SerializeField] Button textAdvanceButton;
        [SerializeField] SpriteRenderer backgroundImage;

        [SerializeField] TextMeshProUGUI realMessageInput;
        [SerializeField] Sprite realBackgroundSprite;
        [SerializeField] List<GameObject> fakeSmolsies;
        [SerializeField] List<GameObject> realSmolsies;

        [SerializeField] Image brokenSmolsiesImage;
        [SerializeField] string advancedText1;
        [SerializeField] string advancedText2;

        Button buttonClone;
        Button currentManagerButton;

        int textIndex = 0;

        void Awake()
        {
            currentManagerButton = GameManager.Instance.GetCurrentManagerButton();
            buttonClone = Instantiate(currentManagerButton, transform);
            currentManagerButton.gameObject.SetActive(false);

            buttonClone.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            buttonClone.onClick.AddListener(FakeButtonPress);
        }

        void FakeButtonPress()
        {
            messageInput.text = "Just Kidding ;)";
            buttonClone.gameObject.SetActive(false);

            Invoke(nameof(Transition1), 2.5f);
        }

        void Transition1()
        {
            backgroundImage.sprite = realBackgroundSprite;
            Invoke(nameof(Transition2), 1f);
        }

        void Transition2()
        {
            foreach (GameObject smolsie in fakeSmolsies)
            {
                smolsie.gameObject.SetActive(false);
            }
            Invoke(nameof(Transition3), 1f);
        }

        void Transition3()
        {
            foreach (GameObject smolsie in realSmolsies)
            {
                smolsie.gameObject.SetActive(true);
            }
            Invoke(nameof(ShowActualMessage), 1f);
        }


        void ShowActualMessage()
        {
            //textAdvanceButton.gameObject.SetActive(true);
            messageInput.enabled = false;
            realMessageInput.gameObject.SetActive(true);

            currentManagerButton.gameObject.SetActive(true);
        }

        public void AdvanceText()
        {
            if (textIndex == 0)
            {
                messageInput.enabled = true;
                realMessageInput.enabled = false;
                messageInput.text = advancedText1;
                textIndex++;
            }
            else if (textIndex == 1)
            {
                messageInput.enabled = false;
                brokenSmolsiesImage.gameObject.SetActive(true);
                textIndex++;
            }
            else if (textIndex == 2)
            {
                messageInput.enabled = true;
                messageInput.text = advancedText2;
                brokenSmolsiesImage.gameObject.SetActive(false);
                textAdvanceButton.gameObject.SetActive(false);
                currentManagerButton.gameObject.SetActive(true);
            }
        }
    }
}