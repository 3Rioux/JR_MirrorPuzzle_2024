using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class SceneInit : MonoBehaviour
{
    /**
     * This will find the Game Managers prefab that contains ALL the Different Game Managers 
     */
    [SerializeField] private GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
        /**
         * Get the GameManager component from the Child of the GameObject
         */
        //gm = GetComponentInChildren<GameObject>();
        gm = gm.GetComponent<GameObject>();

        InitializeGameManager();
    }

    private void InitializeGameManager()
    {
        GameManager existingGameManager = FindObjectOfType<GameManager>();

        if (existingGameManager == null)
        {
            Debug.Log("CREATING PREFAB!");
            GameObject gameManagerPrefab = gm;
            DontDestroyOnLoad(gm);
            //Instantiate(gameManagerPrefab);
        }
    }
}
