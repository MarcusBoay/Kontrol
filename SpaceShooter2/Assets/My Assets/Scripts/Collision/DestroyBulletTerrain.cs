using UnityEngine;

namespace Kontrol
{
    public class DestroyBulletTerrain : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "MainCamera" || other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "PlayerRicochetBullet" || other.gameObject.tag == "EnemyRicochetBullet" || other.gameObject.tag == "Background" || other.gameObject.tag == "DestructibleTerrain" || other.gameObject.tag == "Respawn")
            {
                //do nothing
            }
            else
            {
                Destroy(other.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "EnemyBullet")
            {
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "PlayerBullet")
            {
                Instantiate(other.gameObject.GetComponent<PlayerBulletCollide>().explosion, other.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "EnemyRicochetBullet" || other.gameObject.tag == "DestructibleTerrain")
            {
                //do nothing
            }
        }
    }

}