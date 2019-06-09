using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidMoveUpDown : MonoBehaviour
{
    public DoorPower doorPower;

    private Vector2 startPos;
    [SerializeField]
    private Vector2 endPos;
    [SerializeField]
    private Vector2 distance;
    [SerializeField]
    private float moveSpeed = 1;
    private Vector2 target;

    void Awake()
    {
        startPos = transform.position;
        endPos = startPos + distance;
        target = endPos;
    }
    
    void Update()
    {
        if(!doorPower.isLock)
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        if ((Vector2)transform.position == target) {
            if (target == endPos)
                target = startPos;
            else if (target == startPos)
                target = endPos;
        }
    }
}
