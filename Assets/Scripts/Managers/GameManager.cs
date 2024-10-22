using Audio;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //==============================================================================Game Manager
    /**
     * Create a static instance of the game manager 
     */
    public static GameManager gmInstance;


    //==============================================================================Game Manager

    //==============================================================================MENU VARIABLES START
    //====================================GAME SETTINGS VARIABLES START

    public int musicVolume = 0;
    public int soundEffectVolume = 0;
    public int ambiencelevel = 0;
    /**
     * Set the screens resolution 
     * !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!MAKE A LIST OR SOMETHING 
     */
    public int resoulution = 0;
    /**
     * make screen full screen or not 
     * 
     * if resolution == default then GetScreenResolution()
     * else set full screen to the desired screen resolution 
     */
    public bool fullScreen = false;


    //====================================GAME SETTINGS VARIABLES START


    /**
      * Button press variables for the pause menu
      */
    [SerializeField] private bool _isGamePaused;
    [SerializeField] private GameObject _pauseMenuUI;

    /**
     * HUD user Info 
     */
    [SerializeField] public GameObject _playerInfoUI;
    /**
     * main Menu
     */
    [SerializeField] private GameObject _mainMenuUI;

    /**
     * Endding Menu
     */
    [SerializeField] private GameObject _endingMenuUI;

    //==============================================================================MENU VARIABLES END

    //==============================================================================SCORE VARIABLES START ***KEY INFO***

    /**
     * The textbox used to display the score to the user 
     */
    private TextMeshPro scoreText; // this will NOT be set in the start but RATHER in the onLevelLoad()

    /**
     * Variable to store the score amount
     */
    private float score;



    //==============================================================================SCORE VARIABLES END

    //==============================================================================LOAD SCENE ELEMENTS START


    //==============================================================================LOAD SCENE ELEMENTS END
    /**
     * access the GUI (canvas) or in this case lodingPannel parent 
     */
    public GameObject loadingScreen;

    /**
     * Get the loading Bar that shows the progress 
     */
    public Image LoadingBarFill;

    //==============================================================================UI IN GAME OVERLAY ELEMENTS START
    /**
     * Will handle the navigation arrow that tells the player where to go in the UI of the Levels
     * this will change from "Find Key" to "Unlock Door With Key"
     */
    public TextMeshProUGUI gameStateText; // this will NOT be set in the start but RATHER in the onLevelLoad()

    /**
     * Variable to store the current Level progression state
     *      0 = find key
     *      1 = find door
     *      2 = gameOver // Lost
     *      3 = gameWon  // Success opening the door 
     */
    private int currentState;

    //==============================================================================UI IN GAME OVERLAY ELEMENTS END

    //==============================================================================CURRENT SCENE VARIABLES START

    /**
      * public variable to store the current level the player is on 
      * default is level 1 (Scene 2)
      */
    public int currentLevel = 2;

    /**
      * private variable to store the previous level the player is on 
      *     - called by the NextScene() => previousLevel = SceneManager.CurrentIndex
      *     - XXXonSceneLoad()XXX 
      */
    private int previousLevel = 0;

    /**
     * This bool will allow for a check every time a new scene is loaded 
     * It is called in the update LevelWasLoaded && loadNextScene methods 
     * update LevelWasLoaded will set to false 
     * loadNextScene will set to true again 
     * 
     * ***This is so that we can save RAM and not checking a bunch of things every update if we only need it at the start ***
     */
    private bool checkOnLoad = true;

    //==============================================================================CURRENT SCENE VARIABLES END

    //==============================================================================OPEN GATE START
    /**
     * bool to allow the gate to open or close 
     * 
     *      ***Set to True after picking up the key***
     *      ***Make sure to set it to False after Ending game***
     */
    public bool isGateOpen = false;

    /**
     * Gate Location for the UI Minimap
     */
    public GameObject gate;

    //==============================================================================OPEN GATE END
    //==============================================================================VEHICLE START

    //access the vehicle control info textbox
    public GameObject HelpText;

    //==============================================================================VEHICLE END
    //============================================================================== START

    //--------------------------------------------------------Player Controls
    /**
     * private access to the players movement script 
     */
    private CharacterController player;
    //--------------------------------------------------------Player Controls 




    //=================================================================================================================
    void Awake()
    {
        if (gmInstance == null)
        {
            gmInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //if (gmInstance == null)
        //{
        //    /**
        //     * If game manager does NOT already exist create(instatiate) it.
        //     */
        //    gmInstance = this;

        //    /**
        //     * Set the default amount of crystals to -1 a score we can never have 
        //     * this will help add to the score later 
        //     */
        //    //CRYSTALCOUNT = -1;

        //    /**
        //     * Set the default amount of needed crystals to -1 a score we can never have 
        //     */
        //    //crystalsNeeded = -1;

        //    /**
        //     * Set the game state to a default value
        //     */
        //    //UpdateGameState(GameState.SelectColor);

        //}
        //else
        //{
        //    Destroy(gameObject);// just incase the GameManager already exists then we dont need to create another one 
        //}
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /**
         * Check to see if the current active scene is a level or if its Main Menu or Endding Menu
         * (Main index 0 && End index 1) Everything else is a level 
         */

        if (SceneManager.GetActiveScene().buildIndex > 1 && checkOnLoad)
        {
            /**
             * Could move to LevelWasLoaded() 
             * 
             * Just tries to find the players controller 
             */
            if (SceneManager.GetActiveScene().buildIndex > 1 && checkOnLoad) // make sure we are in a level before looking for a player + Make sure it only checks 1 time
            {
                //Get the players movement script for the game pause 
                player = FindObjectOfType<CharacterController>().GetComponent<CharacterController>();
            }
            else if (player == null && checkOnLoad)
            {
                Debug.Log("Failled to find Player");
            }

            //if game level 
            LevelWasLoaded();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1 && checkOnLoad)
        {
            // end menu
            //EndGameWasLoad();
        }
        else
        {
            //if main

        }



        /**
          * if the escape key is pressed then pause the game 
          * if game already paused resume game 
          */
        if (Keyboard.current.escapeKey.wasPressedThisFrame && SceneManager.GetActiveScene().buildIndex > 1) // make sure the game scene is not main or ending 
        {
            if (_isGamePaused)
                ResumeButton();
            else
                PauseButton();
        }
    }

    //========================================================================================================Game State Method

    private void UpdateStateText(String currentGameStateText)
    {
        // gameStateText.text = currentGameStateText;
        gameStateText.outlineColor = Color.yellow;
        //message 

    }// end UpdateStateText

    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.FindKey:
                UpdateStateText("Find The Gate Key in the Maze to unlock the Maze Exit!");
                gameStateText.outlineColor = Color.white;
                break;
            case GameState.DoorKeyPickedUp:
                UpdateStateText("Exit Maze By Unlocking The Locked Exit Door On The Other Side Of The Maze!");
                gameStateText.outlineColor = Color.white;
                break;
            case GameState.DoorOppenned:
                UpdateStateText("Drive to the Gate Portal and Win!");
                gameStateText.outlineColor = Color.white;
                break;
            case GameState.EndLevel:

                EndGameWasLoad();
                //_endingMenuUI.SetActive(true);
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
                //default:
                //throw new ArgumentOutOfRangeException(nameof(newState), newState, null);

        }// end switch 
    }// end UpdateGameState 

    //========================================================================================================Game State Method

    //========================================================================================================Display Information  
    /**
     * This method increases the overal score by the int amount +
     * calls a method to update the displayed score textbox 
     * 
     * Input: int amount: Score increase amount
     */
    public void UpdateScoreCount(int amount)
    {
        /**
         * FUTURE ADD an if else so that the score will be added ot substracted from the total depending on a bool isAdding 
         * if(!isAddind && CRYSTALCOUNT > 0) -=
         * else += 
         */
        //CRYSTALCOUNT += amount; // the score gets increased by whatever the amount is 

        /**
         * Update the UI text with new crystal amount 
         */
        //UpdateScoreText(scoreText);

        /**
         * end the game after the CRYSTALCOUNT reaches the desired amount 
         */
        //if (CRYSTALCOUNT < 5)
        //{
        //    //GAME_STATE = state2_1;
        //}
        //else if (CRYSTALCOUNT >= 5 && CRYSTALCOUNT < 10)
        //{
        //    //GAME_STATE = state2_2;
        //}
        //else if (CRYSTALCOUNT > 10)
        //{
        //    //GAME_STATE = state2_3;
        //}//end else if 

        //UpdateStateText();
    }//end IncreaseScore

    /**
      * update the UI with the current crystal counter
      */
    private void UpdateScoreText(String text)
    {
        // Update the TextMeshPro text with the current value of myVariable
        //if (crystalCountText != null)
        //{
        //    crystalCountText.text = CRYSTALCOUNT.ToString();
        //}
    }// end UpdateCrystalCounter
    //========================================================================================================Display Information  

    //========================================================================================================Scene Load Method
    /**
         * using IEnumerator because we are working with delays 
         */
    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        //wait for the set delay 
        yield return new WaitForSeconds(delay);

        /**
         * Use the UpdateStateText method to change the state 
         */

        //if (GAME_STATE.Equals(state1))
        //{
        //    SCORE = 0;
        //    UpdateScoreText();

        //    GAME_STATE = state2;
        //    UpdateStateText();
        //}
        //else if (GAME_STATE.Equals(state2))
        //{
        //    GAME_STATE = state3;
        //    UpdateStateText();
        //}//end if else  if

        //load the new scene
        SceneManager.LoadScene(sceneName);
    }// end LoadSceneAfterDelay


    /**
     * Called when loading into a level 
     * 
     *      1. set all the count variable to 0 
     *      2. Reset bools 
     *      3. Reset Textboxs 
     *      4.???
     */
    private void LevelWasLoaded()
    {
        Debug.Log("onLevelLoad");
        /**
         * Turn On the HUD overlay + Close Main Menu and Ending menus 
         */
        _playerInfoUI.SetActive(true);
        _pauseMenuUI.SetActive(false);
        _endingMenuUI.SetActive(false);
        _mainMenuUI.SetActive(false);

        /**
         * set the current level to the current scene IF i want to add a Resume Level function**********************************************
         */
        //currentLevel = SceneManager.GetActiveScene().buildIndex;

        //set the current level back to 2 
        currentLevel = 2;


        checkOnLoad = false; // stop the load functions from happening more than once
        score = 0; // set the default score to 0 for a default score ***Reset score every time a new level is loaded 

        /**
         * Set the score textbox for the scene 
         */
        //scoreText = FindAnyObjectByType<TextMeshProUGUI>();
        //UpdateScoreText(scoreText); // display the current default score to the user 

        /**
         * Set the state textbox for the scene
         */
        //gameStateText = FindAnyObjectByType<TextMeshProUGUI>();
        //gameStateText.text = currentState.ToString(); // display the current default score to the user 
        UpdateGameState(GameState.FindKey);



    }//end LevelWasLoaded
    //========================================================================================================Scene Load Method

    //========================================================================================================MAIN MENU 
    //++++++++++++++++++++++++++++++++LOAD SCENE
    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        //activate the loading screen
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            //get the progress of the scene loading 
            float progressValue = Mathf.Clamp01(operation.progress / 0.1f);

            //set the loading bar size to be == to progress 
            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
        /**
         * Make sure the currentScene == the current scene after the scene was loaded
         */
        //currentLevel = SceneManager.GetActiveScene().buildIndex; 

        //activate the loading screen
        loadingScreen.SetActive(false);
    }
    //++++++++++++++++++++++++++++++++LOAD SCENE

    public void PlayButton()
    {
        /**
         * Activate the checkOnload bool when we start playing the game
         */
        checkOnLoad = true;

        Time.timeScale = 1f;

        HideMouseCursor();

        StartCoroutine(LoadSceneAsync(currentLevel));


        AudioManager.Instance.PlayAmbient("WindAmbient");
        AudioManager.Instance.PlayMusic("GameTheme");

        Debug.Log("Loading Game Level " + currentLevel);
    }

    /**
     * method To exit the game
     * 
     * ***Only works once BUILDT
     */
    public void ExitButton()
    {
        Application.Quit();

        Debug.Log("Quitting the game");
    }

    //========================================================================================================MAIN MENU 

    //========================================================================================================END GAME MENU 

    /**
     * USING GAME STATE IN GAMEMANAGER UPDATE TO CALL THIS FUNCTION
     */
    private void EndGameWasLoad()
    {
        // Re Enable the mouse like in pause menu/main 
        ShowMouseCursor();

        /**
         * Turn On the HUD overlay + Close Main Menu and Ending menus 
         */
        _playerInfoUI.SetActive(false);
        _pauseMenuUI.SetActive(false);
        _mainMenuUI.SetActive(false);
        _endingMenuUI.SetActive(true);

        /**
         * Make sure to close the gate for the next level
         */
        isGateOpen = false;

        /**
         * Stops the check from happenning more than once
         */
        checkOnLoad = false;


        /** THIS SHOULD BE CALLED BY THE endingScene
         * 
         * Make sure the next level exists
         * - 2 for the end game menu and the main menu == total amount of levels available to play 
         */


        if (SceneManager.GetActiveScene().buildIndex != 1) //end scene is index 1
        {
            /**
             * Return to the main menu OR !!! make it so that the Next Level Button does not show up 
             */
            StartCoroutine(LoadSceneAsync(1));
            //SceneManager.LoadSceneAsync(1); // else return to the main menu 
        }
        //else
        //{
        //    Debug.Log("Not End Scene but still level scene: " + SceneManager.GetActiveScene().buildIndex);
        //}

        /**
         * Change the Music from level or endgame to the mainmenu music +++
         * Change Stop playing ambient music in Main Menu 
         */
        AudioManager.Instance.PlayMusic("MainMenuMusic");
        AudioManager.Instance.PlayAmbient("None");
    }

    /**
     * Next button method for the end scene
     * 
     * Description: This method will start the next game level by taking the currentLevel int that was set in the onSceneLoad() method and add 1 
     *              IF the game runs out of scenes(Levels) to play the method will return the player to the main menu instead 
     */
    public void nextLevelButton()
    {
        /**
         * Add to the previous level 
         */
        ++currentLevel;
        Debug.Log("Current Game Level Index: " + (currentLevel));

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(LoadSceneAsync(currentLevel));
            Debug.Log("Loading Game Level " + (currentLevel - 1));
        }
        else
        {
            Debug.Log("Error Loading Next Scene " + (currentLevel - 1));
        }

        /**
         * After level Load
         */
        HideMouseCursor();

        /**
         * Update state Test ----------------------------------------------------------??? XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXShould remove from here and have the onSceneLoad() do it 
         */
        UpdateGameState(GameState.FindKey);

        Time.timeScale = 1f;
        _isGamePaused = false;


        /**
         * remove the old player controller 
         */
        player = null;

        /**
         * Activate the checkOnload bool 
         */
        checkOnLoad = true;


        /**
         * Reset the audio to the level audio rather than the end scene audio 
         */
        AudioManager.Instance.PlayAmbient("WindAmbient");
        AudioManager.Instance.PlayMusic("GameTheme");
        Debug.Log("Loaded Next Scene Success");


        /**
         * Turn On the HUD overlay + Close Main Menu and Ending menus 
         */
        //_playerInfoUI.SetActive(true);
        //_pauseMenuUI.SetActive(false);
        //_endingMenuUI.SetActive(false);
        //_mainMenuUI.SetActive(false);



        ///************************************************************
        // * Check if the current scene is the last scene (Level) or not 
        // */
        //if (SceneManager.sceneCount >= currentLevel)
        //{





        //}
        //else
        //{
        //    Debug.Log("Loaded Next Scene But ran out of scenes");
        //    Debug.Log("Loading Main Menu");
        //    StartCoroutine(LoadSceneAsync(0));

        //}

    }

    ///**
    //* Next button method for the end scene
    //* 
    //* Description: This method will start the next game level by taking the currentLevel int that was set in the onSceneLoad() method and add 1 
    //*              IF the game runs out of scenes(Levels) to play the method will return the player to the main menu instead 
    //*/
    //public void nextLevelButton()
    //{
    //    /**
    //     * Add to the previous level 
    //     */
    //    ++currentLevel;
    //    Debug.Log("Current Game Level Index: " + (currentLevel));

    //    if (SceneManager.GetActiveScene().buildIndex == 1 && currentLevel < (SceneManager.sceneCountInBuildSettings - 1))
    //    {
    //        StartCoroutine(LoadSceneAsync(currentLevel));
    //        Debug.Log("Loading Game Level " + (currentLevel - 1));
    //        Debug.Log("TotalScenes " + SceneManager.sceneCountInBuildSettings);

    //        /**
    //        * After level Load
    //        */
    //        HideMouseCursor();

    //        /**
    //         * Update state Test ----------------------------------------------------------??? XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXShould remove from here and have the onSceneLoad() do it 
    //         */
    //        UpdateGameState(GameState.FindKey);

    //        Time.timeScale = 1f;
    //        _isGamePaused = false;


    //        /**
    //         * remove the old player controller 
    //         */
    //        player = null;

    //        /**
    //         * Activate the checkOnload bool 
    //         */
    //        checkOnLoad = true;


    //        /**
    //         * Reset the audio to the level audio rather than the end scene audio 
    //         */
    //        AudioManager.Instance.PlayAmbient("WindAmbient");
    //        AudioManager.Instance.PlayMusic("GameTheme");
    //        Debug.Log("Loaded Next Scene Success");
    //    }
    //    else
    //    {
    //        Debug.Log("Error Loading Next Scene " + (currentLevel - 1));
    //        /**
    //         * if the game runs out of levels (minus the Main + End scenes!!)
    //         */
    //        if (currentLevel == (SceneManager.sceneCountInBuildSettings - 1)) BackToMainMenuButton(); Debug.Log("Ran Out of Levels!!!");
    //    }




    //    /**
    //     * Turn On the HUD overlay + Close Main Menu and Ending menus 
    //     */
    //    //_playerInfoUI.SetActive(true);
    //    //_pauseMenuUI.SetActive(false);
    //    //_endingMenuUI.SetActive(false);
    //    //_mainMenuUI.SetActive(false);



    //    ///************************************************************
    //    // * Check if the current scene is the last scene (Level) or not 
    //    // */
    //    //if (SceneManager.sceneCount >= currentLevel)
    //    //{





    //    //}
    //    //else
    //    //{
    //    //    Debug.Log("Loaded Next Scene But ran out of scenes");
    //    //    Debug.Log("Loading Main Menu");
    //    //    StartCoroutine(LoadSceneAsync(0));

    //    //}

    //}
    //========================================================================================================END GAME MENU 


    //========================================================================================================PAUSE MENU 

    private void Resume()
    {
        //Cursor.visible = false; // makes the mouse insivible when playing
        //Cursor.lockState = CursorLockMode.Locked;
        HideMouseCursor();

        _playerInfoUI.SetActive(true);
        _pauseMenuUI.SetActive(false);
        player.enabled = true;
        Time.timeScale = 1f;
        _isGamePaused = false;
    }

    private void Pause()
    {
        //Cursor.visible = true; // makes the mouse visible when press escape
        //Cursor.lockState = CursorLockMode.None;
        ShowMouseCursor();

        _playerInfoUI.SetActive(false);
        _pauseMenuUI.SetActive(true);
        player.enabled = false;
        Time.timeScale = 0f;
        _isGamePaused = true;
    }

    //^^^ HAS NO settings buttons
    //???????????????????????????????????????????????????????????????????????????????????I have 2 pause and resume method button presses 
    public void ResumeButton()
    {
        HideMouseCursor();

        _playerInfoUI.SetActive(true);
        _pauseMenuUI.SetActive(false);
        player.enabled = true;
        Time.timeScale = 1f;
        _isGamePaused = false;
    }

    private void PauseButton()
    {
        ShowMouseCursor();

        _playerInfoUI.SetActive(false);
        _pauseMenuUI.SetActive(true);
        player.enabled = false;
        Time.timeScale = 0f;
        _isGamePaused = true;
    }

    public void BackToMainMenuButton()
    {
        _playerInfoUI.SetActive(false);
        _pauseMenuUI.SetActive(false);
        _endingMenuUI.SetActive(false);
        _mainMenuUI.SetActive(true);

        Time.timeScale = 1f;
        _isGamePaused = false;

        ShowMouseCursor();


        //SceneManager.LoadSceneAsync(0);
        if (SceneManager.GetActiveScene().buildIndex != 0) //end scene is index 1
        {
            /**
             * Return to the main menu OR !!! make it so that the Next Level Button does not show up 
             */
            StartCoroutine(LoadSceneAsync(0));
            //SceneManager.LoadSceneAsync(1); // else return to the main menu 

            /**
             * Reset levels to level 1 OR could have a default level const DEFAULTLEVEL = 2 that gets used by the play 
             *  and have current level be used by NEW button Resume Game...
             */
            currentLevel = 2;
        }

        //Destroy(AudioManager.Instance.gameObject);
        //AudioManager.Instance.Muted(true);
        /**
         * Change the Music from level or endgame to the mainmenu music +++
         * Change Stop playing ambient music in Main Menu 
         */
        AudioManager.Instance.PlayMusic("MainMenuMusic");
        AudioManager.Instance.PlayAmbient("None");

        Debug.Log("Going Back to Main Menu " + currentLevel);
    }
    //========================================================================================================PAUSE MENU 

    //========================================================================================================AUDIO MANAGEMENT


    //========================================================================================================AUDIO MANAGEMENT

    //========================================================================================================MOUSE HIDE/SHOW MENU 
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
    //========================================================================================================MOUSE HIDE/SHOW MENU 
}


/**
 * Can make this a separate class script 
 */
public enum GameState
{
    OpenningScene,
    Scene1,
    FindKey,
    DoorKeyPickedUp,
    DoorOppenned,
    EndLevel,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Lose
}
