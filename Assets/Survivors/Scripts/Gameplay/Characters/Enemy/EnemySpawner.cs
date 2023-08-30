using UnityEngine;

namespace Survivors.Gameplay {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private Enemy enemyPrefab;

        private Player player;
        private Vector3 spawnPosition;
        public float radius = 20;

        public float enemyspawnTimer;
        public float enemySpawnInterval = 3f;
        public float enemySpawnMinInterval = 0.2f;

        private int currentSpawnAmount = 1;

        public float enemyWaveSpawnTimer;
        public float enemyWaveSpawnInterval = 10f;
        public int enemyWaveSize = 10;
        public int enemyWaveMaxSize = 40;

        private void Start() {
            player = GameplayManager.Instance.Player;
            Vector3 bottomLeft = new Vector3(CameraBounds.Instance.MinX, 0.0f, CameraBounds.Instance.MinY);
            radius = (bottomLeft - player.transform.position).magnitude;
            spawnPosition = Vector3.left * radius;
            SpawnSomeInitialEnemies(10);
        }

        private void OnEnable()
        {
            Enemy.OnDead += EnemyDead;
        }

        private void OnDisable()
        {
            Enemy.OnDead -= EnemyDead;
        }

        private void Update() {
            //spawnPosition = GetPositionOutsideCameraView();

            enemyspawnTimer += Time.deltaTime;
            enemyWaveSpawnTimer += Time.deltaTime;

            if (enemyWaveSpawnTimer > enemyWaveSpawnInterval) {
                SpawnEnemyWave(enemyWaveSize);
                
                enemyWaveSize+=2;
                enemyWaveSize = Mathf.Clamp(enemyWaveSize, 10, enemyWaveMaxSize);
                enemySpawnInterval -= 0.1f;
                enemySpawnInterval = Mathf.Clamp(enemySpawnInterval, enemySpawnMinInterval, 3);

                enemyWaveSpawnTimer = 0;
                enemyspawnTimer = 0;

            }
            if (enemyspawnTimer > enemySpawnInterval) {
                SpawnSomeEnemies();
                enemyspawnTimer = 0;
            }

        }

        private void SpawnEnemyWave(int _amt = 3)
        {
            spawnPosition = GetPositionOutsideCameraView();
            Vector3 moveDir = player.MoveDir.normalized;
            bool isStill = true;
            if (moveDir.sqrMagnitude > 0.0f)
            {
                isStill = false;
            }
            float dist = isStill ? 1.2f : 0.5f;
            Vector2 insideUnitCircle = Random.insideUnitCircle.normalized * dist * radius;
            for (int i = 0; i < _amt; i++)
            {
                Enemy _enemy = GetEnemy();
                if (isStill)
                {
                    Vector3 newRandomDir = new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y);
                    spawnPosition = player.transform.position + newRandomDir.normalized * radius;
                    spawnPosition.x += insideUnitCircle.x;
                    spawnPosition.z += insideUnitCircle.y;
                }
                else
                {
                    spawnPosition = player.transform.position + moveDir * radius;
                }
                _enemy.Transform.position = spawnPosition;
            }
        }

        private void SpawnSomeInitialEnemies(int _spawnAmount = 1) {
            for (int i = 0; i < _spawnAmount; i++) {
                spawnPosition = GetRandomPositionAroundPlayer();
                Enemy _enemy = GetEnemy();
                _enemy.Transform.position = spawnPosition;
            }
        }

        private void SpawnSomeEnemies(int _spawnAmount = 1) {
            for (int i = 0; i < _spawnAmount; i++) {
                Enemy _enemy = GetEnemy();
                _enemy.Transform.position = GetPositionOutsideCameraView();
            }
        }

        private Vector3 GetPositionOutsideCameraView() {
            Vector3 moveDir = player.MoveDir.normalized;
            if (moveDir.sqrMagnitude > 0.0f) {
                //Vector2 insideUnitCircle = Random.insideUnitCircle * 0.1f * radius;
                spawnPosition = player.transform.position + moveDir * radius;
                //spawnPosition.x += insideUnitCircle.x;
                //spawnPosition.z += insideUnitCircle.y;
            } else {
                spawnPosition = GetRandomPositionAroundPlayer(false);
            }
            return spawnPosition;
        }

        private Vector3 GetRandomPositionAroundPlayer(bool _canBeVisible = true) {
            Vector3 randomPosition = Vector3.zero;
            Vector2 origin = Vector2.zero;
            origin.x = player.transform.position.x;
            origin.y = player.transform.position.z;
            if (_canBeVisible) {
                //print("Random, but can be visible");
                origin = RandomPointInAnnulus(origin, 0.5f * radius, radius);
            } else {
                //print("Random, but can't be visible");
                origin = RandomPointInAnnulus(origin, radius, 1.01f * radius);
            }
            randomPosition.x = origin.x;
            randomPosition.y = 0f;
            randomPosition.z = origin.y;
            return randomPosition;
        }


        public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius) {
            Vector2 randomDirection = Random.insideUnitCircle;
            float randomDistance = Random.Range(minRadius, maxRadius);
            Vector2 point = origin + randomDirection.normalized * randomDistance;
            if ((point - origin).magnitude < minRadius) {
                print(randomDistance + " non wanted " + (point - origin).magnitude);
            }
            return point;
        }

        private Enemy GetEnemy()
        {
            Enemy _enemy = MainFactory.Instance.GetPrefabProduct<Enemy>(enemyPrefab);
            if (_enemy.IsInitialized && !_enemy.IsAlive)
            {
                _enemy.ReviveEnemy();
            }
            return _enemy;
        }

        private void EnemyDead(Enemy _enemy)
        {
            MainFactory.Instance.MakeProductFree(_enemy);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Color _color = Color.cyan;
            _color.a = 1;
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(spawnPosition, 0.1f * radius);
        }
#endif
    }
}
