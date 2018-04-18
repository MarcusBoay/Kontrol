using UnityEngine;

namespace Kontrol
{
    public class ShipLaserShooterSpawner : MonoBehaviour
    {
        public GameObject ShipLaserShooterGO;

        public float spawnCooldown;

        private float _nextSpawn;
        private bool _isSpawned;
        private bool _checkChild;

        void Start()
        {
            _isSpawned = false;
        }

        void Update()
        {
            //checks if there is no Ship Laser Shooter as child of this GO
            if (_checkChild && transform.childCount == 0)
            {
                _nextSpawn = Time.time;
                _isSpawned = false;
                _checkChild = false;
            }

            //spawns ship laser shooter
            if (!_isSpawned && Time.time > _nextSpawn + spawnCooldown)
            {
                _checkChild = true;
                _isSpawned = true;
                GameObject _shooter = Instantiate(ShipLaserShooterGO, transform.position, transform.rotation);
                _shooter.transform.SetParent(this.transform);
            }
        }
    }
}
