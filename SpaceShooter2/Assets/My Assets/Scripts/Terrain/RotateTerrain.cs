using UnityEngine;

namespace Kontrol
{
    public class RotateTerrain : MonoBehaviour
    {
        public float rotateSpeed;

        void FixedUpdate()
        {
            transform.eulerAngles += new Vector3(0, 0, rotateSpeed);
        }
    }
}
