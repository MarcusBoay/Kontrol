using UnityEngine;

namespace Kontrol
{
    public class ChangeMusic : MonoBehaviour
    {

        public float distanceFromCameraToChangeMusic;
        public bool checkX;
        public bool checkY;

        public string introMusic;
        public string loopMusic;

        private AudioManager audioManager;
        private bool spawned;

        private void Start()
        {
            audioManager = AudioManager.instance;
        }

        private void FixedUpdate()
        {
            if (checkX && !checkY && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToChangeMusic && !spawned && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToChangeMusic)
            {
                spawned = true;
                audioManager.BossMusic(introMusic, loopMusic);
                //destroy this spawner once it has switched music
                Destroy(gameObject);
            }
            else if (checkY && !checkX && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToChangeMusic && !spawned && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToChangeMusic)
            {
                spawned = true;
                audioManager.BossMusic(introMusic, loopMusic);
                //destroy this spawner once it has switched music
                Destroy(gameObject);
            }
            else if (checkX && checkY && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToChangeMusic && !spawned && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToChangeMusic && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToChangeMusic && !spawned && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToChangeMusic)
            {
                spawned = true;
                audioManager.BossMusic(introMusic, loopMusic);
                //destroy this spawner once it has switched music
                Destroy(gameObject);
            }
        }
    }

}