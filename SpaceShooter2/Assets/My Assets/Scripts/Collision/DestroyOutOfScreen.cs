using UnityEngine;

namespace Kontrol
{
    public class DestroyOutOfScreen : MonoBehaviour
    {
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Background" || other.gameObject.tag == "EnemyTerrain" || other.gameObject.tag == "DestructibleTerrain" || other.gameObject.tag == "InvulEnemy")
            {
                //do nothing
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

}