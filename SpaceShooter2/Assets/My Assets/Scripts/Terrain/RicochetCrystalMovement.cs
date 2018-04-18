using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class RicochetCrystalMovement : MonoBehaviour
    {
        private Rigidbody2D rb2d;

        public Vector2 movementDirection;
        private Vector3 ricochetDirection;
        public float speed;
        public string ricochetSoundName;
        private Vector2 cameraDistance;
        private bool isInCameraView;

        private AudioManager audioManager;

        private void Start()
        {
            //crystal's rigid body reference
            rb2d = GetComponent<Rigidbody2D>();
            //initialize ricochetDirection as movementDirection
            ricochetDirection = new Vector3(movementDirection.x, movementDirection.y, transform.position.z);
            audioManager = AudioManager.instance;
            Assert.IsNotNull(ricochetSoundName);
            isInCameraView = false;
            cameraDistance = new Vector2(10, 10);
        }

        private void Update()
        {
            if (!isInCameraView && Mathf.Abs
                (Camera.main.transform.position.x - transform.position.x) <= cameraDistance.x && Mathf.Abs
                (Camera.main.transform.position.y - transform.position.y) <= cameraDistance.y)
            {
                isInCameraView = true;
                //move crystal
                rb2d.velocity = ricochetDirection.normalized * speed;
            }
            else if (Mathf.Abs
                (Camera.main.transform.position.x - transform.position.x) >= cameraDistance.x || Mathf.Abs
                (Camera.main.transform.position.y - transform.position.y) >= cameraDistance.y)
            {
                isInCameraView = false;
                //stop moving crystal
                rb2d.velocity = new Vector3(0, 0, 0);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //if crystal collides with terrain, reverse movement direction
            if (other.gameObject.tag == "Background" || other.gameObject.tag == "EnemyTerrain")
            {
                //redirect ricochet crystal movement
                ricochetDirection = -ricochetDirection;
                rb2d.velocity = ricochetDirection.normalized * speed;
                //play sound
                audioManager.PlaySound(ricochetSoundName);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //if crystal collides with terrain, reverse movement direction
            if (other.gameObject.tag == "Background" || other.gameObject.tag == "EnemyTerrain")
            {
                //redirect ricochet crystal movement
                ricochetDirection = -ricochetDirection;
                rb2d.velocity = ricochetDirection.normalized * speed;
                //play sound
                audioManager.PlaySound(ricochetSoundName);
            }
        }
    }
}
