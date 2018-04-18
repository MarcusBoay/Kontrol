using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kontrol
{
    public class RestartLevel : MonoBehaviour
    {
        private AudioManager audioManager;

        private void Start()
        {
            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager: There is no AudioManager in the scene!!!");
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                audioManager.StopMusic();
                SceneManager.LoadScene("RicochetLevel");
                audioManager.PlayMusic("Prototype Music");
            }
        }
    }
}