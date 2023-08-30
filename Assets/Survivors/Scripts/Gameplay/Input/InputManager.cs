using UnityEngine;

namespace Survivors {

    public class InputManager : MonoBehaviour {

        [SerializeField] FloatingJoystick m_joystick;

        internal bool ContainsMovementInput => TouchesExist || KeyboardInputExists;

        private bool KeyboardInputExists => m_keyboardInput.sqrMagnitude > float.Epsilon;
        private bool TouchesExist => m_joystick.Exists;

        private Vector2 m_keyboardInput, m_joystickInput, m_input;

        private void Start() {
            m_joystick.Initialize();
        }

        private void Update() {
            m_keyboardInput.x = Input.GetAxis("Horizontal");
            m_keyboardInput.y = Input.GetAxis("Vertical");
            if (TouchesExist) {
                m_joystickInput.x = m_joystick.Horizontal;
                m_joystickInput.y = m_joystick.Vertical;
            }
        }

        internal Vector2 GetInput() {
            if (TouchesExist) {
                m_input = m_joystickInput;
            } else {
                m_input = m_keyboardInput;
            }
            return m_input;
        }
    }


}
