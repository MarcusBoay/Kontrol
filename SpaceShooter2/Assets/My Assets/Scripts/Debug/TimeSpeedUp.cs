using UnityEngine;

public class TimeSpeedUp : MonoBehaviour
{
    public float speedScale;

    private bool isScaled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!isScaled)
            {
                isScaled = true;
                Time.timeScale = speedScale;
                Debug.Log("time has sped up");
            }
            else
            {
                isScaled = false;
                Time.timeScale = 1f;
                Debug.Log("toki wo tomare");
            }
        }
    }
}
