using UnityEngine;

namespace Survivors.Gameplay
{
    public class CameraFix : MonoBehaviour
    {
        [SerializeField] private Camera gameplayCamera;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
            mainCamera.gameObject.SetActive(false);
            gameplayCamera.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            mainCamera.gameObject.SetActive(true);
            gameplayCamera.gameObject.SetActive(false);
        }
    }
}
