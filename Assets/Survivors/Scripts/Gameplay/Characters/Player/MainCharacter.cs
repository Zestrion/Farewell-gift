using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Survivors {
    public class MainCharacter : MonoBehaviour {

        [SerializeField] private PlayerSpritesheetAnimator playerSpritesAnimator;
        [SerializeField] private AutoAimBullet autoAimBulletPrefab;

        private AutoAimBullet m_bullet;

        private float m_cooldown = 0.3f;
        private float m_timer;
        private float m_dmgRadius = 0.5f;

        internal bool CanShoot => m_timer <= 0.0f;

        internal void Init() {
            playerSpritesAnimator.Init();
            m_timer = Random.Range(0.0f, m_cooldown);
        }

        private void Update() {
            m_timer -= Time.deltaTime;
        }

        internal void SetMove(Vector3 _dir) {
            if (playerSpritesAnimator == null) {
                return;
            }
            playerSpritesAnimator.SetMove(_dir);
        }

        internal void Shoot(Boid _target) {
            if (m_bullet != null) {
                Destroy(m_bullet.gameObject);
                m_bullet = null;
            }
            m_bullet = Instantiate(autoAimBulletPrefab);
            m_bullet.Shoot(transform, _target);
            //m_boidManager.RemoveBoid(_target);
            m_timer = m_cooldown;
        }
    }
}
