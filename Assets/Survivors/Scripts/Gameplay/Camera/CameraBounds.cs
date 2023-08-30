using System;
using UnityEngine;

namespace Survivors {

    public class CameraBounds : Singleton<CameraBounds> {

        [SerializeField] bool drawBounds;

        private Camera m_cam;

        internal float MinX => minX;
        internal float MaxX => maxX;
        internal float MinY => minY;
        internal float MaxY => maxY;

        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        private bool m_initialized;

        protected override void Awake() {
            base.Awake();
            Init();
            CalculateBounds();
        }

        private void Init() {
            m_cam = GetComponent<Camera>();
            if (m_cam == null) {
                m_cam = Camera.main;
            }
            if (m_cam != null) {
                m_initialized = true;
            }
        }

        private void CalculateBounds() {
            float vertExtent = m_cam.orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;
            minX = -horzExtent;
            maxX = horzExtent;
            minY = -vertExtent;
            maxY = vertExtent;
        }

#if UNITY_EDITOR

        Vector3 _bottomLeft = Vector3.zero;
        Vector3 _bottomRight = Vector3.zero;
        Vector3 _topLeft = Vector3.zero;
        Vector3 _topRight = Vector3.zero;

        float size = 1;

        private void OnDrawGizmos() {
            if (!m_initialized || !drawBounds) {
                return;
            }
            DrawBounds();
        }

        private void DrawBounds() {
            Gizmos.color = Color.yellow;
            Vector3 camPos = m_cam.transform.position;

            _bottomLeft.x = minX;
            _bottomLeft.y = 0;
            _bottomLeft.z = minY;

            _bottomRight.x = maxX;
            _bottomRight.y = 0;
            _bottomRight.z = minY;

            _topLeft.x = minX;
            _topLeft.y = 0;
            _topLeft.z = maxY;

            _topRight.x = maxX;
            _topRight.y = 0;
            _topRight.z = maxY;

            _bottomLeft += camPos;
            _bottomRight += camPos;
            _topLeft += camPos;
            _topRight += camPos;

            Gizmos.DrawLine(_bottomLeft, _bottomLeft + Vector3.forward * size);
            Gizmos.DrawLine(_bottomLeft, _bottomLeft + Vector3.right * size);

            Gizmos.DrawLine(_bottomRight, _bottomRight + Vector3.forward * size);
            Gizmos.DrawLine(_bottomRight, _bottomRight + Vector3.left * size);

            Gizmos.DrawLine(_topLeft, _topLeft + Vector3.right * size);
            Gizmos.DrawLine(_topLeft, _topLeft + Vector3.back * size);

            Gizmos.DrawLine(_topRight, _topRight + Vector3.left * size);
            Gizmos.DrawLine(_topRight, _topRight + Vector3.back * size);

        }
#endif
    }
}
