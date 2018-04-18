using UnityEngine;

namespace Kontrol
{
    public class ChangeSceneNumber : MonoBehaviour
    {
        public int thisScene;

        void Start()
        {
            RestartScene.currentScene = thisScene;
        }
    }
}