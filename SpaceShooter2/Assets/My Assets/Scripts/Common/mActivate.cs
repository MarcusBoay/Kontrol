using UnityEngine;

namespace Kontrol
{
    public class mActivate : MonoBehaviour
    {
        public GameObject GOToActivate;

        public bool stateToActivate;
        public float distanceFromCameraToSpawn;
        public bool checkX;
        public bool checkY;

        private bool isActivated;

        private void FixedUpdate()
        {
            if (checkX && !checkY && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToSpawn && !isActivated && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToSpawn)
            {
                isActivated = true;
                ActivateGameObject();
                Destroy(gameObject);
            }
            else if (checkY && !checkX && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToSpawn && !isActivated && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToSpawn)
            {
                isActivated = true;
                ActivateGameObject();
                Destroy(gameObject);
            }
            else if (checkX && checkY && transform.position.y - Camera.main.transform.position.y < distanceFromCameraToSpawn && !isActivated && transform.position.y - Camera.main.transform.position.y > -distanceFromCameraToSpawn && transform.position.x - Camera.main.transform.position.x < distanceFromCameraToSpawn && !isActivated && transform.position.x - Camera.main.transform.position.x > -distanceFromCameraToSpawn)
            {
                isActivated = true;
                ActivateGameObject();
                Destroy(gameObject);
            }
        }

        private void ActivateGameObject()
        {
            if (GOToActivate != null)
            {
                GOToActivate.SetActive(stateToActivate);
            }
        }
    }
}