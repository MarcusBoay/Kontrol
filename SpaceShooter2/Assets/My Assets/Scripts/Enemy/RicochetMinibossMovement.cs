using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class RicochetMinibossMovement : MonoBehaviour
    {
        [Header("Prefabs")]
        private Rigidbody2D rb2d;
        public GameObject ricochetShooter;
        public GameObject[] normalShooter;
        public GameObject ricochetBullet;
        public GameObject normalBullet;
        private GameObject player;
        private Vector2 playerPosition;
        private AudioManager audioManager;

        private float nextPhaseTime;

        [Header("General variables")]
        public string ricochetSoundName;
        public string bulletSoundName;

        [Header("Spawn variables")]
        public Vector2 spawnSpeed;
        public float spawnTime;

        [Header("Phase 1 variables")]
        public float phase1Time;
        public Vector2 moveSpeed1;
        public float moveTime1;
        private float nextMove1;
        [Space(5)]
        public float shootDuration1;
        private float nextShoot1;
        public float degreeDeviation;
        public int numberOfRicochetBullets;
        public Vector2 offsetRicochet;
        public float ricochetBulletSpeed;

        [Header("Phase 2 variables")]
        public float phase2Time;
        [Space(5)]
        public float shootDuration2;
        private float nextShoot2;
        public Vector2 offsetNormal;
        public float normalBulletSpeed;

        [Header("Despawn variables")]
        public Vector2 despawnSpeed;
        public float despawnTime;
        public float timeToDespawn;

        public enum _enemyState
        {
            SPAWNING,
            ALIVE1,
            ALIVE2,
            DESPAWN,
            DEAD
        }
        public _enemyState enemyState;

        void Start()
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            enemyState = _enemyState.SPAWNING;
            nextPhaseTime = Time.time;
            try
            {
                player = GameObject.Find("Player").gameObject;
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }
        }

        void FixedUpdate()
        {
            //find player in case player is dead
            try
            {
                if (player == null)
                {
                    player = GameObject.Find("Player").gameObject;
                }
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }

            //add camera movement to miniboss
            transform.position += new Vector3(Camera.main.GetComponent<CameraMovement>().moveSpeed.x, Camera.main.GetComponent<CameraMovement>().moveSpeed.y, 0);

            try
            {
                switch (enemyState)
                {
                    case (_enemyState.SPAWNING):
                        //enemy moves in from right into screen then stops
                        transform.position += new Vector3(spawnSpeed.x, spawnSpeed.y, 0);

                        if (Time.time > nextPhaseTime + spawnTime)
                        {
                            nextPhaseTime = Time.time;
                            timeToDespawn = Time.time;
                            enemyState = _enemyState.ALIVE1;
                        }
                        break;

                    case (_enemyState.ALIVE1):
                        //enemy shoots a few ricochet bullets at player a few times before going to alive2
                        //movement
                        if (Time.time > nextMove1 + moveTime1)
                        {
                            nextMove1 = Time.time;
                            if (playerPosition.y < gameObject.transform.position.y)
                            {
                                //move down if player is down
                                rb2d.AddForce(-moveSpeed1);
                            }
                            else if (playerPosition.y >= gameObject.transform.position.y)
                            {
                                //move up if player is up
                                rb2d.AddForce(moveSpeed1);
                            }
                        }
                        //shooting
                        if (Time.time > nextShoot1 + shootDuration1)
                        {
                            nextShoot1 = Time.time;
                            try
                            {
                                //find vector from enemy to player
                                Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - ricochetShooter.transform.position;
                                //spawn point of bullet
                                Vector3 startPoint = new Vector3(ricochetShooter.transform.position.x + offsetRicochet.x, ricochetShooter.transform.position.y + offsetRicochet.y, transform.position.z);
                                //shoot bullets
                                ShootInDirection.ShootEqualSpread(ricochetBullet, numberOfRicochetBullets, degreeDeviation, startPoint, distToPlayer.normalized, ricochetBullet.GetComponent<RicochetBulletMovement>().speed);
                                //play sound
                                audioManager.PlaySound(ricochetSoundName);
                            }
                            catch
                            {
                                //player is dead, do nothing
                            }
                        }
                        //attack phase change
                        if (Time.time > nextPhaseTime + phase1Time)
                        {
                            nextPhaseTime = Time.time;
                            enemyState = _enemyState.ALIVE2;
                        }
                        //despawn
                        if (Time.time > timeToDespawn + despawnTime)
                        {
                            enemyState = _enemyState.DESPAWN;
                        }
                        break;

                    case (_enemyState.ALIVE2):
                        //enemy shoots bullets at player a few times before going back to alive1
                        //shooting
                        if (Time.time > nextShoot2 + shootDuration2)
                        {
                            nextShoot2 = Time.time;
                            try
                            {
                                for (int i = 0; i < normalShooter.Length; i++)
                                {
                                    //find vector from enemy to player
                                    Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - normalShooter[i].transform.position;
                                    //spawn point of bullet
                                    Vector3 startPoint = new Vector3(normalShooter[i].transform.position.x + offsetNormal.x, normalShooter[i].transform.position.y + offsetNormal.y, transform.position.z);
                                    //instantiate bullet
                                    GameObject bullet = (GameObject)Instantiate(normalBullet, startPoint, Quaternion.identity);
                                    //set velocity of bullet using unit vector of distance from enemy to player
                                    bullet.GetComponent<Rigidbody2D>().velocity = distToPlayer.normalized * normalBulletSpeed;
                                    //play sound
                                    audioManager.PlaySound(bulletSoundName);
                                }
                            }
                            catch
                            {
                                //player is dead, do nothing
                            }
                        }
                        //attack phase change
                        if (Time.time > nextPhaseTime + phase2Time)
                        {
                            nextPhaseTime = Time.time;
                            enemyState = _enemyState.ALIVE1;
                        }
                        //despawn
                        if (Time.time > timeToDespawn + despawnTime)
                        {
                            enemyState = _enemyState.DESPAWN;
                        }
                        break;

                    case (_enemyState.DESPAWN):
                        //if time elapsed is more than despawn time, enter god mode and move right and get destroyed by out of camera trigger
                        transform.position += new Vector3(despawnSpeed.x, despawnSpeed.y, 0);
                        break;

                    case (_enemyState.DEAD):
                        //probably nothing
                        break;
                }
            }
            catch { }
        }
    }
}
