using UnityEngine;

namespace Kontrol
{
    public class TimedDespawn : MonoBehaviour
    {
        public float despawnTime;

        private float _nextTime;

        void Start()
        {
            _nextTime = Time.time;
        }

        void LateUpdate()
        {
            if (Time.time >= _nextTime + despawnTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
