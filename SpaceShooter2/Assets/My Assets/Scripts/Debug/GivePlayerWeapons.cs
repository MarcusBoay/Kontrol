using UnityEngine;

namespace Kontrol
{
    public class GivePlayerWeapons : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                for (int i = 0; i < WeaponUnlocks.unlocks.Length; i++)
                {
                    WeaponUnlocks.unlocks[i] = 2;
                }
                Debug.Log("WEAPON GET!");
            }
        }
    }
}
