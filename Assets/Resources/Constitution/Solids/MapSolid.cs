using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MapSolid : MonoBehaviour
{
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("solid");
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
    }
    
    void Update()
    {
        
    }
}
