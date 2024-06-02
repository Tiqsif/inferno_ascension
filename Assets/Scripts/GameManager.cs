using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{ 
    static GameManager instance;
    static GameState gameState;
    public static event Action<GameState, GameState> OnGameStateChanged;
    static int score = 0;
    static int highScore = 0;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) // singleton pattern
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
                
            }
            return instance;
        }
    }
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }
    private void Awake()
    {
        OnGameStateChanged?.Invoke(gameState, gameState); // invoke event for other scripts to listen to
    }
    void Start()
    {
        
        highScore = GetHighScore();
        Time.timeScale = 1.0f;
    }


    public void SetGameState(GameState state)
    {
        // set state for other scripts to use
        GameState previousState = gameState;
        gameState = state;

        //Debug.Log($"Game state changed from {previousState} to {gameState}");
        switch (gameState)
        {
            case GameState.Menu:
                SceneManager.LoadScene(0); // main menu
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                if (previousState == GameState.Paused)
                {
                    break; // returning from pause menu to playing
                }
                SceneManager.LoadScene(1); // game scene
                
                score = 0;
                break;
                
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                if (score > highScore)
                {
                    SetHighScore(score);
                }
                Time.timeScale = 0f;
                break;
            default: // default to menu
                SceneManager.LoadScene(0); // main menu
                Time.timeScale = 1f;
                break;
        }
        OnGameStateChanged?.Invoke(gameState, previousState); // invoke event for other scripts to listen to

    }
    public GameState GetGameState()
    {
        return gameState;
    }
    public static void AddScore(int value)
    {
        score += value;
        //Debug.Log($"Score: {score}");
    }
    public static int GetScore()
    {
        return score;
    }
    [System.Serializable]
    public struct HighScoreData
    {
        public int highScore;
    }
    public static void SetHighScore(int value)
    {
        highScore = value;
        PlayerPrefs.SetInt("highscore", highScore);

    }
    public static int GetHighScore()
    {
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScore = PlayerPrefs.GetInt("highscore");
        }
        else
        {
            highScore = 0;
        }

        return highScore;
    }
    
}