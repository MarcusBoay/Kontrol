using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Kontrol
{
    public class PlayerStateMachine : MonoBehaviour
    {

        public GameObject playerPrefab;
        public GameObject dummyPlayer;
        private Rigidbody2D rb2d;
        private GameObject _player;
        private AudioManager audioManager;
        //public float invincibleTime;
        public float respawnTime;
        public float gameOverPauseBeforeTextAndMusic;
        public float gameOverWaitBeforeStartScreen;

        public string gameOverMusicName;
        public string respawnSoundName;

        public GameObject gameOverTextGO;

        public enum PlayerState
        {
            SPAWNING,
            ALIVE,
            DEAD,
            GAMEOVER
        }
        public PlayerState myPlayerState;

        void Start()
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
        }

        //messy implementation of state functions and coroutines but whatever
        void FixedUpdate()
        {
            switch (myPlayerState)
            {
                case (PlayerState.SPAWNING):
                    {
                        break;
                    }
                case (PlayerState.ALIVE):
                    {
                        break;
                    }
                case (PlayerState.DEAD):
                    {
                        StartCoroutine(Respawn());
                        myPlayerState = PlayerState.SPAWNING;
                        break;
                    }
                case (PlayerState.GAMEOVER):
                    {
                        break;
                    }
            }
        }

        //IEnumerator Invincibility(GameObject player)
        //{
        //    player.tag = "Respawn";
        //    player.GetComponent<SpriteRenderer>().color = new Color(1, 0.92f, 0.016f, 1);
        //    yield return new WaitForSeconds(invincibleTime);
        //    player.tag = "Player";
        //    player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        //}

        IEnumerator Respawn()
        {
            if (gameObject.GetComponent<PlayerLives>().getPlayerLives() < 0)
            {
                ////switch player & game state to gameover
                myPlayerState = PlayerState.GAMEOVER;
                //GameStateMachine.myGameState = GameStateMachine.GameState.GAMEOVER;
                ////send player back to title screen
                ////audioManager.StopMusic();
                ////pause before executing gameover text and music
                //yield return new WaitForSeconds(gameOverPauseBeforeTextAndMusic);
                ////play game over music
                //audioManager.PlayMusic(gameOverMusicName);
                //gameOverTextGO = GameObject.Find("Canvas").transform.Find("Game Over Text").gameObject;
                ////show game over text on screen
                //gameOverTextGO.SetActive(true);
                //yield return new WaitForSeconds(gameOverWaitBeforeStartScreen);
                ////hide game over text
                //gameOverTextGO.SetActive(false);
                ////change game state to start
                //GameStateMachine.myGameState = GameStateMachine.GameState.STARTSCREEN;
                ////go back to start screen
                //SceneManager.LoadScene(0);
                ////destroy this GO
                //Destroy(this.gameObject);


            }
            else
            {
                yield return new WaitForSeconds(respawnTime);
                //set player state to spawning
                myPlayerState = PlayerState.SPAWNING;
                //spawn dummy player animator
                GameObject _dummyPlayer = Instantiate(dummyPlayer, Camera.main.transform.position, Quaternion.Euler(0, 0, 0));
                _dummyPlayer.transform.parent = Camera.main.transform;
                //dummy player will spawn player and change player state
                //play respawn sound
                audioManager.PlaySound(respawnSoundName);
            }
        }
    }
}
