using UnityEngine;

namespace Kontrol
{
    public class CameraAccelerator : MonoBehaviour
    {
        public Vector2 distanceFromCameraToChangeSpeed;
        public bool checkX;
        public bool checkY;

        public Vector2 accel;
        public Vector2 finalSpeed;

        private bool isChangingSpeedX = false;
        private bool isChangingSpeedY = false;

        void FixedUpdate()
        {
            if (!isChangingSpeedX && checkX && !checkY && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToChangeSpeed.x && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToChangeSpeed.x)
            {
                isChangingSpeedX = true;
            }
            else if (!isChangingSpeedY && !checkX && checkY && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToChangeSpeed.y && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToChangeSpeed.y)
            {
                isChangingSpeedY = true;
            }
            else if (!isChangingSpeedX && !isChangingSpeedY && checkX && checkY && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToChangeSpeed.x && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToChangeSpeed.x && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToChangeSpeed.y && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToChangeSpeed.y)
            {
                isChangingSpeedX = true;
                isChangingSpeedY = true;
            }

            if (isChangingSpeedX && Camera.main.GetComponent<CameraMovement>().moveSpeed.x < finalSpeed.x)
            {
                Camera.main.GetComponent<CameraMovement>().moveSpeed.x += accel.x;
                if (Camera.main.GetComponent<CameraMovement>().moveSpeed.x > finalSpeed.x)
                {
                    Camera.main.GetComponent<CameraMovement>().moveSpeed.x = finalSpeed.x;
                }
            }
            if (isChangingSpeedY && Camera.main.GetComponent<CameraMovement>().moveSpeed.y < finalSpeed.y)
            {
                Camera.main.GetComponent<CameraMovement>().moveSpeed.y += accel.y;
                if (Camera.main.GetComponent<CameraMovement>().moveSpeed.y > finalSpeed.y)
                {
                    Camera.main.GetComponent<CameraMovement>().moveSpeed.y = finalSpeed.y;
                }
            }
        }
    }

}