using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class ShipBomb : MonoBehaviour
    {
        public float initialSpeedMin;
        public float initialSpeedMax;
        public float rotateSpeed;
        public float rotateAccel;
        public float scaleAccel;
        public float deceleration;
        public float fuseTime;
        public float destroyTime;
        public bool moveInYDirection = true;
        public string bombSoundName;

        private AudioManager audioManager;
        private float _nextMove;
        private float _accel;
        private float _accelPower;
        private bool isSoundPlaying;

        private enum _enemyState
        {
            DECELERATING,
            STOPPED
        }
        private _enemyState enemyState;

        void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);

            _nextMove = Time.time;

            _accelPower = Random.Range(initialSpeedMin, initialSpeedMax);
            _accel = deceleration;

            isSoundPlaying = false;

            enemyState = _enemyState.DECELERATING;
        }

        void FixedUpdate()
        {
            //add accel/decel movement to enemy
            _accelPower += _accel * Time.deltaTime;
            if (moveInYDirection)
                transform.position += new Vector3(0, _accelPower, 0);
            else
                transform.position += new Vector3(_accelPower, 0, 0);
            transform.eulerAngles += new Vector3(0, 0, rotateSpeed);

            //state machine
            switch (enemyState)
            {
                case (_enemyState.DECELERATING):
                    //change phase
                    if (_accelPower <= 0)
                    {
                        _accel = 0;
                        _accelPower = 0;
                        enemyState = _enemyState.STOPPED;
                        _nextMove = Time.time;
                    }
                    break;

                case (_enemyState.STOPPED):
                    //'explode' enemy
                    if (Time.time >= _nextMove + fuseTime)
                    {
                        transform.localScale += new Vector3(scaleAccel, scaleAccel, 0);
                        rotateSpeed += rotateAccel;

                        if (!isSoundPlaying)
                        {
                            isSoundPlaying = true;

                            audioManager.PlaySound(bombSoundName);
                        }
                    }

                    //destroy enemy
                    if (Time.time >= _nextMove + fuseTime + destroyTime)
                    {
                        Destroy(this.gameObject);
                    }
                    break;
            }
        }
    }
}
