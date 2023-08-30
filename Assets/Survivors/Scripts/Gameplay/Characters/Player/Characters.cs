using UnityEngine;
using System.Collections.Generic;
using System;

namespace Survivors {

    public class Characters : MonoBehaviour {

        [SerializeField] public MainCharacter mainCharacter;
        [SerializeField] public float largerCircleRadius;
        [SerializeField] public int numberOfCharacters;
        [SerializeField] public MinionCharacter smallerCirclePrefab;
        [SerializeField] public GameObject largerCirclePrefab;

        private List<MinionCharacter> m_minions;
        private Transform m_transform;
        private GameObject m_largerCircle;

        private float m_currentAngle;
        private float m_rotationSpeed = 60;

        internal void Init() {
            m_transform = transform;
            RearrangeCircles();
        }

        private void RearrangeCircles() {

            if (m_minions != null && m_minions.Count > 0) {
                foreach (MinionCharacter character in m_minions) {
                    Destroy(character.gameObject);
                }
            }
            m_minions = new List<MinionCharacter>();

            if (m_largerCircle) {
                Destroy(m_largerCircle.gameObject);
                m_largerCircle = null;
            }

            m_largerCircle = Instantiate(largerCirclePrefab, m_transform.position, Quaternion.identity);
            m_largerCircle.transform.localScale = Vector3.one * largerCircleRadius * 2;
            m_largerCircle.name = "LargerCircle";
            m_largerCircle.transform.SetParent(m_transform);
            Material mat = m_largerCircle.GetComponentInChildren<SpriteRenderer>().material;
            if (mat) mat.color = Color.red;

            mainCharacter.Init();

            float angleIncrement = 360f / numberOfCharacters;
            
            for (int i = 0; i < numberOfCharacters; i++) {
                float angle = i * angleIncrement;
                Vector3 circlePosition = CalculateCirclePosition(angle);
                float smallerCircleRadius = CalculateSmallerCircleRadius();

                MinionCharacter newMinion = Instantiate(smallerCirclePrefab, circlePosition, Quaternion.identity);
                newMinion.transform.localScale = Vector3.one * smallerCircleRadius * 2;
                newMinion.transform.SetParent(m_transform);
                newMinion.Init();
                newMinion.name = "Minion" + i;
                m_minions.Add(newMinion);
            }
        }

        private Vector3 CalculateCirclePosition(float angle) {
            float radians = angle * Mathf.Deg2Rad;
            float x = m_transform.position.x + largerCircleRadius * 1.5f * Mathf.Cos(radians);
            float z = m_transform.position.z + largerCircleRadius * 1.5f * Mathf.Sin(radians);
            return new Vector3(x, 0.01f, z);
        }

        private float CalculateSmallerCircleRadius() {
            // Calculate the maximum possible radius for the smaller circles to fit within the larger circle
            float maxAllowedRadius = (largerCircleRadius * 2) / Mathf.Sqrt(numberOfCharacters);

            // You might want to add some padding or adjust the radius further to ensure proper spacing
            float padding = 0.1f; // Adjust this value as needed
            float smallerCircleRadius = maxAllowedRadius - padding;

            return smallerCircleRadius;
        }

        internal void SetMove(Vector3 _dir) {
            mainCharacter.SetMove(_dir);
            foreach (MinionCharacter character in m_minions) {
                character.SetMove(_dir);
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.E)) {
                numberOfCharacters++;
                RearrangeCircles();
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                numberOfCharacters--;
                RearrangeCircles();
            }
            RotateMinions();
            //SearchAndOrderToShoot();
        }


        private void RotateMinions() {
            float angleIncrement = 360f / numberOfCharacters;

            for (int i = 0; i < numberOfCharacters; i++) {

                float angle = i * angleIncrement + m_currentAngle;
                //m_currentAngle = angle + Time.deltaTime * m_rotationSpeed;

                Vector3 circlePosition = CalculateCirclePosition(angle);
                //float smallerCircleRadius = CalculateSmallerCircleRadius();

                MinionCharacter minion = m_minions[i];
                minion.transform.position = circlePosition;
            }
            m_currentAngle += Time.deltaTime * m_rotationSpeed;
        }

        
    }
}

//if ((closest.transform.position - transform.position).sqrMagnitude < m_dmgRadius * m_dmgRadius + closest.AttackRadius * closest.AttackRadius) {
//    print("Damage!!!");
//}
//if (closest != null) {
//    var bullet = Instantiate(autoAimBulletPrefab);
//    bullet.Shoot(transform, closest);
//    m_boidManager.RemoveBoid(closest);
//}


//private void SearchAndOrderToShoot() {

//    if (!m_boidManager.isActiveAndEnabled) {
//        return;
//    }
//    List<Boid> boids = m_boidManager.Boids;
//    if (boids.Count == 0) {
//        return;
//    }
//    Boid closest = boids[0];
//    for (int i = 0; i < boids.Count; i++) {
//        if (boids[i] == null) {
//            continue;
//        }
//        if (closest == null) {
//            for (int j = 0; j < boids.Count; j++) {
//                if (boids[j] != null) {
//                    closest = boids[j];
//                }
//            }
//        }
//        if ((boids[i].transform.position - m_transform.position).sqrMagnitude <
//            (closest.transform.position - m_transform.position).sqrMagnitude) {
//            closest = boids[i];
//        }
//    }

//    if (closest != null) {
//        if (mainCharacter.CanShoot) {
//            mainCharacter.Shoot(closest);
//            return;
//        }
//        for (int j = 0; j < m_minions.Count; j++) {
//            if (m_minions[j].CanShoot){
//                m_minions[j].Shoot(closest);
//                return;
//            }
//        }
//    }
//}