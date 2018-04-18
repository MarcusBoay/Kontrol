using System;
using TMPro;
using UnityEngine;

namespace Kontrol
{
    public class PlayerLives : MonoBehaviour
    {
        public static PlayerLives instance;

        private static int playerLives;
        public int getPlayerLives()
        {
            return playerLives;
        }
        public int maxPlayerLives;
        private GameObject[] playerLifeImages = new GameObject[3];
        private TextMeshProUGUI playerLifeText;

        private AudioManager audioManager;

        private void Awake()
        {
            //singleton
            if (instance != null)
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }

            audioManager = AudioManager.instance;
        }

        void Start()
        {
            playerLives = maxPlayerLives;
        }

        void Update()
        {
            SetPlayerLifeText();
        }

        private void LateUpdate()
        {
            if (audioManager == null)
            {
                audioManager = AudioManager.instance;
            }
        }

        private void SetPlayerLifeImages()
        {
            try
            {
                for (int i = 0; i < maxPlayerLives; i++)
                {
                    if (playerLifeImages[i] == null)
                    {
                        playerLifeImages[i] = GameObject.Find("Canvas").transform.Find("Life Panel BG").transform.Find("Life Panel").transform.Find("LifeImage" + i).gameObject;
                    }
                }
                for (int i = 0; i < playerLives; i++)
                {
                    playerLifeImages[i].gameObject.SetActive(true);
                }
                for (int i = 2; i >= playerLives; i--)
                {
                    playerLifeImages[i].gameObject.SetActive(false);
                }
            }
            catch
            {
                //no more images to set active
            }
        }

        private void SetPlayerLifeText()
        {
            try
            {
                if (playerLifeText == null)
                {
                    playerLifeText = GameObject.Find("Canvas").transform.Find("Life Panel BG").transform.Find("Life Panel").transform.Find("LifeText").gameObject.GetComponent<TextMeshProUGUI>();
                }

                if (getPlayerLives() < 0)
                {
                    playerLifeText.text = "0";
                }
                else
                {
                    playerLifeText.text = Convert.ToString(getPlayerLives());
                }
            }
            catch { }
        }

        public void RestartMaxLives()
        {
            playerLives = maxPlayerLives;
        }

        public void DecreasePlayerLives()
        {
            playerLives -= 1;
            GameOver();
        }

        public void GameOver()
        {
            if (playerLives < 0)
            {
                GameStateMachine.myGameState = GameStateMachine.GameState.GAMEOVER;
                audioManager.StopMusic();
                WeaponUnlocks.InitialUnlocks();
                GameObject.Find("UI").GetComponent<ShowPanels>().ShowGameOverPanel();
                Time.timeScale = 0;
                Pause.isGameOver = true;
            }
        }
    }
}
