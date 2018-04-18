using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kontrol
{
    public class ScreenFade : MonoBehaviour
    {
        public Image blackScreen;
        public float fadeOutDuration;
        public float fadeInDuration;

        private bool isFadingOut;
        private bool isFadingIn;
        private float fadeValue;

        private void Start()
        {
            if (blackScreen == null)
            {
                blackScreen = GetComponent<Image>();
            }
            isFadingOut = false;
            FadeIn();
        }

        void Update()
        {
            if (isFadingOut)
            {
                blackScreen.color = new Color(0, 0, 0, fadeValue);
                fadeValue += Time.deltaTime / fadeOutDuration;
                if (blackScreen.color.a >= 1)
                {
                    blackScreen.color = new Color(0, 0, 0, 1);
                    isFadingOut = false;
                }
            }
            else if (isFadingIn)
            {
                blackScreen.color = new Color(0, 0, 0, fadeValue);
                fadeValue -= Time.deltaTime / fadeInDuration;
                if (blackScreen.color.a <= 0)
                {
                    blackScreen.color = new Color(0, 0, 0, 0);
                    isFadingIn = false;
                }
            }
        }

        public void FadeOut()
        {
            isFadingOut = true;
            fadeValue = 0;
        }

        public void FadeIn()
        {
            isFadingIn = true;
            fadeValue = 1;
        }
    }
}
