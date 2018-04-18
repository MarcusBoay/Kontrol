using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class EnemyAI4BulletMovement : MonoBehaviour
    {
        public float shootSpeed;
        public float spawnRotation;
        public string bulletSoundName;
        public GameObject bullet;

        private AudioManager audioManager;

        void Start()
        {
            //initialization
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
            //start spawning laser
            StartCoroutine(SpawnBullets());
        }

        IEnumerator SpawnBullets()
        {
            while (true)
            {
                //spawn laser
                GameObject mBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
                //change laser rotation
                mBullet.transform.eulerAngles += new Vector3(0, 0, 270);
                //play sound
                audioManager.PlaySound(bulletSoundName);
                //shoot interval
                yield return new WaitForSeconds(shootSpeed);
            }
        }
    }
}
