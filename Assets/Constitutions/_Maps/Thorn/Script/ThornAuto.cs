using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornAuto : MonoBehaviour
{
    public float upLastTime = 1;
    public float downLastTime = 1;

    private float counter = 0;
    private bool isUp = false;

    private MapThorn mapThorn;

    void Start()
    {
        mapThorn = GetComponent<MapThorn>();

        if (isUp)
            counter = upLastTime;
        else
            counter = downLastTime;
    }
    
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0) {
            if (isUp) {
                counter = downLastTime;
                mapThorn.SetDown();
                isUp = false;
            }
            else {
                counter = upLastTime;
                mapThorn.SetUp();
                isUp = true;
            }
        }        
    }
}
