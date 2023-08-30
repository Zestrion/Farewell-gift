using System;
using UnityEngine;

namespace Survivors {

    public class PlayerSpritesheetAnimator : MonoBehaviour {

        private Animator m_playerAnimator;
        private int m_horizontalHash;
        private int m_verticalHash;
        private int m_speedHash;
        private Transform m_transform;

        internal void Init() {
            m_playerAnimator = GetComponent<Animator>();
            m_horizontalHash = Animator.StringToHash("Horizontal");
            m_verticalHash = Animator.StringToHash("Vertical");
            m_speedHash = Animator.StringToHash("Speed");
            m_transform = transform;
        }

        internal void SetMove(Vector3 _translation) {
            _translation = _translation.normalized;
            if (m_playerAnimator.isActiveAndEnabled) {
                m_playerAnimator.SetFloat(m_horizontalHash, _translation.x);
                m_playerAnimator.SetFloat(m_verticalHash, _translation.z);
                m_playerAnimator.SetFloat(m_speedHash, _translation.sqrMagnitude);
            }
        }

    }
}
