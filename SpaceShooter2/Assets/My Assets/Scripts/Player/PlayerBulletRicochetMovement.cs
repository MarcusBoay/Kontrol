using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class PlayerBulletRicochetMovement : MonoBehaviour
    {

        public GameObject SFXPrefab;
        public LayerMask collisionMask1;
        public LayerMask collisionMask2;
        private Rigidbody2D rb2d;
        private GameObject player;
        private Vector3 shootDirection;
        public float offsetX;
        public float offsetY;
        public float deviation;
        public float speed;
        private float startTime;
        public float timeBeforeDisappear;

        private AudioManager audioManager;

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
            //start time
            startTime = Time.time;
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
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
            //bullet state machine
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
                        shootDirection = Quaternion.Euler(0, 0, deviation) * shootDirection;
                        //set bullet rotation
                        float rotationZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
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
                    //ricochet logic
                    //make ray from bullet's position directed towards travel direction
                    Ray2D ray = new Ray2D(transform.position, new Vector2(shootDirection.x, shootDirection.y).normalized);
                    //make raycast hit to gather info about collider that raycast hits
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(shootDirection.x, shootDirection.y).normalized, Time.deltaTime * speed, collisionMask1);
                    //if raycast hits collider, make bullet reflect
                    if (hit)
                    {
                        //make reflected shoot direction
                        shootDirection = Vector2.Reflect(ray.direction, hit.normal);
                        //make rotation variable in refelcted direction
                        float rotation = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                        //set bullet rotation in reflected direction
                        transform.eulerAngles = new Vector3(0, 0, rotation);
                        //set bullet speed in reflected direction
                        rb2d.velocity = shootDirection.normalized * speed;
                    }
                    //check if bullet time is up, if bullet time is up, destroy bullet
                    if (Time.time - startTime > timeBeforeDisappear)
                    {
                        Destroy(gameObject);
                    }
                    break;
                case (BulletState.NOTACTIVE):
                    break;
            }
        }
    }
}
