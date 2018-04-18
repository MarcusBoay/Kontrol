using System.Collections;
using UnityEngine;

namespace Kontrol
{
    public class AudioDestroyer : MonoBehaviour
    {
        public float time;

        void Start()
        {
            StartCoroutine(Destroy());
        }

        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
