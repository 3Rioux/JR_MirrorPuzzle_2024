using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] _musicSound, _sfxSound, _ambientSound;
        [SerializeField] private AudioSource _musicSource, _sfxSource, _ambientSource;

        //private bool mute = false;

        public static AudioManager audioManager_Instance;

        private void Awake()
        {
            if (audioManager_Instance == null)
            {
                audioManager_Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            audioManager_Instance.PlayMusic(currentScene.buildIndex == 0 ? "MainMenuMusic" : "GameTheme");
        }

        public void PlaySFX(string sfxName)
        {
            Sound sound = Array.Find(_sfxSound, x => x.name == sfxName);

            if (sound == null)
                Debug.Log("Sound Not Found");
            else
                _sfxSource.PlayOneShot(sound.clip);
        }

        public void PlayMusic(string musicName)
        {
            Sound sound = Array.Find(_musicSound, x => x.name == musicName);

            if (sound == null)
            {
                Debug.Log("Sound Not Found");
            }
            else
            {
                _musicSource.clip = sound.clip;
                _musicSource.Play();
            }
        }


        public void PlayAmbient(string ambientName)
        {
            Sound sound = Array.Find(_ambientSound, x => x.name == ambientName);

            if (sound == null)
            {
                Debug.Log("Sound Not Found");
            }
            else
            {
                _ambientSource.clip = sound.clip;
                _ambientSource.Play();
            }
        }
        public void Muted(bool mute)
        {
            mute = !mute;
            if (mute)
            {
                gameObject.GetComponent<AudioSource>().volume = 0;
            }
            else
            {
                gameObject.GetComponent<AudioSource>().volume = 1;
            }
        }

        public void MusicVolume(float volume)
        {
            _musicSource.volume = volume;
        }

        public void SFXVolume(float volume)
        {
            _sfxSource.volume = volume;
        }

        public void AmbientVolume(float volume)
        {
            _ambientSource.volume = volume;
        }
    }
}