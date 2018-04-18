using UnityEngine;

namespace Kontrol
{
    public class RicochetCoreMovement : MonoBehaviour
    {
        [Header("Prefabs")]
        private Rigidbody2D rb2d;
        private GameObject player;
        private Vector2 playerPosition;

        private float nextPhaseTime;
        private float nextMove;

        [Header("Phase 1 variables")]
        public Vector2 moveSpeed1;
        public float moveTime1;

        [Header("Phase 2 variables")]
        public Vector2 moveSpeed2;
        public float moveTime2;

        public enum _enemyState
        {
            SPAWNING,
            PHASE1,
            PHASE2,
            NEUTRAL,
            DEAD
        }
        public _enemyState enemyState;

        void Start()
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
            enemyState = _enemyState.SPAWNING;
            nextPhaseTime = Time.time;
            try
            {
                player = GameObject.Find("Player").gameObject;
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }
        }

        void FixedUpdate()
        {
            //find player in case player is dead
            try
            {
                if (player == null)
                {
                    player = GameObject.Find("Player").gameObject;
                }
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.50f, 4.50f), transform.position.z);

            switch (enemyState)
            {
                case (_enemyState.SPAWNING):
                    //waiting for boss body to enter phase A1
                    break;

                case (_enemyState.PHASE1):
                    //movement
                    if (Time.time > nextMove + moveTime1)
                    {
                        nextMove = Time.time;
                        if (playerPosition.y < gameObject.transform.position.y)
                        {
                            //move down if player is down
                            rb2d.AddForce(-moveSpeed1);
                        }
                        else if (playerPosition.y >= gameObject.transform.position.y)
                        {
                            //move up if player is up
                            rb2d.AddForce(moveSpeed1);
                        }
                    }
                    break;

                case (_enemyState.PHASE2):
                    //movement
                    if (Time.time > nextMove + moveTime2)
                    {
                        nextMove = Time.time;
                        if (playerPosition.y < gameObject.transform.position.y)
                        {
                            //move down if player is down
                            rb2d.AddForce(-moveSpeed2);
                        }
                        else if (playerPosition.y >= gameObject.transform.position.y)
                        {
                            //move up if player is up
                            rb2d.AddForce(moveSpeed2);
                        }
                    }
                    break;

                case (_enemyState.NEUTRAL):
                    //waiting state
                    break;

                case (_enemyState.DEAD):
                    //probably nothing
                    break;
            }
        }
    }
}
