using UnityEngine;

namespace Manager
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public GameObject soundNodePrefab;
        public AudioClip testSound;

        [Header("For Background Music")] public bool muteMusic;
        public bool muteSounds;
        public AudioSource musicSource;
        public AudioClip musicStart;
        public AudioClip musicLoop;

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        public void Start()
        {
            if (musicStart != null)
            {
                musicSource.PlayOneShot(musicStart);
            }

            if (musicLoop != null)
            {
                musicSource.clip = musicLoop;
                musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);
                musicSource.loop = true;
            }

            //if (!MuteMusic)
            //{
            //    musicSource.Play();
            //}
            //musicSource.loop = true;
        }

        public void Update()
        {
            if (muteMusic)
            {
                musicSource.mute = true;
            }
            else
            {
                musicSource.mute = false;
            }

            //if (!MuteMusic && !musicSource.isPlaying)
            //{
            //    musicSource.Play();
            //}
            //else if (MuteMusic)
            //{
            //    musicSource.Stop();
            //    musicSource.mute = true;
            //}
        }

        public void PlayTestSound()
        {
            PlaySound(testSound);
        }

        private void PlaySound(AudioClip audioClip)
        {
            if (muteSounds)
            {
                return;
            }

            float audioClipLength = audioClip.length;

            GameObject soundNode = Instantiate(soundNodePrefab);

            AudioSource audioSource = soundNode.GetComponent<AudioSource>();

            audioSource.clip = audioClip;
            audioSource.Play();

            Destroy(soundNode, audioClipLength);
        }

        public void MuteMusic(bool vBool)
        {
            muteMusic = vBool;
        }

        public void MuteSounds(bool vBool)
        {
            muteSounds = vBool;
        }
    }
}