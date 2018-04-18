using UnityEngine;

namespace Kontrol
{
    public class LaserMovement : MonoBehaviour
    {
        public float moveSpeed;

        private void Start()
        {
            try
            {
                //set bullet rotation
                float rotationZ = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
            }
            catch
            {
            }
        }
    }
}
