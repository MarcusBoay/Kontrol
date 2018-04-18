using UnityEngine;

namespace Kontrol
{
    public class EnemyAI4 : MonoBehaviour
    {
        public float rotateSpeed;

        void FixedUpdate()
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed));
        }
    }
}
