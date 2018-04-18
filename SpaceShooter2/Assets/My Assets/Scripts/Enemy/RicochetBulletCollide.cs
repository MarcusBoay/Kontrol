using UnityEngine;

namespace Kontrol
{
    public class RicochetBulletCollide : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "PlayerRicochetBullet" || other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerBullet")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
            else if (other.gameObject.tag == "PlayerPenetrateBullet" || other.gameObject.tag == "PlayerInvulRicochetBullet")
            {
                Destroy(this.gameObject);
            }
        }
    }
}
