using UnityEngine;

namespace Kontrol
{
    public class PlayerBulletPenetrate : MonoBehaviour
    {
        public GameObject SFXPrefab;
        private Rigidbody2D rb2d;
        private GameObject player;
        public float offsetX;
        public float offsetY;
        public float speed;
        public float rotateSpeed;

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
                        Vector3 shootDirection = Input.mousePosition;
                        shootDirection.z = 0.0f;
                        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
                        shootDirection = shootDirection - transform.position;
                        //set bullet rotation
                        float rotationZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + 90);
                        //set bullet x velocity
                        rb2d.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * speed;
                        //set bullet state to alive
                        myBulletState = BulletState.ALIVE;
                    }
                    catch
                    {
                        Destroy(gameObject);
                    }
                    break;
                case (BulletState.ALIVE):
                    //keep moving bullet with respect to camera movement
                    //transform.Translate(new Vector3(Camera.main.GetComponent<CameraMovement>().moveSpeed.x, Camera.main.GetComponent<CameraMovement>().moveSpeed.y, 0));
                    //rotate bullet for fun
                    transform.eulerAngles += new Vector3(0, 0, rotateSpeed);
                    break;
                case (BulletState.NOTACTIVE):
                    break;
            }
        }
    }
}
