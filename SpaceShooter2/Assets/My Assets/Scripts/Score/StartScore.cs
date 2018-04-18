using UnityEngine;

namespace Kontrol
{
    public class StartScore : MonoBehaviour
    {
        private void LateUpdate()
        {
            gameObject.GetComponent<EnemyScore>().AddScore();
            Destroy(gameObject);
        }
    }
}
