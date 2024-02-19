using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    GameManager gameManager;
    private AudioSource audioSource;
    void Start()
    {
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayGame() //Play Button
    {
        audioSource.Play();
        StartCoroutine(StartGame());
        
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(.2f);
        gameManager.SetGameState(GameManager.GameState.Playing);
    }
    public void QuitGame() // Quit Button
    {
        Application.Quit(); 
    }

}