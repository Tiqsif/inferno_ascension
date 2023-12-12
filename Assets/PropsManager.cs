using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : MonoBehaviour
{
    public Sprite[] wallProps;
    public Sprite[] bgProps;
    public int crowdedness = 1;
    private float wallPropSpawnRate = 0.1f;
    private float bgPropSpawnRate = 0.1f;

    private float spawnRectHalfWidth = 3f;
    private float spawnRectHalfHeight = 3f;

    private Transform player;
    private Vector3 spawnPos;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        int childrenInRect = 0;
        foreach (Transform child in transform)
        {
            if (child.position.y > player.position.y + 10  && child.position.y < player.position.y + 10 + spawnRectHalfHeight*2)
            {
                childrenInRect++;
            }

            if (child.position.y < player.position.y - 10)
            {
                Destroy(child.gameObject);
            }
        }

        if (childrenInRect < crowdedness)
        {
            if (Random.value > 0.5) // spawn bg or wall prop with equal probability
            {
                if(Random.value > 0.5) spawnPos = new Vector3(Random.Range(1.5f*-spawnRectHalfWidth, -spawnRectHalfWidth), player.position.y + 10+ Random.Range(0, spawnRectHalfHeight*2), 0);
                else spawnPos = new Vector3(Random.Range(spawnRectHalfWidth, 1.5f*spawnRectHalfWidth), player.position.y + 10+ Random.Range(0, spawnRectHalfHeight*2), 0);
                if (Random.value < wallPropSpawnRate)
                {
                    SpawnWallProp();
                }

            }
            else
            {
                spawnPos = new Vector3(Random.Range(-spawnRectHalfWidth, spawnRectHalfWidth), player.position.y + 10 + Random.Range(0, spawnRectHalfHeight*2), 0);
                if (Random.value < bgPropSpawnRate)
                {
                    SpawnBgProp();
                }


            }

        }
        
    }
    
    void SpawnWallProp()
    {
        
        GameObject prop = new GameObject("WallProp");
        prop.transform.position = spawnPos;
        prop.transform.parent = transform;
        prop.AddComponent<SpriteRenderer>().sortingOrder = 1;
        prop.GetComponent<SpriteRenderer>().sprite = wallProps[Random.Range(0, wallProps.Length)];
    }
     
    void SpawnBgProp()
    {
        GameObject prop = new GameObject("BgProp");
        prop.transform.position = spawnPos;
        prop.transform.parent = transform;
        prop.AddComponent<SpriteRenderer>().sortingOrder = -1;
        prop.GetComponent<SpriteRenderer>().sprite = bgProps[Random.Range(0, bgProps.Length)];
        if(Random.value > 0.5) prop.transform.localScale = new Vector3(-1, 1, 1);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (player)
        {
            Gizmos.DrawWireCube(new Vector3(0, player.position.y + 10 + spawnRectHalfHeight, 0), new Vector2(spawnRectHalfWidth*2, spawnRectHalfHeight*2));
        }
    }
}
