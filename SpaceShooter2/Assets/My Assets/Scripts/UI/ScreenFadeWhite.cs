using UnityEngine;
using UnityEngine.UI;

namespace Kontrol
{
    public class ScreenFadeWhite : MonoBehaviour
    {
        public Image whiteScreen;

        private bool isFlash;
        private float flashValue;

        private void Start()
        {
            if (whiteScreen == null)
            {
                whiteScreen = GetComponent<Image>();
            }
        }

        void Update()
        {
            if (isFlash)
            {
                whiteScreen.color = new Color(1, 1, 1, flashValue);
                flashValue -= Time.deltaTime / 3;
                if (whiteScreen.color.a <= 0)
                {
                    whiteScreen.color = new Color(1, 1, 1, 0);
                    isFlash = false;
                }
            }
        }

        public void FadeOutWhite()
        {
            isFlash = true;
            flashValue = 1;
        }
    }
}
