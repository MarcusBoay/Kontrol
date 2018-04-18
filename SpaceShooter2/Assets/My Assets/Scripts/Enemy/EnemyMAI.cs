using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class EnemyMAI : MonoBehaviour
    {
        public float rotateSpeed;
        public int health;

        public GameObject[] ricochetSpawners;
        public GameObject ricochetBullet;

        public GameObject explosion;
        public GameObject blinkAudio;
        public GameObject powerUp;

        public string enemyHitSoundName;
        public string explodeSoundName;
        private AudioManager audioManager;
        private string _tag;

        private void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);

            _tag = gameObject.tag;
        }

        void Update()
        {
            if (health <= 0)
            {
                //drop powerup if there is one
                if (powerUp != null)
                {
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }
                //spawn ricochet bullets
                for (int i = 0; i < ricochetSpawners.Length; i++)
                {
                    //spawn bullets only if ricochet spawner is active
                    if (ricochetSpawners[i].activeSelf)
                    {
                        ShootInDirection.Shoot(ricochetBullet, 1, 0, ricochetSpawners[i].transform.position, ricochetSpawners[i].transform.up, ricochetBullet.GetComponent<RicochetBulletMovement>().speed, true);
                    }
                }
                //destroys enemy gameobject when hp = 0
                Destroy(this.gameObject);

                audioManager.PlaySound(explodeSoundName);
                gameObject.GetComponent<EnemyScore>().AddScore();
                Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            }
        }

        private void FixedUpdate()
        {
            transform.eulerAngles += new Vector3(0, 0, rotateSpeed);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (this.tag != "InvulEnemy")
            {
                if (other.tag == "PlayerBullet" || other.tag == "PlayerRicochetBullet")
                {
                    //destroy player bullet on collision with enemy
                    Destroy(other.gameObject);
                    if (other.tag == "PlayerBullet")
                    {
                        Instantiate(other.GetComponent<PlayerBulletCollide>().explosion, other.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
                    }
                    //damage enemy hp
                    health = health - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                    //blink on hit if enemy is not ded
                    if (health > 0)
                    {
                        StartCoroutine(BlinkOnHit(0.02f));
                        //play sound
                        audioManager.PlaySound(enemyHitSoundName);
                    }
                }
                else if (other.tag == "PlayerInvulRicochetBullet")
                {
                    //damage enemy hp
                    health = health - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                    //blink on hit if enemy is not ded
                    if (health > 0)
                    {
                        StartCoroutine(BlinkOnHit(0.02f));
                        //play sound
                        audioManager.PlaySound(enemyHitSoundName);
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (this.tag != "InvulEnemy")
            {
                if (other.tag == "PlayerPenetrateBullet" && this.tag == "Enemy" || other.tag == "PlayerPenetrateBullet" && this.tag == "EnemyTerrain")
                {
                    //damage enemy hp
                    health = health - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                    //blink on hit if enemy is not ded
                    if (health > 0)
                    {
                        StartCoroutine(BlinkOnHit(0.015f));
                        //play sound
                        audioManager.PlaySound(enemyHitSoundName);
                    }
                }
            }
        }

        IEnumerator BlinkOnHit(float invulDuration)
        {
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1);
            gameObject.tag = "Untagged";
            yield return new WaitForSeconds(invulDuration); //original value = 0.02f
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0);
            gameObject.tag = _tag;
        }
    }
}
