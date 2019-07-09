using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MapSolid : MonoBehaviour
{
    public bool isOneSide = false;
    public Vector2 colliderSize = new Vector2(-1, -1);
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("solid");
        if(colliderSize == new Vector2(-1, -1))
            GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
        else
            GetComponent<BoxCollider2D>().size = colliderSize;
    }

    void Update()
    {
        
    }
}
