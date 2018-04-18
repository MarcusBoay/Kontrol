using UnityEngine;

namespace Kontrol
{
    public class WeaponUnlocks : MonoBehaviour
    {
        public static int[] unlocks;

        void Start()
        {
            unlocks = new int[] { 1, 0, 0, 0, 0 };
        }

        public static void InitialUnlocks()
        {
            unlocks = new int[] { 1, 0, 0, 0, 0 };
        }
    }
}
