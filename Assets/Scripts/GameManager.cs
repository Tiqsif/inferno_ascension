using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{// ui prefablerini instancela
    static GameManager instance;
    static GameState gameState;
    public static event Action<GameState> OnGameStateChanged;
    static int score = 0;
    public static GameManager Instance
    {
        get
        {
            if (instance == null) // En baþta burasý var mý yok mu diye kontrol ediyoruz ve yoksa buradan bir kopya oluþturuyoruz.
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
    }
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {

    }

    public void SetGameState(GameState state)
    {
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
                if (previousState == GameState.Paused) break;
                EditorSceneManager.LoadScene("Assets/Scenes/Game.unity");break;
                
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
            default:
                EditorSceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
                Time.timeScale = 1f;
                break;
        }
        OnGameStateChanged?.Invoke(gameState);

    }
    public GameState GetGameState()
    {
        return gameState;
    }
    public static void AddScore(int value)
    {
        score += value;
    }
    /*
    public void PlayPressed()
    {
        Debug.Log("Play pressed");
        SetGameState(GameState.Playing);

    }
    public void RestartPressed()
    {
        Debug.Log("Restart pressed");
        SetGameState(GameState.Playing);
    }
    public void MainMenuPressed()
    {
        Debug.Log("Main Menu pressed");
        SetGameState(GameState.Menu);
    }*/
}