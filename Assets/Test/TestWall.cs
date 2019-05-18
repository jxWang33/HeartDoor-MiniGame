using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("wall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {

    }
}
