using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Sprite[] stoneSprites; // plaform sprite consists of multiple stone sprites
    [SerializeField] private Sprite coinSprite;
    private GameObject coin;
    public Transform standPoint; // the point where the player stands on the platform
    private bool hasCoin = false;
    public bool isCurrentPlatform = false; 
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        ShuffleArray(stoneSprites);
        hasCoin = Random.Range(0, 10) == 0; // 10% chance of having a coin
        standPoint = transform.GetChild(0).GetComponent<Transform>();
    }
    void Start()
    {

        for (int i = 0; i < 3; i++)
        {
            GameObject spriteObj = new GameObject($"Sprite{i}");
            spriteObj.transform.parent = transform;
            spriteObj.transform.position = new Vector3(transform.position.x + (i - 1) * 0.64f, transform.position.y, transform.position.z);
            spriteObj.AddComponent<SpriteRenderer>().sprite = stoneSprites[i];
        }
        if (hasCoin)
        {
            coin = new GameObject("Coin");
            coin.transform.parent = transform;
            coin.transform.position = new Vector3(transform.position.x, transform.position.y + 0.64f, transform.position.z);
            coin.AddComponent<SpriteRenderer>().sprite = coinSprite;
        }

        /*
        GameObject sprite0 = new GameObject("sprite0");
        sprite0.transform.parent = transform;
        sprite0.transform.position = new Vector3(transform.position.x -0.64f ,transform.position.y,transform.position.z);
        sprite0.AddComponent<SpriteRenderer>().sprite = stone0;

        GameObject sprite1 = new GameObject("sprite1");
        sprite1.transform.parent = transform;
        sprite1.transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z);
        sprite1.AddComponent<SpriteRenderer>().sprite = stone1;

        GameObject sprite2 = new GameObject("sprite2");
        sprite2.transform.parent = transform;
        sprite2.transform.position = new Vector3(transform.position.x + 0.64f, transform.position.y, transform.position.z);
        sprite2.AddComponent<SpriteRenderer>().sprite = stone2;
        */

        //Debug.Log(standPoint.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ShuffleArray<T>(T[] array) // used for randomizing stone sprites
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);
            T temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
    public bool HasCoin()
    {
        return hasCoin;
    }
    public void SetCurrent() // if player hooks to current platform dont add score
    {
        if (isCurrentPlatform) return;
        if (hasCoin)
        {
            GameManager.AddScore(10);
            hasCoin = false;
        }
        Destroy(coin);
        isCurrentPlatform = true;
    }
}
