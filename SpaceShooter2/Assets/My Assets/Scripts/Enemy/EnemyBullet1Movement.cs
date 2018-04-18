using UnityEngine;

namespace Kontrol
{
    public class EnemyBullet1Movement : MonoBehaviour
    {

        private void FixedUpdate()
        {
            transform.position += new Vector3(Camera.main.GetComponent<CameraMovement>().moveSpeed.x, Camera.main.GetComponent<CameraMovement>().moveSpeed.y, 0);
        }
    }
}
