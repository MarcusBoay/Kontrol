using UnityEngine;

namespace Kontrol
{
    public class HomingMissile : MonoBehaviour
    {
        public float moveSpeed;
        public float rotatingSpeed;

        private GameObject player;
        private Rigidbody2D rb2d;

        private float _startTime;

        void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            _startTime = Time.time;

            //find player GO
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            {
                //ignored
            }
        }

        void FixedUpdate()
        {
            //find player GO
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            {
                //ignored
            }

            try
            {
                Vector2 pointToTarget = (Vector2)transform.position - (Vector2)player.transform.position;
                pointToTarget.Normalize();

                float value = Vector3.Cross(pointToTarget, -transform.right).z;

                rb2d.angularVelocity = rotatingSpeed * value;
                rb2d.velocity = -transform.right * moveSpeed * (Time.time - _startTime + 1);
            }
            catch
            {
                Destroy(this.gameObject);
            }
        }
    }
}
