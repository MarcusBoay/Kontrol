using UnityEngine;

namespace Kontrol
{
    public class AccelBullet : MonoBehaviour
    {
        public float accelValue;

        private Rigidbody2D _rb2d;

        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            transform.position += new Vector3(Camera.main.GetComponent<CameraMovement>().moveSpeed.x, Camera.main.GetComponent<CameraMovement>().moveSpeed.y, 0);
            _rb2d.velocity += _rb2d.velocity * accelValue;
        }
    }
}