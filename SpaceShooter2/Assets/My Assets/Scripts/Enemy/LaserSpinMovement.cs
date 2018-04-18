using UnityEngine;

namespace Kontrol
{
    public class LaserSpinMovement : MonoBehaviour
    {
        public float moveSpeed;
        public float rotateSpeed;

        private void Start()
        {
            try
            {
                //set bullet rotation
                float rotationZ = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + 90);
            }
            catch
            {
            }
        }

        private void FixedUpdate()
        {
            transform.eulerAngles += new Vector3(0, 0, rotateSpeed);
        }
    }
}
