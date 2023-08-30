using UnityEngine;

namespace Survivors {

    public class AutoAimBullet : MonoBehaviour {

        public LineRenderer lr;
        public ParticleSystem enemyHit;

        float duration = 0.3f;
        float timer;
        private bool shot;

        private Color startColor;
        private Color endColor;
        private Color invisibleStartColor;
        private Color invisibleEndColor;
        private Transform m_origin;

        private void Start() {

            startColor = lr.startColor;
            endColor = lr.endColor;
            invisibleStartColor = startColor;
            invisibleEndColor = endColor;
            invisibleStartColor.a = 0f;
            invisibleEndColor.a = 0f;
        }

        public void Shoot(Transform _origin, Boid _target) {
            lr.SetPosition(0, _origin.transform.position);
            lr.SetPosition(1, _target.transform.position);
            Instantiate(enemyHit, _target.transform.position, enemyHit.transform.rotation);
            _target.enabled = false;
            m_origin = _origin;
            Destroy(_target.gameObject);
            timer = duration;
            shot = true;
        }

        void Update() {
            if (timer > 0) {
                timer -= Time.deltaTime;
                lr.startColor = Color.Lerp(startColor, invisibleStartColor, duration - timer / duration);
                lr.endColor = Color.Lerp(endColor, invisibleEndColor, duration - timer / duration);
                if (m_origin) lr.SetPosition(0, m_origin.position);
            } else {
                if (shot) {
                    Destroy(gameObject);
                }
            }
        }
    }
}