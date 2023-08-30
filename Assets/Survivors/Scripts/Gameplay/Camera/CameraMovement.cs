using UnityEngine;

namespace Survivors {
    public class CameraMovement : MonoBehaviour {

        [SerializeField] private Transform m_target;

        private Transform m_transform;
        private Vector3 m_offset;

        private void Start() {
            m_transform = transform;
            m_offset = m_transform.position - m_target.position;
        }

        private void Update() {
            MoveCameraToTarget();
        }

        private void MoveCameraToTarget() {
            m_transform.position = m_target.position + m_offset;
        }

    }
}
