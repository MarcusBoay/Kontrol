using System.Collections;
using UnityEngine;

namespace Kontrol
{
    [System.Serializable]
    public abstract class Audio
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 0.7f;

        [System.NonSerialized]
        public AudioSource source;

        public virtual void SetSource(AudioSource _source)
        {
            source = _source;
            source.clip = clip;
            source.volume = volume;
        }

        public virtual void Play()
        {
            source.Play();
        }

        public virtual void Stop()
        {
            source.Stop();
        }
    }

    [System.Serializable]
    public class Sound : Audio
    {
        [Range(0.5f, 1.5f)]
        public float pitch = 1f;

        [Range(0f, 0.5f)]
        public float randomVolume = 0.1f;
        [Range(0f, 0.5f)]
        public float randomPitch = 0.1f;

        public override void SetSource(AudioSource _source)
        {
            base.SetSource(_source);
            source.pitch = pitch;
        }

        public override void Play()
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
            source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
            source.Play();
        }
    }

    [System.Serializable]
    public class Music : Audio
    {
        public bool loop = true;

        public override void SetSource(AudioSource _source)
        {
            base.SetSource(_source);
            source.loop = loop;
        }

        public void Pause()
        {
            source.Pause();
        }

        public void UnPause()
        {
            source.UnPause();
        }
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField]
        Sound[] sounds;
        [SerializeField]
        Music[] music;

        public AudioSource currentMusic;

        public float decreaseDuration = 3f;
        public float waitDuration = 2f;

        private void Awake()
        {
            if (instance != null)
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            //initialization for sounds
            for (int i = 0; i < sounds.Length; i++)
            {
                GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
                _go.transform.SetParent(this.transform);
                sounds[i].SetSource(_go.AddComponent<AudioSource>());
                _go.GetComponent<AudioSource>().playOnAwake = false;
            }
            //initialization for music
            for (int i = 0; i < music.Length; i++)
            {
                GameObject _go = new GameObject("Music_" + i + "_" + music[i].name);
                _go.transform.SetParent(this.transform);
                music[i].SetSource(_go.AddComponent<AudioSource>());
                _go.GetComponent<AudioSource>().playOnAwake = false;
            }
        }

        //public sound fx functions
        public void PlaySound(string _name)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].name == _name)
                {
                    sounds[i].Play();
                    return;
                }
            }

            //no sound with _name
            Debug.LogWarning("AudioManager: Sound not found in list: " + _name);
        }

        public void StopSound(string _name)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].name == _name)
                {
                    sounds[i].Stop();
                    return;
                }
            }

            //no sound with _name
            Debug.LogWarning("AudioManager: Sound not found in list: " + _name);
        }

        //public music functions
        public void PlayMusic(string _name)
        {
            //find music and play
            for (int i = 0; i < music.Length; i++)
            {
                if (music[i].name == _name)
                {
                    currentMusic = music[i].source;
                    currentMusic.Play();
                    return;
                }
            }

            //no sound with _name
            Debug.LogWarning("AudioManager: Music not found in list: " + _name);
        }

        public void StopMusic()
        {
            try
            {
                currentMusic.Stop();
            }
            catch
            {
                //no music to stop
            }
        }

        public void PauseMusic()
        {
            currentMusic.Pause();
        }

        public void UnPauseMusic()
        {
            currentMusic.UnPause();
        }

        public void SwitchMusic(string _name)
        {
            StartCoroutine(_SwitchMusic(_name));
        }

        public void BossMusic(string _intro, string _loop)
        {
            StartCoroutine(_BossMusic(_intro, _loop));
        }

        public IEnumerator _SwitchMusic(string _name)
        {
            //getting initial volume of music
            float initialVolume = currentMusic.volume;
            //decrease volume interval
            float decreaseVolumeInterval = 0.1f;

            //decreases current music volume every interval
            while (currentMusic.volume != 0)
            {
                try
                {
                    currentMusic.volume -= initialVolume / decreaseDuration * decreaseVolumeInterval;
                }
                catch
                {
                    currentMusic.volume = 0;
                }
                yield return new WaitForSeconds(decreaseVolumeInterval);
            }

            currentMusic.Stop();
            currentMusic.volume = initialVolume;

            //wait for a while
            yield return new WaitForSeconds(waitDuration);

            //play new music
            for (int i = 0; i < music.Length; i++)
            {
                if (music[i].name == _name)
                {
                    currentMusic = music[i].source;
                    currentMusic.volume = music[i].volume;
                    currentMusic.Play();
                    yield break;
                }
            }

            //no sound with _name
            Debug.LogWarning("AudioManager: Music not found in list: " + _name);
        }

        public IEnumerator _BossMusic(string _intro, string _loop)
        {
            //getting initial volume of music
            float initialVolume = currentMusic.volume;
            //decrease volume interval
            float decreaseVolumeInterval = 0.1f;

            //decreases current music volume every interval
            while (currentMusic.volume != 0)
            {
                try
                {
                    currentMusic.volume -= initialVolume / decreaseDuration * decreaseVolumeInterval;
                }
                catch
                {
                    currentMusic.volume = 0;
                }
                yield return new WaitForSeconds(decreaseVolumeInterval);
            }

            currentMusic.Stop();
            currentMusic.volume = initialVolume;

            //wait for a while
            yield return new WaitForSeconds(waitDuration);

            //play new music
            for (int i = 0; i < music.Length; i++)
            {
                if (music[i].name == _intro)
                {
                    currentMusic = music[i].source;
                    currentMusic.volume = music[i].volume;
                    currentMusic.Play();
                    break;
                }
            }

            //no sound with _name
            Debug.LogWarning("AudioManager: Music not found in list: " + _intro);

            while (currentMusic.isPlaying)
            {
                //wait until intro boss clip is finish
                yield return null;
            }

            //play new music
            for (int i = 0; i < music.Length; i++)
            {
                if (music[i].name == _loop)
                {
                    currentMusic = music[i].source;
                    currentMusic.volume = music[i].volume;
                    currentMusic.Play();
                    break;
                }
            }

            //no sound with _name
            Debug.LogWarning("AudioManager: Music not found in list: " + _loop);
        }
    }

}