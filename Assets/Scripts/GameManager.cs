using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using static GameManager;

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

    void Update()
    {

    }

    public void SetGameState(GameState state)
    {
        // set state for other scripts to use
        GameState previousState = gameState;
        gameState = state;

        Debug.Log($"Game state changed from {previousState} to {gameState}");
        switch (gameState)
        {
            case GameState.Menu:
                EditorSceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                if (previousState == GameState.Paused)
                {
                    break; // returning from pause menu to playing
                }
                EditorSceneManager.LoadScene("Assets/Scenes/Game.unity");
                
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
                EditorSceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
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
        HighScoreData highScoreData = new HighScoreData
        {
            highScore = highScore
        };
        string path = "Assets/highscore.json"; 
        string json = JsonUtility.ToJson(highScoreData);
        Debug.Log(json);
        File.WriteAllText(path, json);

    }
    public static int GetHighScore()
    {
        string json = (File.ReadAllText("Assets/highscore.json"));
        HighScoreData highScoreData = JsonUtility.FromJson<HighScoreData>(json);
        highScore = highScoreData.highScore;
        return highScore;
    }
    
}