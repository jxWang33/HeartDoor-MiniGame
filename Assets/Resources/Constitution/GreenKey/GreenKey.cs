using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKey : MonoBehaviour
{
    float radian = 0; 
    public float radianSpeed = 0.5f;
    public float radius = 0.5f;
    Vector3 startPos; 
    void Awake()
    {
        gameObject.name = "greenKey";
        gameObject.layer = LayerMask.NameToLayer("interact");
        startPos = transform.position;
    }
    
    void Update() {
        radian += radianSpeed*Time.deltaTime; 
        float dy = Mathf.Cos(radian) * radius;
        transform.position = startPos + new Vector3(0, dy, 0);
    }
}
