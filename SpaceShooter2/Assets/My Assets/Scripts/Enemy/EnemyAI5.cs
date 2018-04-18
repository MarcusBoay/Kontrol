using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class EnemyAI5 : MonoBehaviour
    {
        public Vector2 moveSpeed;

        public bool willShoot;

        public float startShootWait;
        public float shootRate;
        public float offsetX;
        public float offsetY;
        public float bulletSpeed;

        public float timeTillDetectMode;
        public string bulletSoundName;

        //public int maxLoop;
        //private int _loop;

        private GameObject player;
        private Rigidbody2D rb2d;
        private CircleCollider2D cc2d;
        public GameObject Bullet;
        //private GameObject LM;
        private AudioManager audioManager;

        public enum _enemyState
        {
            SPAWNING,
            DETECTING,
            DETECTED
        }
        public _enemyState enemyState;

        void Start()
        {
            //initializing
            rb2d = GetComponent<Rigidbody2D>();
            cc2d = GetComponent<CircleCollider2D>();
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            //loop stuff
            //LM = GameObject.Find("LoopManager").gameObject;
            //_loop = 1;
            //if (LM.GetComponent<LoopManager>().loop <= maxLoop)
            //{
            //    _loop = LM.GetComponent<LoopManager>().loop;
            //}
            //else
            //{
            //    _loop = maxLoop;
            //}
            //GetComponent<EnemyScore>().score *= _loop;
            //if player is alive, find player gameobject
            try
            {
                player = GameObject.Find("Player").gameObject;
                //for bullets
                if (willShoot)
                {
                    StartCoroutine(SpawnBullets());
                }
            }
            catch
            {
                //player iz ded, do nothing
            }
            enemyState = _enemyState.SPAWNING;
            StartCoroutine(WaitToDetect());
        }

        void FixedUpdate()
        {
            //move enemy in  x direction
            switch (enemyState)
            {
                case (_enemyState.SPAWNING):
                    //move enemy in y direction
                    rb2d.MovePosition(new Vector2(transform.position.x, moveSpeed.y + transform.position.y));
                    break;
                case (_enemyState.DETECTING):
                    //move enemy in y direction
                    rb2d.MovePosition(new Vector2(transform.position.x, moveSpeed.y + transform.position.y));
                    //detect player in range of y variable, add radius of enemy
                    if (player != null)
                    {
                        if (player.transform.position.y < transform.position.y + cc2d.radius && player.transform.position.y > transform.position.y - cc2d.radius)
                        {
                            //if player is in front of enemy, make move speed negative
                            if (player.transform.position.x > transform.position.x)
                            {
                                moveSpeed.x = -moveSpeed.x;
                            }
                            //player is inside y range, move enemy state to DETECTED
                            enemyState = _enemyState.DETECTED;
                        }
                    }
                    break;
                case (_enemyState.DETECTED):
                    //move enemy in x direction
                    rb2d.MovePosition(new Vector2(moveSpeed.x + transform.position.x, transform.position.y));
                    break;
            }
        }

        IEnumerator SpawnBullets()
        {
            yield return new WaitForSeconds(startShootWait);
            while (GameObject.Find("Player") != null)
            {
                try
                {
                    //find vector from enemy to player
                    Vector3 distToPlayer = player.transform.position - transform.position;
                    //spawn point of bullet
                    Vector3 startPoint = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
                    //instantiate bullet
                    GameObject bullet = (GameObject)Instantiate(Bullet, startPoint, Quaternion.identity);
                    //set velocity of bullet using unit vector of distance from enemy to player
                    bullet.GetComponent<Rigidbody2D>().velocity = distToPlayer.normalized * bulletSpeed;
                    //play bullet sound
                    audioManager.PlaySound(bulletSoundName);
                }
                catch
                { }
                yield return new WaitForSeconds(shootRate);
            }
        }

        IEnumerator WaitToDetect()
        {
            yield return new WaitForSeconds(timeTillDetectMode);
            enemyState = _enemyState.DETECTING;
        }
    }
}