using System;
using TMPro;
using UnityEngine;

namespace Kontrol
{
    public class SpeedManager : MonoBehaviour
    {
        public static SpeedManager instance;
        private TextMeshProUGUI speedText;

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

        public void SetSpeedText(int speedSwitch)
        {
            speedText = GameObject.Find("Canvas").transform.Find("Speed Panel BG").transform.Find("Speed Panel").transform.Find("SpeedText").gameObject.GetComponent<TextMeshProUGUI>();
            speedText.text = Convert.ToString(speedSwitch + 1);
        }
    }
}
