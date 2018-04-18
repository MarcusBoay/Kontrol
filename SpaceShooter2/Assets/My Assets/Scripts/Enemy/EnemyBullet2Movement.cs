using UnityEngine;

namespace Kontrol
{
    public class EnemyBullet2Movement : MonoBehaviour
    {
        public float moveSpeed;

        void FixedUpdate()
        {
            transform.position += -transform.right * Time.deltaTime * moveSpeed;
        }
    }
}
