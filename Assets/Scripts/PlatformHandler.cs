using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    //handles the lava and platforms
    public float lavaStartHeight = -5f;
    public float lavaSpeed = 1f;
    private Platform[] platforms;
    private float lavaHeight;
    private float elapsedTime = 0;
    private Transform player;
    private bool gameOver = false;
    private Transform lava;
    [SerializeField] private Material lavaMaterial;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        lava = new GameObject("Lava").transform;
        lava.parent = transform;
        SpriteRenderer lavaSR = lava.AddComponent<SpriteRenderer>();
        lavaSR.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
        lavaSR.material = lavaMaterial;
        lava.localPosition = new Vector3(-10, 0, -2);
        lava.localScale = new Vector3(2000, 2000, 1);
        platforms = FindObjectsOfType<Platform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lavaHeight = lavaStartHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        lavaHeight += Time.deltaTime * lavaSpeed;
        lava.position = new Vector3(lava.position.x, lavaHeight - (lava.localScale.y / 100), lava.position.z);

        if (player.position.y < lavaHeight)
        {
            GameOver();

            player.GetComponent<Player>().Die();
        }
        //Debug.Log(elapsedTime);
        if (elapsedTime > 1)
        {
            platforms = FindObjectsOfType<Platform>();
            DestroyPlatformsBelowLava();
            CreatePlatforms();
            elapsedTime = 0;
        }
        else
        {
            elapsedTime += Time.deltaTime;
        }



    }

    private void DestroyPlatformsBelowLava()
    {

        foreach (Platform platform in platforms)
        {
            if (platform.transform.position.y < lavaHeight)
            {
                Debug.Log("Destroying platforms below lava");
                Destroy(platform.gameObject);
            }
        }

    }
    void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over");
        gameManager.SetGameState(GameManager.GameState.GameOver);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-100, lavaHeight, 0), new Vector3(100, lavaHeight, 0));
    }

    void CreatePlatforms()
    {
        float minY = player.position.y + 5;
        float maxY = minY + 5;
        float minX = -2.5f;
        float maxX = 2.5f;
        float placeMinY = minY + 2;
        float placeMaxY = maxY - 2;
        int platformsInArea = 0;
        foreach (Platform platform in platforms)
        {
            if (platform.transform.position.y > minY && platform.transform.position.y < maxY)
            {
                if (platform.transform.position.x > minX && platform.transform.position.x < maxX)
                {
                    platformsInArea++;
                }
            }
        }
        if (platformsInArea > 0) return;
        Random.InitState(System.DateTime.Now.Millisecond);
        float x = Random.Range(minX, maxX);
        Random.InitState(System.DateTime.Now.Millisecond);
        float y = Random.Range(placeMinY, placeMaxY);
        Platform platformInstance = Instantiate(Resources.Load<Platform>("Prefabs/Platform"), new Vector3(x,y,1), Quaternion.identity);
        
    }
}
