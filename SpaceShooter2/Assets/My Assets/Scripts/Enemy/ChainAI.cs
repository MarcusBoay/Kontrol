using UnityEngine;

namespace Kontrol
{
    public class ChainAI : MonoBehaviour
    {
        public float moveSpeed;
        public float moveDuration;

        private float startTime;

        private void Start()
        {
            startTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (startTime + moveDuration > Time.time)
            {
                transform.Translate(0, moveSpeed, 0);
            }
        }
    }
}
