using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kontrol
{
    public class RestartScene : MonoBehaviour
    {
        public static int currentScene;
        public int mainMenuScene;

        public void RestartThisScene()
        {
            GameObject.Find("GameManager (bring to all scenes)").GetComponent<ScoreManager>().scoreLevel[currentScene - 1] = 0;
            SceneManager.LoadScene(currentScene);
            Time.timeScale = 1;
            Pause.isGameOver = false;
            GameObject.Find("UI").GetComponent<ShowPanels>().HideGameOverPanel();
            GameObject.Find("UI").GetComponent<ShowPanels>().HidePausePanel();
            GameObject.Find("PlayerManager").GetComponent<PlayerLives>().RestartMaxLives();
            GameObject.Find("PlayerManager").GetComponent<PlayerStateMachine>().myPlayerState = PlayerStateMachine.PlayerState.DEAD;
            GameObject.Find("WeaponUIManager").GetComponent<WeaponManager>().TakeAwayWeapons();
            GameObject.Find("WeaponUIManager").GetComponent<WeaponManager>().TakeAwayWeapons();
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
            Time.timeScale = 1;
            Pause.isGameOver = false;
            GameObject.Find("UI").GetComponent<ShowPanels>().HideGameOverPanel();
            Destroy(GameObject.Find("GameManager (bring to all scenes)").gameObject);
            try
            {
                Destroy(GameObject.Find("PlayerManager").gameObject);
            }
            catch { }
            Destroy(GameObject.Find("UI").gameObject);
        }
    }
}
