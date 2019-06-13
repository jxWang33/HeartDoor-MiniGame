using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKey : MonoBehaviour
{
    float radian = 0; 
    public float radianSpeed = 2f;
    public float radius = 0.2f;
    Vector3 startPos;
    public GameObject player;
    public float moveSpeed = 0.1f;
    public float radiusOfAdsorption = 3.0f;
    void Awake()
    {
        gameObject.name = "greenKey";
        gameObject.layer = LayerMask.NameToLayer("interact");
        startPos = transform.position;
        player = GameObject.Find("nUsr");
    }
    
    void Update() {
        radian += radianSpeed*Time.deltaTime; 
        float dy = Mathf.Cos(radian) * radius;

        // 吸附效果
        if (player != null && Vector2.Distance(transform.position, player.transform.position) < radiusOfAdsorption) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed);
        } else {
            transform.position = startPos + new Vector3(0, dy, 0);
        }
    }
}
