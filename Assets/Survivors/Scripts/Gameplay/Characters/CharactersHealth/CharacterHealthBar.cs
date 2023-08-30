using UnityEngine;
using UnityEngine.UI;

namespace Survivors {
    internal class CharacterHealthBar : MonoBehaviour {
        [Header("Bar position")]
        [SerializeField] private Transform characterTransform;
        [SerializeField] private Vector3 offsetFromCharacter;

        [Header("Fill amount")]
        [SerializeField] private Image healthbarSprite;
        [SerializeField] private float healthReduceSpeed = 2f;

        protected Transform barTransform;
        private float m_fillAmountTarget;
        private Camera m_cam;
        private Vector3 m_healthbarForward;

        private void Start() {
            m_cam = Camera.main;
            m_fillAmountTarget = 1f;
            barTransform = transform;
        }

        internal void SetHealthFillAmount(float _fillAmount) {
            m_fillAmountTarget = _fillAmount;
        }

        private void TryUpdateFillAmount() {
            if (!Mathf.Approximately(m_fillAmountTarget, healthbarSprite.fillAmount)) {
                healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, m_fillAmountTarget, healthReduceSpeed * Time.deltaTime);
            }
        }

        private void UpdateRotation() {
            m_healthbarForward = m_cam.transform.position - barTransform.position;
            m_healthbarForward = Vector3.ProjectOnPlane(m_healthbarForward, m_cam.transform.right);
            barTransform.rotation = Quaternion.LookRotation(m_healthbarForward);
        }

        private void UpdateMovement() {
            Vector3 _newPosition = characterTransform.position + offsetFromCharacter;
            barTransform.position = _newPosition;
        }

        protected virtual void Update() {
            UpdateRotation();
            UpdateMovement();
            TryUpdateFillAmount();
        }
    }
}
