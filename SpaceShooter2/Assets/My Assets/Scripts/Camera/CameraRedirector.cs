using UnityEngine;

namespace Kontrol
{
    public class CameraRedirector : MonoBehaviour
    {
        public float distanceFromCameraToRedirect;
        public Vector2 moveSpeed;

        public bool checkX;
        public bool checkY;

        private bool redirected;

        private void FixedUpdate()
        {
            if (checkX && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToRedirect && !redirected && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToRedirect)
            {
                redirected = true;
                Camera.main.GetComponent<CameraMovement>().moveSpeed = moveSpeed;
                //destroy this spawner once it has spawned the enemy
                Destroy(gameObject);
            }
            else if (checkY && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToRedirect && !redirected && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToRedirect)
            {
                redirected = true;
                Camera.main.GetComponent<CameraMovement>().moveSpeed = moveSpeed;
                //destroy this spawner once it has spawned the enemy
                Destroy(gameObject);
            }
        }
    }

}