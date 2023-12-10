using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void PlayGame() //Play Button
    {
        gameManager.SetGameState(GameManager.GameState.Playing);
    }

    public void QuitGame() // Quit Button
    {
        Application.Quit(); 
    }

}