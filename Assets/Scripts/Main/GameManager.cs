using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        const string ProgressSavedatakey = "Progress";
        const string MessagesReadSavedataKey = "MessagesRead";

        [SerializeField] List<GameObject> prefabs;

        [SerializeField] Button button1;
        [SerializeField] Button button2;
        [SerializeField] Button buttonNextPrefab;
        [SerializeField] TextMeshProUGUI textInput;
        [SerializeField] List<string> initialTextList;

        int progress;
        List<bool> messagesRead;

        GameObject currentMessage;

        int textIndex;

        void Awake()
        {
            Application.targetFrameRate = 60;

            messagesRead = new List<bool>();

            progress = PlayerPrefs.GetInt(ProgressSavedatakey, 0);

            if (progress == 1)
            {
                //Enable skip start button
            }
            else if (progress == 2)
            {
                //Show List, disable buttons
            }

            ParseSavedMessagesRead();

            textIndex = 0;
        }

        public void AdvanceText()
        {
            if (textIndex < initialTextList.Count)
            {
                textInput.text = initialTextList[textIndex];
                textIndex++;
            }
            else if (textIndex == initialTextList.Count)
            {
                textInput.enabled = false;
                button1.gameObject.SetActive(false);
                button2.gameObject.SetActive(true);
            }
        }

        public void LoadNextRandomPrefab()
        {
            buttonNextPrefab.gameObject.SetActive(true);
            button2.gameObject.SetActive(false);

            Destroy(currentMessage);

            List<GameObject> unreadPrefabs = new List<GameObject>();
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (!messagesRead[i])
                {
                    unreadPrefabs.Add(prefabs[i]);
                }
            }

            if (unreadPrefabs.Count > 0)
            {
                GameObject selectedPrefab = unreadPrefabs[Random.Range(0, unreadPrefabs.Count)];
                currentMessage = Instantiate(selectedPrefab);

                for (int i = 0; i < prefabs.Count; i++)
                {
                    if (prefabs[i] == selectedPrefab)
                    {
                        messagesRead[i] = true;
                        break;
                    }
                }
                SaveMessagesRead();
            }
            else
            {
                ShowFinalMessage();
            }
        }

        void ShowFinalMessage()
        {

        }

        void SaveMessagesRead()
        {
            StringBuilder builder = new StringBuilder();
            foreach (bool messageRead in messagesRead)
            {
                builder.Append(messageRead ? "1" : "0" + "|");
            }
            builder.Length--;

            PlayerPrefs.SetString(MessagesReadSavedataKey, builder.ToString());
        }

        void ParseSavedMessagesRead()
        {
            string savedData = PlayerPrefs.GetString(MessagesReadSavedataKey, "");

            if (savedData.Length != 0)
            {
                string[] dataSplit = savedData.Split('|');
                foreach (string data in dataSplit)
                {
                    messagesRead.Add(int.Parse(data) == 1);
                }
            }
            while (messagesRead.Count < prefabs.Count)
            {
                messagesRead.Add(false);
            }
        }
    }
}