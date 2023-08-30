using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barzda
{
    public class SmoothCamera : MonoBehaviour
    {
        public Transform target;
        public float smoothSpeed = 0.125f;

        private Vector3 offset;

        private void Start()
        {
            offset = transform.position - target.position;
        }

        public void CameraUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, transform.position.y, smoothedPosition.z);
        }
    }
}
