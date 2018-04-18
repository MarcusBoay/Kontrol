using UnityEngine;

namespace Kontrol
{
    public class Destroyer : MonoBehaviour
    {
        void DestroyGameObject()
        {
            Destroy(this.gameObject);
        }
    }
}
