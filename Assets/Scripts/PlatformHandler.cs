using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    //handles the lava and platforms
    public float lavaStartHeight = -5f;
    private float lavaSpeed = 1.2f;
    private float lavaAcceleration = 0.004f;
    private Platform[] platforms;
    private float lavaHeight;
    private float elapsedTime = 0;
    private Transform player;
    private bool gameOver = false;
    private Transform lava;
    [SerializeField] private Material lavaMaterial;
    [SerializeField] private GameObject lavaParticle;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        // create lava sprite and assign material
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

        lavaHeight += Time.deltaTime * lavaSpeed; // increase lava height
        lava.position = new Vector3(lava.position.x, lavaHeight - (lava.localScale.y / 100), lava.position.z);

        if (player.position.y < lavaHeight)
        {
            GameOver();

            player.GetComponent<Player>().Die();
        }
        //Debug.Log(elapsedTime);
        if (elapsedTime > 1)
        {
            // check if there are platforms below lava every second
            platforms = FindObjectsOfType<Platform>();
            DestroyPlatformsBelowLava();
            CreatePlatforms();
            lavaSpeed += lavaAcceleration;
            GameManager.AddScore(1 * Mathf.CeilToInt(lavaSpeed - 1));
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
            if (platform.transform.position.y < lavaHeight - 1)
            {
                Debug.Log("Destroying platforms below lava");

                CreateParticle(new Vector3(platform.transform.position.x, lavaHeight, 0));
                Destroy(platform.gameObject);

            }
        }

    }
    void CreateParticle(Vector3 pos)
    {
        foreach (Transform child in transform)
        {
            // if child name has Particle in it, destroy it
            if (child.name.Contains("Particle"))
            {
                Destroy(child.gameObject);
            }
            
        }
        GameObject particle = Instantiate(lavaParticle, pos, Quaternion.identity);
        particle.transform.parent = transform;
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
        // create platforms randomly in an area above the player
        // area is a virtual box above the player
        float minY = player.position.y + 10;
        float maxY = minY + 5;
        float minX = -2f;
        float maxX = 2f;

        // check if there are platforms in a bigger area
        // create platforms only if there are none in a smaller area to avoid creating too close to each other
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
