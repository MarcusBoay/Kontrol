using UnityEngine;

namespace Kontrol
{
    public class TimedSpawn : MonoBehaviour
    {
        public GameObject ObjectToSpawnGO;

        public float durationBeforeNextSpawn;
        public string spawnSoundName;
        public bool isChild = true;

        private float _nextTime;
        private AudioManager audioManager;

        void Start()
        {
            _nextTime = Time.time;

            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!!!");
            }
        }

        void FixedUpdate()
        {
            //spawn object
            if (Time.time >= _nextTime + durationBeforeNextSpawn)
            {
                _nextTime = Time.time;
                GameObject _gameObject = Instantiate(ObjectToSpawnGO, transform.position, transform.rotation);
                if (isChild)
                {
                    _gameObject.transform.parent = transform;
                }

                audioManager.PlaySound(spawnSoundName);
            }
        }
    }
}