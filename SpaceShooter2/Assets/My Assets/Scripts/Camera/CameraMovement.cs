using UnityEngine;

namespace Kontrol
{
    public class CameraMovement : MonoBehaviour
    {
        public Vector2 startPosition;
        public Vector2 moveSpeed;

        public bool isStartingAtStartPosition;
        public bool isMoving;

        void Start()
        {
            if (isStartingAtStartPosition)
                transform.position = new Vector3(startPosition.x, startPosition.y, transform.position.z);
        }

        void FixedUpdate()
        {
            if (isMoving)
            {
                transform.Translate(moveSpeed);
            }
        }
    }

}