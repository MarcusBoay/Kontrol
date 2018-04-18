using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class PlayerController : MonoBehaviour
    {
        float moveHorizontal;
        float moveVertical;
        private Rigidbody2D rb2d;
        public float[] moveSpeed;
        private float _moveSpeed;
        public float GetMoveSpeed()
        {
            return _moveSpeed;
        }
        private int speedSwitch;
        public float invincibleTime;

        public GameObject bulletNormal;
        public GameObject bulletRicochet;
        public GameObject bulletRicochetUp;
        public GameObject bulletRicochetDown;
        public GameObject bulletRicochetInvul;
        public GameObject bulletRicochetInvulUp;
        public GameObject bulletRicochetInvulDown;
        public GameObject bulletFollow;
        public GameObject bulletPenetrate;
        public float normalFireRate1;
        public float normalFireRate2;
        public float ricochetFireRate1;
        public float ricochetFireRate2;
        public float crawlerFireRate;
        public float penetrateFireRate1;
        public float penetrateFireRate2;
        private float _nextFire = 0.0f;

        public string normalBulletSoundName;
        public string ricochetBulletSoundName;
        public string penetrateBulletSoundName;

        public enum BulletType
        {
            NORMAL1,
            NORMAL2,
            RICOCHET1,
            RICOCHET2,
            FOLLOW,
            PENETRATE1,
            PENETRATE2
        }
        public BulletType myBulletType;

        private GameObject myCamera;
        private AudioManager audioManager;
        private SpeedManager speedManager;

        void Start()
        {
            //initialization
            rb2d = GetComponent<Rigidbody2D>();
            myCamera = Camera.main.gameObject;
            Assert.IsNotNull(myCamera);
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            speedSwitch = 1;
            _moveSpeed = moveSpeed[speedSwitch];
            speedManager = SpeedManager.instance;

            StartCoroutine(Invincibility(this.gameObject));
        }

        private void Update()
        {
            //detecting for player input for shooting
            MakeBullet();
            //detecting for change in move speed
            ChangeMoveSpeed();
            //rotate player
            //getting direction from bullet to mouse position
            Vector3 shootDirection = Input.mousePosition;
            shootDirection.z = 0.0f;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            shootDirection = shootDirection - transform.position;
            //get bullet rotation
            float rotationZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            //set bullet rotation
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6.16f + myCamera.transform.position.x, 6.3f + myCamera.transform.position.x), Mathf.Clamp(transform.position.y, -4.7f + myCamera.transform.position.y, 4.7f + myCamera.transform.position.y), 0);
            transform.position += new Vector3(myCamera.GetComponent<CameraMovement>().moveSpeed.x, myCamera.GetComponent<CameraMovement>().moveSpeed.y, 0);
            //getting inputs
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            //setting player move speed
            rb2d.velocity = new Vector3(moveHorizontal, moveVertical, 0) * _moveSpeed;
        }

        private void MakeBullet()
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > _nextFire || Input.GetKey(KeyCode.Mouse0) && Time.time > _nextFire)
            {
                //state machine for different weapons
                switch (myBulletType)
                {
                    case (BulletType.NORMAL1):
                        //make bullet
                        GameObject thisBullet1 = Instantiate(bulletNormal);
                        //set parent and activate
                        thisBullet1.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet1.SetActive(true);
                        //play sound
                        audioManager.PlaySound(normalBulletSoundName);
                        //next shoot time
                        _nextFire = Time.time + normalFireRate1;
                        break;
                    case (BulletType.NORMAL2):
                        //make bullet
                        GameObject thisBullet2 = Instantiate(bulletNormal);
                        //set parent and activate
                        thisBullet2.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet2.SetActive(true);
                        //play sound
                        audioManager.PlaySound(normalBulletSoundName);
                        //next shoot time
                        _nextFire = Time.time + normalFireRate2;
                        break;
                    case (BulletType.RICOCHET1):
                        //make bullet
                        GameObject thisBullet3r1 = Instantiate(bulletRicochet);
                        GameObject thisBullet3r2 = Instantiate(bulletRicochetUp);
                        GameObject thisBullet3r3 = Instantiate(bulletRicochetDown);

                        //set parent and activate
                        thisBullet3r1.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet3r1.SetActive(true);
                        thisBullet3r2.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet3r2.SetActive(true);
                        thisBullet3r3.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet3r3.SetActive(true);

                        //play sound
                        audioManager.PlaySound(ricochetBulletSoundName);
                        _nextFire = Time.time + ricochetFireRate1;
                        break;
                    case (BulletType.RICOCHET2):
                        //make bullet
                        GameObject thisBullet4r1 = Instantiate(bulletRicochetInvul);
                        GameObject thisBullet4r2 = Instantiate(bulletRicochetInvulUp);
                        GameObject thisBullet4r3 = Instantiate(bulletRicochetInvulDown);

                        //set parent and activate
                        thisBullet4r1.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet4r1.SetActive(true);
                        thisBullet4r2.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet4r2.SetActive(true);
                        thisBullet4r3.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet4r3.SetActive(true);

                        //play sound
                        audioManager.PlaySound(ricochetBulletSoundName);
                        _nextFire = Time.time + ricochetFireRate2;
                        break;
                    case (BulletType.FOLLOW):
                        GameObject thisBulletX = Instantiate(bulletFollow);
                        thisBulletX.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBulletX.SetActive(true);
                        _nextFire = Time.time + crawlerFireRate;
                        break;
                    case (BulletType.PENETRATE1):
                        GameObject thisBullet5 = Instantiate(bulletPenetrate);
                        thisBullet5.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet5.SetActive(true);
                        //play sound
                        audioManager.PlaySound(penetrateBulletSoundName);
                        _nextFire = Time.time + penetrateFireRate1;
                        break;
                    case (BulletType.PENETRATE2):
                        GameObject thisBullet6 = Instantiate(bulletPenetrate);
                        thisBullet6.transform.parent = GameObject.Find("PlayerBullets").transform;
                        thisBullet6.SetActive(true);
                        //play sound
                        audioManager.PlaySound(penetrateBulletSoundName);
                        _nextFire = Time.time + penetrateFireRate2;
                        break;
                }
            }
        }

        public void ChangeMoveSpeed()
        {
            speedManager.SetSpeedText(speedSwitch);
            //decrease player move speed index
            if (Input.GetKeyDown(KeyCode.Q) && speedSwitch > 0)
            {
                speedSwitch--;
            }
            //increase player move speed index
            else if (Input.GetKeyDown(KeyCode.E) && speedSwitch < moveSpeed.Length - 1)
            {
                speedSwitch++;
            }
            //change player move speed
            _moveSpeed = moveSpeed[speedSwitch];
        }

        IEnumerator Invincibility(GameObject player)
        {
            var _tag = player.tag;
            player.tag = "Respawn";
            player.GetComponent<SpriteRenderer>().color = new Color(1, 0.92f, 0.016f, 1);
            player.GetComponent<CircleCollider2D>().isTrigger = true;
            yield return new WaitForSeconds(invincibleTime);
            player.tag = _tag;
            player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            player.GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }
}