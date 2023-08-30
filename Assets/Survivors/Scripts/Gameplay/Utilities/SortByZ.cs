using UnityEngine;

namespace Survivors.Gameplay
{
    public class SortByZ : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Transform objectTransform;

        private void Start()
        {
            objectTransform = transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            float _zPosition = objectTransform.position.z;
            int _sortingOrder = Mathf.RoundToInt(-_zPosition * 100);
            spriteRenderer.sortingOrder = _sortingOrder;
        }
    }
}
