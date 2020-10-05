using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public GameObject soundNodePrefab;
        public List<AudioClip> birds;
        public AudioClip coins;
        public AudioClip placeRail;
        public AudioClip placeStation;
        public AudioClip remove;
        public AudioClip uiClick;


        [Header("For Background Music")]
        public bool muteMusic;
        public bool muteSounds;
        public bool muteAmbientSound;
        public AudioSource musicSource;
        public AudioClip ambientSound;
        public AudioClip musicStart;
        public AudioClip musicLoop;

        private AudioSource _ambientSoundAudioSource;

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

            if (ambientSound != null)
            {
                float audioClipLength = ambientSound.length;

                GameObject soundNode = Instantiate(soundNodePrefab);

                _ambientSoundAudioSource = soundNode.GetComponent<AudioSource>();

                _ambientSoundAudioSource.clip = ambientSound;
                _ambientSoundAudioSource.Play();
            }

            StartCoroutine(routine: PlayBirdSounds());



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

            if (_ambientSoundAudioSource != null)
            {
                _ambientSoundAudioSource.mute = muteAmbientSound;
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

        private IEnumerator PlayBirdSounds()
        {
            for(;;)
            {
                PlayRandomBirdSound();
                yield return new WaitForSeconds(seconds: Random.Range( min: 5f, max: 15f));
            }
        }

        private void PlayRandomBirdSound()
        {
            int index = Mathf.FloorToInt(f: Random.value * birds.Count);
            PlaySound(audioClip: birds[index: index]);
        }

        public void PlaySoundCoins()
        {
            PlaySound(audioClip: coins);
        }

        public void PlaySoundPlaceRail()
        {
            PlaySound(audioClip: placeRail);
        }

        public void PlaySoundPlaceStation()
        {
            PlaySound(audioClip: placeStation);
        }

        public void PlaySoundRemove()
        {
            PlaySound(audioClip: remove);
        }

        public void PlaySoundUiClick()
        {
            PlaySound(audioClip: uiClick);
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