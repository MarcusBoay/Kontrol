using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class EnemyAI6 : MonoBehaviour
    {
        private int _bulletQuantity;
        public float maxBulletQuantity;

        public float startShootWait;
        public float ricochetFireRateShort;
        public float ricochetFireRateLong;
        private float nextFire = 0.0f;

        public float offsetX;
        public float offsetY;

        public float degreeDeviation;

        public string ricochetShotSoundName;

        private Rigidbody2D rb2d;
        public GameObject mBullet;
        private AudioManager audioManager;

        private void Start()
        {
            //initializing
            rb2d = GetComponent<Rigidbody2D>();
            nextFire = Time.time + startShootWait;
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
        }

        private void FixedUpdate()
        {
            //for bullets
            MakeBullets();
        }

        private void MakeBullets()
        {
            if (Time.time > nextFire)
            {
                try
                {
                    _bulletQuantity += 1;

                    //spawn point of bullet
                    Vector3 startPoint = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
                    //shoot bullet
                    ShootInDirection.Shoot(mBullet, 1, degreeDeviation, startPoint, transform.up, mBullet.GetComponent<RicochetBulletMovement>().speed, false);
                    //play sound
                    audioManager.PlaySound(ricochetShotSoundName);

                    if (_bulletQuantity >= maxBulletQuantity)
                    {
                        nextFire = Time.time + ricochetFireRateLong;
                        _bulletQuantity = 0;
                    }
                    else
                    {
                        nextFire = Time.time + ricochetFireRateShort;
                    }
                }
                catch
                { }
            }
        }
    }
}