using UnityEngine;

namespace Kontrol
{
    public class RicochetBulletMovement : MonoBehaviour
    {
        private float startTime;
        public float speed;
        public LayerMask collisionMask;
        private Vector2 shootDirection;

        private Rigidbody2D rb2d;
        public float timeBeforeDisappear;

        void Start()
        {
            //initialization
            rb2d = GetComponent<Rigidbody2D>();
            shootDirection = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
            //start time
            startTime = Time.time;
            try
            {
                //set bullet rotation
                float rotationZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + 90);
            }
            catch
            {
                Destroy(gameObject);
            }
        }

        void FixedUpdate()
        {
            //ricochet logic
            //make ray from bullet's position directed towards travel direction
            Ray2D ray = new Ray2D(transform.position, new Vector2(shootDirection.x, shootDirection.y).normalized);
            //make raycast hit to gather info about collider that raycast hits
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(shootDirection.x, shootDirection.y).normalized, Time.deltaTime * speed + 0.2f, collisionMask);
            //if raycast hits collider, make bullet reflect
            if (hit)
            {
                //make reflected shoot direction
                shootDirection = Vector2.Reflect(ray.direction, hit.normal);
                //make rotation variable in refelcted direction
                float rotation = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                //set bullet rotation in reflected direction
                transform.eulerAngles = new Vector3(0, 0, rotation + 90);
                //set bullet speed in reflected direction
                rb2d.velocity = shootDirection.normalized * speed;
            }
            //check if bullet time is up, if bullet time is up, destroy bullet
            if (Time.time - startTime > timeBeforeDisappear)
            {
                Destroy(gameObject);
            }
        }
    }
}
