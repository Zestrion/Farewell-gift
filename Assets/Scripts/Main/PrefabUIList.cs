using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class PrefabUIList : MonoBehaviour
    {
        [SerializeField] PrefabUIElement elementPrefab;
        [SerializeField] Transform elementParentTransform;

        public void Open(List<GameObject> prefabList)
        {
            gameObject.SetActive(true);

            foreach (GameObject prefab in prefabList)
            {
                MessageData data = prefab.GetComponent<MessageData>();

                PrefabUIElement newElement = Instantiate(elementPrefab, elementParentTransform);

                newElement.SetAuthorName(data.author);
                newElement.SetPrefabToLoad(prefab);
            }
        }
    }
}