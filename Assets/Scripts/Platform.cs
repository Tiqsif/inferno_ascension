using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]  private Sprite[] stoneSprites;
    public Transform standPoint;
    private void Awake()
    {
        ShuffleArray(stoneSprites);
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

        standPoint = transform.GetChild(0).GetComponent<Transform>();
        //Debug.Log(standPoint.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ShuffleArray<T>(T[] array)
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
}
