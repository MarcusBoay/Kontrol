using UnityEngine;

namespace Kontrol
{
    public class EnemySpawnerV2 : MonoBehaviour
    {
        public GameObject parentGameObject;
        public Vector2 distanceFromCameraToSpawn;
        public bool checkX;
        public bool checkY;

        private bool isSpawned = false;
        private bool isInitialized = false;

        private void Start()
        {
            //disables every enemy that is not disabled in the beginning
            if (!isInitialized)
            {
                isInitialized = true;
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!isSpawned)
            {
                if (checkX && !checkY && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToSpawn.x && !isSpawned && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToSpawn.x)
                {
                    isSpawned = true;

                    SpawnEnemy();

                    //destroy this spawner once it has spawned the enemy
                    Destroy(gameObject);
                }
                else if (checkY && !checkX && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToSpawn.y && !isSpawned && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToSpawn.y)
                {
                    isSpawned = true;

                    SpawnEnemy();

                    //destroy this spawner once it has spawned the enemy
                    Destroy(gameObject);
                }
                else if (checkX && checkY && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToSpawn.y && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToSpawn.y && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToSpawn.x && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToSpawn.x)
                {
                    isSpawned = true;

                    SpawnEnemy();

                    //destroy this spawner once it has spawned the enemy
                    Destroy(gameObject);
                }
            }
        }

        private void SpawnEnemy()
        {
            //set enemy as active and set enemy parent
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
                if (parentGameObject != null)
                {
                    gameObject.transform.GetChild(i).gameObject.transform.parent = parentGameObject.transform;
                }
                else
                {
                    gameObject.transform.GetChild(i).gameObject.transform.parent = gameObject.transform.parent;
                }
            }
        }
    }

}