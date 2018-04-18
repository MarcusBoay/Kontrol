using UnityEngine;

namespace Kontrol
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyToSpawn;
        public GameObject parentGameObject;
        public float spawnRotation;

        public float distanceFromCameraToSpawn;
        public bool checkX;
        public bool checkY;

        private bool spawned;

        private void FixedUpdate()
        {
            if (checkX && !checkY && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToSpawn && !spawned && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToSpawn)
            {
                spawned = true;
                GameObject spawnedEnemy = Instantiate(enemyToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, spawnRotation + transform.eulerAngles.z));
                if (parentGameObject != null)
                {
                    spawnedEnemy.transform.parent = parentGameObject.transform;
                }
                else
                {
                    spawnedEnemy.transform.parent = gameObject.transform.parent;
                }
                //destroy this spawner once it has spawned the enemy
                Destroy(gameObject);
            }
            else if (checkY && !checkX && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToSpawn && !spawned && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToSpawn)
            {
                spawned = true;
                GameObject spawnedEnemy = Instantiate(enemyToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, spawnRotation + transform.eulerAngles.z));
                if (parentGameObject != null)
                {
                    spawnedEnemy.transform.parent = parentGameObject.transform;
                }
                else
                {
                    spawnedEnemy.transform.parent = gameObject.transform.parent;
                }
                //destroy this spawner once it has spawned the enemy
                Destroy(gameObject);
            }
            else if (checkX && checkY && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToSpawn && !spawned && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToSpawn && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToSpawn && !spawned && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToSpawn)
            {
                spawned = true;
                GameObject spawnedEnemy = Instantiate(enemyToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, spawnRotation + transform.eulerAngles.z));
                if (parentGameObject != null)
                {
                    spawnedEnemy.transform.parent = parentGameObject.transform;
                }
                else
                {
                    spawnedEnemy.transform.parent = gameObject.transform.parent;
                }
                //destroy this spawner once it has spawned the enemy
                Destroy(gameObject);
            }
        }
    }
}
