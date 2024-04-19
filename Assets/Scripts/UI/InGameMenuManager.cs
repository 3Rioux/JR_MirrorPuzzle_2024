using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class InGameMenuManager : MonoBehaviour
    {
        [SerializeField] private bool _isGamePaused;
        [SerializeField] private GameObject _pauseMenuUI;

        public void Resume()
        {
            _pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            _isGamePaused = false;
        }

        private void Pause()
        {
            _pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _isGamePaused = true;
        }
    

        // Update is called once per frame
        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (_isGamePaused)
                {
                    ShowMouseCursor();
                    Resume();
                    
                }
                else
                {
                    HideMouseCursor();
                    Pause();
                }

            }
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