using UnityEngine;

namespace Kontrol
{
    public class GameStateMachine : MonoBehaviour
    {
        public static GameStateMachine instance;

        public enum GameState
        {
            STARTSCREEN,
            PLAYING,
            GAMEOVER,
            WIN
        }
        public static GameState myGameState;

        private void Awake()
        {
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
        }

        void Start()
        {
            myGameState = GameState.STARTSCREEN;
        }
    }

}