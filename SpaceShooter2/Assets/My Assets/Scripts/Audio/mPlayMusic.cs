using UnityEngine;

namespace Kontrol
{
    public class mPlayMusic : MonoBehaviour
    {
        public string musicToPlay;

        private AudioManager audioManager;

        void Start()
        {
            audioManager = AudioManager.instance;
        }

        private void LateUpdate()
        {
            audioManager.PlayMusic(musicToPlay);
            Destroy(this.gameObject);
        }
    }
}