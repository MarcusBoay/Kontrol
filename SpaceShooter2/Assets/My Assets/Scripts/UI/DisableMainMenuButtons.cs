using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableMainMenuButtons : MonoBehaviour {
    public Button[] Buttons;

    public void DisableButtons()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
    }
}
