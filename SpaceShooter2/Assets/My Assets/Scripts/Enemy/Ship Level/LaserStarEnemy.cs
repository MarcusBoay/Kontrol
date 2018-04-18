using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class LaserStarEnemy : MonoBehaviour
    {
        public GameObject LaserStarGO;

        public float shootInterval;
        public float timeBeforeMove;

        public float acceleration;
        public float deceleration;
        public float initialSpeed;
        public string laserSoundName;

        private GameObject player;
        private float _nextShoot;
        private float _nextMove;
        private float _accel;
        private float _accelPower;

        private AudioManager audioManager;

        private enum _enemyState
        {
            DECELERATING,
            STOPPED,
            ACCELERATING
        }
        private _enemyState enemyState;

        void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            _nextShoot = Time.time;
            _nextMove = Time.time;
            enemyState = _enemyState.DECELERATING;

            _accelPower = initialSpeed;
            _accel = deceleration;

            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            {
            }
        }

        void FixedUpdate()
        {
            //get player position
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            {
            }

            //add accel/decel movement to enemy
            _accelPower += _accel * Time.deltaTime;
            transform.position += new Vector3(0, -_accelPower, 0);

            //state machine
            switch (enemyState)
            {
                case _enemyState.DECELERATING:
                    //change phase
                    if (_accelPower <= 0)
                    {
                        _accel = 0;
                        enemyState = _enemyState.STOPPED;
                        _nextShoot = Time.time - shootInterval;
                        _nextMove = Time.time;
                    }
                    break;

                case _enemyState.STOPPED:
                    //shoot laser
                    if (Time.time >= _nextShoot + shootInterval)
                    {
                        _nextShoot = Time.time;
                        try
                        {
                            player = GameObject.Find("Player").gameObject;
                        }
                        catch
                        {
                        }
                        try
                        {
                            //find vector from enemy to player
                            Vector3 distToPlayer = player.transform.position - transform.position;
                            //shoot
                            ShootInDirection.Shoot(LaserStarGO, 1, 0, transform.position, distToPlayer, LaserStarGO.GetComponent<LaserSpinMovement>().moveSpeed, true);
                            //play sound
                            audioManager.PlaySound(laserSoundName);
                        }
                        catch
                        {
                        }
                    }

                    //change phase
                    if (Time.time >= _nextMove + timeBeforeMove)
                    {
                        enemyState = _enemyState.ACCELERATING;
                    }
                    break;

                case _enemyState.ACCELERATING:
                    _accel = acceleration;
                    break;
            }
        }
    }
}
