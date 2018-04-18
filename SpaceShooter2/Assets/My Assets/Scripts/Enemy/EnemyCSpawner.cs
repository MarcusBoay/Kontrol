using System.Collections;
using UnityEngine;

namespace Kontrol
{
    public class EnemyCSpawner : MonoBehaviour
    {
        public float startSpawnWait;
        public float shortSpawnDelay;
        public float longSpawnDelay;

        public int enemySpawnQuantity;

        public GameObject enemyToSpawn;

        private void Start()
        {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(startSpawnWait);
            while (true)
            {
                for (int i = 0; i < enemySpawnQuantity; i++)
                {
                    Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(shortSpawnDelay);
                }
                yield return new WaitForSeconds(longSpawnDelay);
            }
        }
    }
}
