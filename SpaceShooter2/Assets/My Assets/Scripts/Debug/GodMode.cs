using UnityEngine;

namespace Kontrol
{
    public class GodMode : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (GameObject.Find("Player").gameObject.transform.tag == "Player")
                {
                    GameObject.Find("Player").gameObject.transform.tag = "Enemy";
                    Debug.Log("that player is a spy!");
                }
                else
                {
                    GameObject.Find("Player").gameObject.transform.tag = "Player";
                    Debug.Log("that player is a player");
                }
            }
        }
    }

}