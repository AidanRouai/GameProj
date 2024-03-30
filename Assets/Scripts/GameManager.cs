using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState //Define the different states of the game 
    {
        Gameplay,
        Paused,
        GameOver,
    }

    public GameState currentState;

    public GameState previousState; //Store the previous state of the game 

    public static GameManager instance;

    [Header("UI")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    //Displaying Current Stats 
    [Header("Current Stats Display")]
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentStrengthDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Text levelReachedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(4);
    public List<Image> chosenPassiveItemsUI = new List<Image>(4);

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public Text stopwatchDisplay;

    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null) //Warning to check if there is another singleton of this kind in the game 
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra" + this + "DELETED");
            Destroy(gameObject);
        }
        DisableScreens();
    }

    void Update()
    {
        //Define the behavior for each of the game states
         
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME OVER");
                    DisplayResults();
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if(currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; //Time is stopped in the game
            pauseScreen.SetActive(true);
            Debug.Log("GAME IS PAUSED");
        }

    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("GAME IS RESUMED");
        }
    }

    public void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    private void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignLevelReached(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenItemsUI(List<Image> chosenweaponsData, List<Image> chosenPassiveItemData)
    {
        if (chosenweaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen item lists data have differnt lengths");
            return;
        }

        for (int i = 0; i < chosenWeaponsUI.Count; i++)// Assign items data to chosenWeaponsUI
        {
            if (chosenweaponsData[i].sprite) //Check that the sprite of the element is not full
            {
                chosenWeaponsUI[i].enabled = true; //enable the element to show the sprite 
                chosenWeaponsUI[i].sprite = chosenweaponsData[i].sprite;
            }

            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemData[i].sprite)
            {
                chosenPassiveItemData[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemData[i].sprite;
            }

            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();
        if (stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
