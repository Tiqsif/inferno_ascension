using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip deathSound;
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Die()
    {
        audioSource.PlayOneShot(deathSound);
        Debug.Log("Player died");
        //GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
        //Destroy(gameObject);
    }
}
