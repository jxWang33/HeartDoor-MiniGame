﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MapWall : MonoBehaviour
{
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("wall");
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
    }
    
    void Update()
    {
        
    }
}
