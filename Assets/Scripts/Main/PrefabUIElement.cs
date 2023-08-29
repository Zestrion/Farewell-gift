using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public class PrefabUIElement : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI authorNameInput;
        [SerializeField] Button loadButton;

        GameObject prefabToLoad;

        public void SetAuthorName(string name)
        {
            authorNameInput.text = name;
        }

        public void SetPrefabToLoad(GameObject prefab)
        {
            prefabToLoad = prefab;
        }

        public void LoadPrefab()
        {
            GameManager.Instance.LoadPrefab(prefabToLoad);
        }
    }
}