using UnityEngine;

namespace Kontrol
{
    public class PlayerBulletMovement : MonoBehaviour
    {
        private Rigidbody2D rb2d;
        private GameObject player;
        public float offsetX;
        public float offsetY;
        public float speed;

        private bool setSpeed;
        private Vector3 shootDirection;
        private float rotationZ;

        public enum BulletState
        {
            JUSTACTIVE,
            ALIVE,
            NOTACTIVE
        }
        public BulletState myBulletState;

        void Start()
        {
            //bullet's player reference
            player = GameObject.Find("Player").gameObject;
            //bullet's rigid body reference
            rb2d = GetComponent<Rigidbody2D>();
            //when bullet is created, set state to JUST ACTIVE
            myBulletState = BulletState.JUSTACTIVE;
            setSpeed = false;
        }

        void FixedUpdate()
        {
            //to reference the spawned player
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            { }
            //bulet state machine
            switch (myBulletState)
            {
                case (BulletState.JUSTACTIVE):
                    try
                    {
                        //set bullet position to be infront of the player
                        transform.position = player.transform.position + new Vector3(1, 0, 0) * offsetX + new Vector3(0, 1, 0) * offsetY;
                        //getting direction from bullet to mouse position
                        shootDirection = Input.mousePosition;
                        shootDirection.z = 0.0f;
                        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
                        shootDirection = shootDirection - transform.position;
                        //get bullet rotation
                        rotationZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                        //set bullet rotation
                        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
                        //set bullet state to alive
                        myBulletState = BulletState.ALIVE;
                    }
                    catch
                    {
                        Destroy(gameObject);
                    }
                    break;
                case (BulletState.ALIVE):
                    if (!setSpeed)
                    {
                        setSpeed = true;
                        //set bullet x velocity
                        rb2d.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * speed;
                    }
                    break;
                case (BulletState.NOTACTIVE):
                    break;
            }
        }
    }
}
