using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private bool _isGamePaused;
        [SerializeField] private GameObject _pauseMenuUI,_settingsMenuUI, _PlayerInfoUI;

        //public void PlayButton()
        //{
        //    Time.timeScale = 1f;

        //    HideMouseCursor();

        //    SceneManager.LoadSceneAsync(1);

            
        //    AudioManager.Instance.PlayAmbient("WindAmbient");
        //    AudioManager.Instance.PlayMusic("GameTheme");

        //    Debug.Log("Loading Game Level");
        //}

        public void BackToMainMenuButton()
        {
            Time.timeScale = 1f;

            ShowMouseCursor();

            SceneManager.LoadSceneAsync(0);

            
            Destroy(AudioManager.Instance.gameObject);
            AudioManager.Instance.PlayMusic("MainMenuMusic");

            Debug.Log("Going Back to Main Menu");
        }

        public void ExitButton()
        {
            Application.Quit();

            Debug.Log("Quitting the game");
        }

        public void ResumeButton()
        {
            HideMouseCursor();
            _pauseMenuUI.SetActive(false);
            _settingsMenuUI.SetActive(false);
            _PlayerInfoUI.SetActive(true);
            Time.timeScale = 1f;
            _isGamePaused = false;
        }

        private void PauseButton()
        {
            ShowMouseCursor();
            _pauseMenuUI.SetActive(true);
            _settingsMenuUI.SetActive(false);
            Time.timeScale = 0f;
            _isGamePaused = true;
        }

        private void Update()
        {
            //if (Keyboard.current.escapeKey.wasPressedThisFrame && SceneManager.GetActiveScene().buildIndex > 1) // make sure the game scene is not main or ending 
            //{
            //    if (_isGamePaused)
            //    {
            //        ResumeButton();
            //    }
            //    else
            //    {
            //        PauseButton();
            //    }
            //}
        }

        private static void ShowMouseCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private static void HideMouseCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}