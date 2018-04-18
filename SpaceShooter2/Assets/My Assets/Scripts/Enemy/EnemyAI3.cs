using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class EnemyAI3 : MonoBehaviour
    {
        public float xSpeed;
        public float ySpeed;

        public bool willShoot;

        public float startShootWait;
        public float shootRate;

        public int bulletQuantity = 1;
        public float deviation = 0;

        public float offsetX;
        public float offsetY;
        public float bulletSpeed;
        public string bulletSoundName;

        //public int maxLoop;
        //private int _loop;

        private GameObject player;
        private Rigidbody2D rb2d;
        public GameObject Bullet;
        //private GameObject LM;
        private AudioManager audioManager;

        private float _nextShoot;

        void Start()
        {
            //initializing
            rb2d = GetComponent<Rigidbody2D>();
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            _nextShoot = Time.time - shootRate + startShootWait;
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

            //find player GO
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            {
                //ignored
            }
        }

        void FixedUpdate()
        {
            //move enemy in direction
            //rb2d.MovePosition(new Vector2(xSpeed + transform.position.x, ySpeed + transform.position.y));
            transform.position += new Vector3(xSpeed, ySpeed, 0);

            //find player GO
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            {
                // ignored
            }

            //shoot
            if (willShoot && Time.time >= _nextShoot + shootRate && player != null)
            {
                _nextShoot = Time.time;
                //find vector from enemy to player
                Vector3 distToPlayer = player.transform.position - transform.position - new Vector3(offsetX, offsetY, 0);
                //spawn point of bullet
                Vector3 startPoint = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
                //shoot bullet
                ShootInDirection.ShootEqualSpread(Bullet, bulletQuantity, deviation, startPoint, distToPlayer, bulletSpeed);
                //play sound
                audioManager.PlaySound(bulletSoundName);
            }
        }
    }
}