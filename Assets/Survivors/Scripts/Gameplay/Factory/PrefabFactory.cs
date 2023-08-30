using UnityEngine;

namespace Survivors.Gameplay
{
    [System.Serializable]
    public class PrefabFactory
    {
        private MonoBehaviour productPrefab;
        private ObjectsPooler<MonoBehaviour> pooler;

        public PrefabFactory(MonoBehaviour _productPrefab, Transform _parent)
        {
            productPrefab = _productPrefab;
            pooler = new ObjectsPooler<MonoBehaviour>(_productPrefab, _parent);
        }

        public MonoBehaviour GetProduct()
        {
            MonoBehaviour _product = pooler.GetObject();
            TryAddPrefabReferenceToProduct(_product);
            return _product;
        }

        public MonoBehaviour GetPrefab()
        {
            return productPrefab;
        }

        public void MakePrefabFree(MonoBehaviour _prefab)
        {
            pooler.MakeObjectFree(_prefab);
        }

        private void TryAddPrefabReferenceToProduct(MonoBehaviour _product)
        {
            if (_product.gameObject.GetComponent<PrefabReference>() == null)
            {
                _product.gameObject.AddComponent<PrefabReference>().originalPrefab = productPrefab;
            }
        }
    }
}
