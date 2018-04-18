using UnityEngine;

namespace Kontrol
{
    public class ShipLaserShooter : MonoBehaviour
    {
        public GameObject LaserGO;

        public float shootLaserCooldown;
        public Vector2 shootDirection;
        public string laserSoundName;

        private float _nextShoot;
        private bool _shoot;
        private AudioManager audioManager;

        void Start()
        {
            _nextShoot = Time.time;
            _shoot = false;

            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!!!");
            }
        }

        void FixedUpdate()
        {
            if (!_shoot && Time.time > _nextShoot + shootLaserCooldown)
            {
                _shoot = true;
                _nextShoot = Time.time;
            }

            if (_shoot)
            {
                _shoot = false;
                ShootInDirection.Shoot(LaserGO, 1, 0, transform.position, shootDirection, LaserGO.GetComponent<LaserMovement>().moveSpeed, true);

                audioManager.PlaySound(laserSoundName);
            }
        }
    }
}
