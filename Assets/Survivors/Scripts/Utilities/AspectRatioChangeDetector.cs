using System;
using UnityEngine;

namespace Survivors {
    public class AspectRatioChangeDetector : MonoBehaviour {
        public static AspectRatioChangeDetector instance;
        private Action<float> onAspectRatioChanged;
        private float currentAspectRatio;
        private float detectedAspectRatio;

        private void OnEnable() {
            currentAspectRatio = GetCurrentAspectRatio();
        }

        public static void AddListener(Action<float> _onAspectRatioChanged) {
            instance = instance != null ? instance : CreateInstance();
            instance.onAspectRatioChanged += _onAspectRatioChanged;
        }

        public static void RemoveListener(Action<float> _onAspectRatioChanged) {
            if (instance != null) {
                instance.onAspectRatioChanged -= _onAspectRatioChanged;
            }
        }

        private void TryDetectAspectRatioUpdate() {
            detectedAspectRatio = GetCurrentAspectRatio();
            if (currentAspectRatio != detectedAspectRatio) {
                AspectRatioUpdateDetected(detectedAspectRatio);
            }
        }

        private void AspectRatioUpdateDetected(float _newAspectRatio) {
            currentAspectRatio = _newAspectRatio;
            onAspectRatioChanged?.Invoke(currentAspectRatio);
        }

        private float GetCurrentAspectRatio() {
            return (float)Screen.width / (float)Screen.height;
        }

        private static AspectRatioChangeDetector CreateInstance() {
            GameObject _gameObject = new GameObject("AspectRatioChangeDetector");
            DontDestroyOnLoad(_gameObject);
            return _gameObject.AddComponent<AspectRatioChangeDetector>();
        }

        private void Update() {
            TryDetectAspectRatioUpdate();
        }
    }
}
