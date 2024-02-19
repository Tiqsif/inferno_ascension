using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using static GameManager;
using TMPro;

public class MenuManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject scoreHolder;
    private TextMeshProUGUI scoreText;
    private AudioSource audioSource;
    public AudioClip pauseSound;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        GameManager.OnGameStateChanged += HandleGameStateChanged; // subscribe to event
        audioSource = GetComponent<AudioSource>();
        
    }

  
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged; // unsubscribe to event for memory management
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        // when game state changes, this method will be called
        if (currentState == GameManager.GameState.Playing)
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            scoreHolder.SetActive(true);
        }
        else if (currentState == GameManager.GameState.Paused)
        {
            pauseMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            scoreHolder.SetActive(true);
        }
        else if (currentState == GameManager.GameState.GameOver)
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(true);
            GameObject.Find("YourNumber").GetComponent<TextMeshProUGUI>().text = GameManager.GetScore().ToString();
            GameObject.Find("HighNumber").GetComponent<TextMeshProUGUI>().text = GameManager.GetHighScore().ToString();
            scoreHolder.SetActive(false);
        }
    }
    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        gameOverMenu = GameObject.FindGameObjectWithTag("GameOverMenu");
        scoreHolder = GameObject.FindGameObjectWithTag("ScoreHolder");
        scoreText = scoreHolder.GetComponentInChildren<TextMeshProUGUI>();
        if (pauseMenu is null || gameOverMenu is null || scoreHolder is null || scoreText is null)
        {
            Debug.LogError("Pause Menu or Game Over Menu is not found!");
        }
        else
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            scoreHolder.SetActive(true);
        }
    }


    void Update()
    {
        // if game is playing and player presses escape or p, pause the game
        HandlePauseMenu();

        scoreText.SetText(GameManager.GetScore().ToString());
        
    }
    void HandlePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameManager.GetGameState() == GameState.Playing)
            {
                gameManager.SetGameState(GameManager.GameState.Paused);
                audioSource.PlayOneShot(pauseSound);

            }
            else if (gameManager.GetGameState() == GameState.Paused)
            {
                gameManager.SetGameState(GameManager.GameState.Playing);
            }
        }
    }
    // --- UI Button Methods ---
    public void PlayPressed()
    {
        gameManager.SetGameState(GameManager.GameState.Playing);

    }
    
    public void RestartGame() 
    {
        gameManager.SetGameState(GameManager.GameState.Playing);
    }
    public void MainMenu()
    {
        gameManager.SetGameState(GameManager.GameState.Menu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
