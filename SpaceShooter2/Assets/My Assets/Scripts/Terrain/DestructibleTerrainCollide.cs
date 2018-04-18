using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class DestructibleTerrainCollide : MonoBehaviour
    {

        public int health;
        public GameObject explosion;
        public GameObject powerUp;
        public string enemyHitSoundName;
        public string enemyExplodeSoundName;

        private AudioManager audioManager;

        private void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
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
                //destroys enemy gameobject when hp = 0
                Destroy(this.transform.parent.gameObject);

                audioManager.PlaySound(enemyExplodeSoundName);
                gameObject.GetComponent<EnemyScore>().AddScore();
                Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            }
        }

        //IMPORTANT NOTE!: OnTriggerEnter2D =/= OnTriggerEnter !!! 2D physics =/= 3D physics!!! They are programmed as DIFFERENT!!
        void OnTriggerEnter2D(Collider2D other)
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
            if (other.tag == "PlayerInvulRicochetBullet")
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

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "PlayerPenetrateBullet" && this.tag == "DestructibleTerrain")
            {
                //damage enemy hp
                health = health - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                //blink on hit if enemy is not ded
                if (health > 0)
                {
                    StartCoroutine(BlinkOnHit(0.01f));
                    //play sound
                    audioManager.PlaySound(enemyHitSoundName);
                }
            }
        }

        IEnumerator BlinkOnHit(float invulDuration)
        {
            string _tag = gameObject.tag;
            //Instantiate(blinkAudio, transform.position, transform.rotation);
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1);
            gameObject.tag = "Untagged";
            yield return new WaitForSeconds(invulDuration); //original value = 0.02f
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0);
            gameObject.tag = _tag;
        }
    }
}
