using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class BossShieldCollide : MonoBehaviour
    {
        public int maxHealthA;
        public int maxHealthB;
        public int curHealth;
        public GameObject explosion;

        public string enemyHitSoundName;

        private AudioManager audioManager;

        void Start()
        {
            curHealth = maxHealthA;
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
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
                    curHealth = curHealth - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                    //blink on hit if enemy is not ded
                    if (curHealth > 0)
                    {
                        StartCoroutine(BlinkOnHit(0.02f));
                        //play sound
                        audioManager.PlaySound(enemyHitSoundName);
                    }
                }
                else if (other.tag == "PlayerInvulRicochetBullet")
                {
                    //damage enemy hp
                    curHealth = curHealth - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                    //blink on hit if enemy is not ded
                    if (curHealth > 0)
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
                if (other.tag == "PlayerPenetrateBullet" && this.tag == "Enemy")
                {
                    //damage enemy hp
                    curHealth = curHealth - other.gameObject.GetComponent<BulletDamage>().bulletDamage;
                    //blink on hit if enemy is not ded
                    if (curHealth > 0)
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
            gameObject.tag = "Enemy";
        }
    }
}