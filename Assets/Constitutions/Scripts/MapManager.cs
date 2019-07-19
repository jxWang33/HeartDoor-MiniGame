using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Vector2 mapSize;//in Unity size
    public UsrState usrState;

    void Awake()
    {
        gameObject.name = "MapManager";
    }


    void Update()
    {
        CheckUsrPos();    
    }

    void CheckUsrPos() {
        Rect tempBound = new Rect(-mapSize/2,mapSize);
        if (!usrState)
            return;
        if (!tempBound.Contains(usrState.transform.position)) {
            usrState.PositionBack();
        }
    }
}
