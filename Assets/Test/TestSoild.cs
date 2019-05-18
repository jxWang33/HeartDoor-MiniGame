using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoild : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("soild");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
