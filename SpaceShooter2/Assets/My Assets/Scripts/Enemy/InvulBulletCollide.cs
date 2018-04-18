using UnityEngine;

namespace Kontrol
{
    public class InvulBulletCollide : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "PlayerRicochetBullet" || other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerBullet")
            {
                Destroy(other.gameObject);
            }
        }
    }
}