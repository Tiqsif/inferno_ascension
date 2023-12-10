using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using static GameManager;

public class MenuManager : MonoBehaviour
{
    GameManager gameManager;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        
    }

  
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameManager.GameState currentState)
    {
        if (currentState == GameManager.GameState.Playing)
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
        }
        else if (currentState == GameManager.GameState.Paused)
        {
            pauseMenu.SetActive(true);
            gameOverMenu.SetActive(false);
        }
        else if (currentState == GameManager.GameState.GameOver)
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }
    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        gameOverMenu = GameObject.FindGameObjectWithTag("GameOverMenu");
        if (pauseMenu is null || gameOverMenu is null)
        {
            Debug.LogError("Pause Menu or Game Over Menu is not found!");
        }
        else
        {
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gameManager.GetGameState() == GameState.Playing)
            {
                gameManager.SetGameState(GameManager.GameState.Paused);
            }
            else if (gameManager.GetGameState() == GameState.Paused)
            {
                gameManager.SetGameState(GameManager.GameState.Playing);
            }
        }
    }
    public void PlayPressed()
    {
        gameManager.SetGameState(GameManager.GameState.Playing);

    }
    
    public void RestartGame()
    {
        gameManager.SetGameState(GameManager.GameState.Playing);
    }
    public void MainMenu() //Play Button
    {
        gameManager.SetGameState(GameManager.GameState.Menu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
