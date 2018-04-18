using UnityEngine;

namespace Kontrol
{
    public class MoveShip : MonoBehaviour
    {
        public Vector2 moveSpeed;


        void FixedUpdate()
        {
            transform.position += new Vector3(moveSpeed.x, moveSpeed.y, 0);
        }
    }
}
