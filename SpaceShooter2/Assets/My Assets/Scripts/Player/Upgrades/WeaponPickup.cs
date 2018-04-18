using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class WeaponPickup : MonoBehaviour
    {
        private AudioManager audioManager;

        public enum _weaponType
        {
            NORMAL,
            RICOCHET,
            PENETRATE
        }
        public _weaponType weaponType;

        private void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" || other.tag == "Respawn")
            {
                if (weaponType == _weaponType.NORMAL && WeaponUnlocks.unlocks[0] < 2)
                {
                    WeaponUnlocks.unlocks[0]++;
                    audioManager.PlaySound("WeaponPickup");
                }
                if (weaponType == _weaponType.RICOCHET && WeaponUnlocks.unlocks[1] < 2)
                {
                    WeaponUnlocks.unlocks[1]++;
                    audioManager.PlaySound("WeaponPickup");
                }
                if (weaponType == _weaponType.PENETRATE && WeaponUnlocks.unlocks[2] < 2)
                {
                    WeaponUnlocks.unlocks[2]++;
                    audioManager.PlaySound("WeaponPickup");
                }
                Destroy(gameObject);
            }
        }
    }
}
