using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class EnemyCollide : MonoBehaviour
    {
        public int health;
        public GameObject explosion;
        public GameObject[] childrenToBlink;
        public GameObject powerUp;
        public string enemyHitSoundName;
        public string enemyExplodeSoundName;

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

                Destroy(this.gameObject);

                audioManager.PlaySound(enemyExplodeSoundName);
                gameObject.GetComponent<EnemyScore>().AddScore();
                Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            }
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
            //Instantiate(blinkAudio, transform.position, transform.rotation);
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1);
            //blink children
            try
            {
                foreach (GameObject child in childrenToBlink)
                {
                    child.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 1);
                }
            }
            catch
            {
                Debug.Log("Couldn't blink child");
            }
            gameObject.tag = "Untagged";
            yield return new WaitForSeconds(invulDuration); //original value = 0.02f
            GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0);
            try
            {
                foreach (GameObject child in childrenToBlink)
                {
                    child.GetComponent<SpriteRenderer>().material.SetFloat("_FlashAmount", 0);
                }
            }
            catch
            {
                Debug.Log("Couldn't blink child");
            }
            gameObject.tag = _tag;
        }
    }
}
