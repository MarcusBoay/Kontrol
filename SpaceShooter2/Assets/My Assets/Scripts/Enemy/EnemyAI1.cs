using UnityEngine;

namespace Kontrol
{
    public class EnemyAI1 : MonoBehaviour
    {
        public float xSpeed;
        public float phaseAngle;
        private float yPos;
        public float yAmplitude;

        void Start()
        {
            //translation
            yPos = transform.position.y;
        }

        void FixedUpdate()
        {
            transform.position = new Vector3(transform.position.x + xSpeed, yPos + Mathf.Sin(transform.position.x + phaseAngle) * yAmplitude, transform.position.z);
        }
    }
}
