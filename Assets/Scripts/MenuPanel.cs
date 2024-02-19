using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //between 0 and 0.2
        image.pixelsPerUnitMultiplier = Mathf.Abs(Mathf.Sin(Time.time)) * 0.2f;
    }
}
