using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class SpawnPlayer : MonoBehaviour
    {
        public GameObject player;
        public bool spawnUnderCamera;
        private WeaponManager weaponManager;


        private void LateUpdate()
        {
            if (weaponManager == null)
            {
                weaponManager = WeaponManager.instance;
                Assert.IsNotNull(weaponManager);
            }
        }

        public void SpawnThePlayer()
        {
            GameObject _player = Instantiate(player, transform.position, Quaternion.Euler(0, 0, 0));
            _player.name = "Player";
            GameObject.Find("PlayerManager").GetComponent<PlayerStateMachine>().myPlayerState = PlayerStateMachine.PlayerState.ALIVE;
            if (spawnUnderCamera)
            {
                _player.transform.parent = Camera.main.transform;
            }
        }

        public void InitializePlayerWeapon()
        {
            weaponManager.InitializePlayerWeapon();
        }
    }
}
