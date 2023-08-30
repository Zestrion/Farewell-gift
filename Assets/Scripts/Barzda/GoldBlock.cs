using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barzda {
    public class GoldBlock : MonoBehaviour
    {
        public GameObject activeObject;
        public GameObject emptyObject;
        public GameObject prefab;

        private bool isHit;

        // Start is called before the first frame update
        void Start()
        {

        }
        private void OnCollisionEnter(Collision collision)
        {
            if (isHit)
            {
                return;
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                activeObject.SetActive(false);
                emptyObject.SetActive(true);
                isHit = true;

                if (prefab != null)
                {
                    Instantiate(prefab, transform.position + Vector3.up * 5.0f, prefab.transform.rotation);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
