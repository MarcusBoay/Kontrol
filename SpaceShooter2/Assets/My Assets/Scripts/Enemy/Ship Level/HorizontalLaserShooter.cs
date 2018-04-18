using UnityEngine;

namespace Kontrol
{
    public class HorizontalLaserShooter : MonoBehaviour
    {
        public GameObject LaserGO;

        public float shootInterval;
        public float spawnRotation;
        public float xSpeed;
        public float ySpeed;
        public string laserSoundName;

        private float _nextShoot;
        private AudioManager audioManager;

        void Start()
        {
            _nextShoot = Time.time + Random.Range(0f, 0.5f);
            transform.eulerAngles = new Vector3(0, 0, spawnRotation);

            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!!!");
            }
        }

        void FixedUpdate()
        {
            //move enemy in direction
            transform.position += new Vector3(xSpeed, ySpeed, 0);

            //shoot
            if (Time.time >= _nextShoot + shootInterval)
            {
                _nextShoot = Time.time;
                ShootInDirection.Shoot(LaserGO, 1, 0, transform.position, transform.right, LaserGO.GetComponent<LaserMovement>().moveSpeed, true);
                ShootInDirection.Shoot(LaserGO, 1, 0, transform.position, -transform.right, LaserGO.GetComponent<LaserMovement>().moveSpeed, true);

                audioManager.PlaySound(laserSoundName);
            }
        }
    }
}
