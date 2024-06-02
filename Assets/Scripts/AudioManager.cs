using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    AudioClip menuMusic;
    AudioClip gameMusic;
    AudioClip gameOverMusic;
    private AudioSource audioSource;
    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
        menuMusic = Resources.Load<AudioClip>("Audio/MenuMusic");
        gameMusic = Resources.Load<AudioClip>("Audio/GameMusic");
        gameOverMusic = Resources.Load<AudioClip>("Audio/MenuMusic");
        if (GameManager.Instance.GetGameState() == GameManager.GameState.Menu)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (GameManager.Instance.GetGameState() == GameManager.GameState.Playing)
        {
            audioSource.clip = gameMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameManager.GameState state, GameManager.GameState previousState)
    {
        if (state == GameManager.GameState.Menu)
        {
            //Debug.Log("Menu");
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (state == GameManager.GameState.Playing)
        {
            if (previousState == GameManager.GameState.Paused)
            {
                audioSource.UnPause();
                //Debug.Log("Paused to Playing");
            }
            else
            {
                //Debug.Log("Directly to Playing");
                audioSource.clip = gameMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (state == GameManager.GameState.GameOver)
        {
            //Debug.Log("GameOver");
            audioSource.clip = gameOverMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (state == GameManager.GameState.Paused)
        {
            //Debug.Log("Paused");
            audioSource.Pause();
        }
    }
}
