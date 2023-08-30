using UnityEngine;

namespace Survivors.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance;

        public float movementSpeed = 5f;
        private Transform playerTransform;

        private void Awake()
        {
            Instance = this;
            playerTransform = transform;
            PlayerStats[] _playerStats = PlayerManager.Instance.GetPlayerStats();
        }

        private void Update()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

            playerTransform.position += movementDirection * movementSpeed * Time.deltaTime;
        }
    }
}
