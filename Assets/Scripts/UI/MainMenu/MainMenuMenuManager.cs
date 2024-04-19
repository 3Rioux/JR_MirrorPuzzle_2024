using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        public void MainMenuPlayButton()
        {
            SceneManager.LoadSceneAsync(sceneBuildIndex: 1);
            Debug.Log("Loading Game Level");
        }
        public void MainMenuExitButton()
        {
            Application.Quit();
            Debug.Log("Quitting the game");
        }
    }
}