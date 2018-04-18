using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class PlayerCollide : MonoBehaviour
    {
        public GameObject explosion;
        private AudioManager audioManager;
        private PlayerLives playerLives;
        private PlayerStateMachine playerStateMachine;
        private WeaponManager weaponManager;

        void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            playerLives = PlayerLives.instance;
            Assert.IsNotNull(playerLives);
            playerStateMachine = GameObject.Find("PlayerManager").gameObject.GetComponent<PlayerStateMachine>();
            Assert.IsNotNull(playerStateMachine);
            weaponManager = GameObject.Find("WeaponUIManager").gameObject.GetComponent<WeaponManager>();
            Assert.IsNotNull(weaponManager);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckPlayerTrigger(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            CheckPlayerTrigger(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            CheckPlayerCollision(other);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            CheckPlayerCollision(other);
        }

        private void CheckPlayerTrigger(Collider2D other)
        {
            string _tag = other.tag;
            if (_tag == "EnemyBullet" || _tag == "Enemy" || _tag == "EnemyRicochetBullet" || _tag == "EnemyTerrain" || _tag == "InvulEnemy")
            {
                if (this.tag == "Player")
                {
                    playerLives.DecreasePlayerLives();
                    playerStateMachine.myPlayerState = PlayerStateMachine.PlayerState.DEAD;
                    Destroy(this.gameObject);
                    //take away player weapons
                    weaponManager = GameObject.Find("WeaponUIManager").gameObject.GetComponent<WeaponManager>();
                    weaponManager.TakeAwayWeapons();
                    //explosion sound
                    audioManager.PlaySound("PlayerExplode");
                    //explosion animation
                    Instantiate(explosion, transform.position, transform.rotation);
                }
            }
        }

        private void CheckPlayerCollision(Collision2D other)
        {
            string _tag = other.gameObject.tag;
            if (_tag == "EnemyBullet" || _tag == "Enemy" || _tag == "EnemyRicochetBullet" || _tag == "EnemyTerrain" || _tag == "InvulEnemy")
            {
                if (this.tag == "Player")
                {
                    playerLives.DecreasePlayerLives();
                    playerStateMachine.myPlayerState = PlayerStateMachine.PlayerState.DEAD;
                    Destroy(this.gameObject);
                    //take away player weapons
                    weaponManager = GameObject.Find("WeaponUIManager").gameObject.GetComponent<WeaponManager>();
                    weaponManager.TakeAwayWeapons();
                    //explosion sound
                    audioManager.PlaySound("PlayerExplode");
                    //explosion animation
                    Instantiate(explosion, transform.position, transform.rotation);
                }
            }
        }
    }
}
