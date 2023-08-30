using UnityEngine;

namespace Survivors {

    public class MinionCharacter : MonoBehaviour {

        [SerializeField] private SpriteRenderer m_sr;
        [SerializeField] private PlayerSpritesheetAnimator playerSpritesAnimator;
        [SerializeField] private AutoAimBullet autoAimBulletPrefab;

        private AutoAimBullet m_bullet;

        private float m_cooldown = 0.3f;
        private float m_timer;
        private float m_dmgRadius = 0.5f;

        internal void Init() {
            SetMaterialToYellow();
            playerSpritesAnimator.Init();
            m_timer = Random.Range(0.0f, m_cooldown);
        }

        internal void SetMaterialToYellow() {
            m_sr.material.color = new Color(1.0f, 0.92f, 0.016f, 0.5f);
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

        internal bool CanShoot {
            get {
                return m_timer <= 0.0f;
            }
        }

        //if ((closest.transform.position - transform.position).sqrMagnitude<m_dmgRadius* m_dmgRadius + closest.AttackRadius* closest.AttackRadius) {
        //    print("Damage!!!");
        //}


        private void Update() {
            m_timer -= Time.deltaTime;
        }


    }
}





    //if (m_boidManager.isActiveAndEnabled) {
    //if (m_timer < 0.0f) {

    //List<Boid> boids = m_boidManager.Boids;
    //if (boids.Count == 0) {
    //    return;
    //}
    //Boid closest = boids[0];
    //for (int i = 0; i < boids.Count; i++) {
    //if (boids[i] == null) {
    //    continue;
    //}
    //if (closest == null) {
    //    for (int j = 0; j < boids.Count; j++) {
    //        if (boids[j] != null) {
    //            closest = boids[j];
    //        }
    //    }
    //}
    //if ((boids[i].transform.position - transform.position).sqrMagnitude <
    //    (closest.transform.position - transform.position).sqrMagnitude) {
    //    closest = boids[i];
    //}


//if (closest != null) {
//    var bullet = Instantiate(autoAimBulletPrefab);
//    bullet.Shoot(transform, closest);
//    m_boidManager.RemoveBoid(closest);
//}
//m_timer = m_cooldown;
//}
//m_timer -= Time.deltaTime;