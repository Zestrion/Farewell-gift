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

        public static GameManager Instance;

        [SerializeField] List<GameObject> prefabs;

        [SerializeField] Button buttonInitial1;
        [SerializeField] Button buttonInitial2;
        [SerializeField] Button buttonFinal;
        [SerializeField] Button buttonNextPrefab;
        [SerializeField] Button buttonReturn;
        [SerializeField] TextMeshProUGUI textInput;
        [SerializeField] PrefabUIList prefabUIList;
        [SerializeField] Image backgroundImage;
        [SerializeField] List<string> initialTextList;
        [SerializeField] List<string> finalTextList;

        int progress;
        List<bool> messagesRead;

        GameObject currentMessage;

        int textIndex;
        int finalTextIndex;

        void Awake()
        {
            Instance = this;

            Application.targetFrameRate = 60;

            messagesRead = new List<bool>();

            progress = PlayerPrefs.GetInt(ProgressSavedatakey, 0);

            if (progress == 1)
            {
                //Enable skip start button
            }
            else if (progress == 2)
            {
                buttonInitial1.gameObject.SetActive(false);
                buttonInitial2.gameObject.SetActive(false);
                buttonNextPrefab.gameObject.SetActive(false);
                buttonReturn.gameObject.SetActive(false);
                prefabUIList.Open(prefabs);
            }

            ParseSavedMessagesRead();

            textIndex = 0;
            finalTextIndex = 0;
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
                buttonInitial1.gameObject.SetActive(false);
                buttonInitial2.gameObject.SetActive(true);
            }
        }

        public void AdvanceFinalText()
        {
            if (finalTextIndex < finalTextList.Count)
            {
                textInput.text = finalTextList[finalTextIndex];
                finalTextIndex++;
            }
            if (finalTextIndex == finalTextList.Count)
            {
                buttonFinal.gameObject.SetActive(false);
            }
        }

        public void LoadPrefab(GameObject prefab)
        {
            backgroundImage.enabled = false;
            prefabUIList.gameObject.SetActive(false);
            buttonReturn.gameObject.SetActive(true);

            currentMessage = Instantiate(prefab);
        }

        public void ReturnFromMessage()
        {
            backgroundImage.enabled = true;
            prefabUIList.gameObject.SetActive(true);
            buttonReturn.gameObject.SetActive(false);

            Destroy(currentMessage);
        }

        public void LoadNextRandomPrefab()
        {
            backgroundImage.enabled = false;
            buttonNextPrefab.gameObject.SetActive(true);
            buttonInitial2.gameObject.SetActive(false);

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

        public List<GameObject> GetPrefabList()
        {
            return prefabs;
        }

        public Button GetCurrentManagerButton()
        {
            if (buttonNextPrefab.gameObject.activeSelf) return buttonNextPrefab;
            return buttonReturn;
        }

        void ShowFinalMessage()
        {
            progress = 2;
            PlayerPrefs.SetInt(ProgressSavedatakey, progress);

            backgroundImage.enabled = true;
            buttonNextPrefab.gameObject.SetActive(false);
            buttonFinal.gameObject.SetActive(true);
            textInput.enabled = true;
            textInput.transform.Translate(new Vector3(0f, 50f));
            AdvanceFinalText();
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