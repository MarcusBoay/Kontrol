using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kontrol
{
    public class ChangeScene : MonoBehaviour
    {
        public float timeBeforeChangeScene;
        public int sceneIndex;
        public GameObject fader;
        public bool isGoingToEndScene;

        private float fadeOutDuration;



        void Start()
        {
            if (isGoingToEndScene)
            {
                StartCoroutine(_ChangeToEndScene(sceneIndex));
            }
            else
            {
                StartCoroutine(_ChangeScene(sceneIndex));
            }
            fader = GameObject.Find("Canvas").transform.Find("Fade").gameObject;
            fadeOutDuration = fader.GetComponent<ScreenFade>().fadeOutDuration;
        }

        IEnumerator _ChangeScene(int _sceneIndex)
        {
            yield return new WaitForSeconds(timeBeforeChangeScene);
            fader.GetComponent<ScreenFade>().FadeOut();
            yield return new WaitForSeconds(fadeOutDuration + 0.1f);

            SceneManager.LoadScene(_sceneIndex);
            //spawn player
            GameObject.Find("PlayerManager").GetComponent<PlayerStateMachine>().myPlayerState = PlayerStateMachine.PlayerState.DEAD;
        }

        IEnumerator _ChangeToEndScene(int _sceneIndex)
        {
            yield return new WaitForSeconds(timeBeforeChangeScene);
            fader.GetComponent<ScreenFade>().FadeOut();
            yield return new WaitForSeconds(fadeOutDuration + 0.1f);

            SceneManager.LoadScene(_sceneIndex);
            //deactivate some game stuff
            Destroy(GameObject.Find("PlayerManager").gameObject);
            Destroy(GameObject.Find("WeaponUIManager").gameObject);
        }
    }
}
