using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Gameplay
{
    public class MainFactory : Singleton<MainFactory>
    {
        [Tooltip("Parent to spawn UI products")]
        [SerializeField] private Transform factoryParentUI;
        private List<PrefabFactory> prefabFactories;

        protected override void Awake()
        {
            base.Awake();
            prefabFactories = new List<PrefabFactory>();
        }

        /// <summary>
        /// Important: when a product is no longer in use, make it free using the MakePrefabFree() method!
        /// </summary>
        public T GetPrefabProduct<T>(MonoBehaviour _prefab)
            where T : MonoBehaviour
        {
            return GetProduct<T>(_prefab, false);
        }

        /// <summary>
        /// Use this method when the prefab is a UI object.
        /// <para>Important: when a product is no longer in use, make it free using the MakePrefabFree() method!</para>
        /// </summary>
        public T GetPrefabProductUI<T>(MonoBehaviour _prefab)
           where T : MonoBehaviour
        {
            if(factoryParentUI == null)
            {
                Debug.LogError("Main factory: UI parent not found!");
            }
            return GetProduct<T>(_prefab, true);
        }

        private T GetProduct<T>(MonoBehaviour _prefab, bool _uiProduct)
            where T : MonoBehaviour
        {
            for (int i = 0; i < prefabFactories.Count; i++)
            {
                if (prefabFactories[i].GetPrefab() == _prefab)
                {
                    return prefabFactories[i].GetProduct() as T;
                }
            }
            return CreateFactoryForPrefab(_prefab, _uiProduct).GetProduct() as T;
        }

        public void MakeProductFree(MonoBehaviour _product)
        {
            MonoBehaviour _originalPrefab = GetOriginalPrefabFromProduct(_product);
            if(_originalPrefab == null)
            {
                Debug.LogError("Main factory: prefab reference not found " + _product.gameObject.name);
                return;
            }
            for (int i = 0; i < prefabFactories.Count; i++)
            {
                if (prefabFactories[i].GetPrefab() == _originalPrefab)
                {
                    prefabFactories[i].MakePrefabFree(_product);
                    break;
                }
            }
        }

        private PrefabFactory CreateFactoryForPrefab(MonoBehaviour _prefab, bool _uiProducts)
        {
            GameObject _parentForNewFactory = new GameObject(_prefab.gameObject.name);
            _parentForNewFactory.transform.SetParent(_uiProducts ? factoryParentUI : transform);

            // Make the parent a UI game object
            if (_uiProducts)
            {
                _parentForNewFactory.layer = LayerMask.NameToLayer("UI");
                RectTransform _rect = _parentForNewFactory.AddComponent<RectTransform>();
                _rect.anchorMin = Vector2.zero;
                _rect.anchorMax = Vector2.one;
                _rect.offsetMin = Vector2.zero;
                _rect.offsetMax = Vector2.zero;
                _rect.localScale = Vector3.one;
            }

            PrefabFactory _factoryForPrefab = new PrefabFactory(_prefab, _parentForNewFactory.transform);
            prefabFactories.Add(_factoryForPrefab);
            return _factoryForPrefab;
        }

        private MonoBehaviour GetOriginalPrefabFromProduct(MonoBehaviour _spawnedPrefab)
        {
            return _spawnedPrefab.gameObject.GetComponent<PrefabReference>().originalPrefab;
        }
    }
}
