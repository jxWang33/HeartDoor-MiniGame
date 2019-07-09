using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsrDoorPrompt : MonoBehaviour
{
    UsrState usrState;
    void Start()
    {
        usrState = GetComponentInParent<UsrState>();
    }
    
    void Update()
    {
        if (!usrState) {
            transform.position = new Vector3(0, 0, -100);
            return;
        }

        MapDoor mapDoor = usrState.GetDoor();
        if (mapDoor) {
            if (usrState.currentDir.x == -1 && usrState.nearDoor == UsrState.LEFT_DIR ||
                usrState.currentDir.x == 1 && usrState.nearDoor == UsrState.RIGHT_DIR)
                transform.position = (Vector2)mapDoor.transform.position
                    + (mapDoor.dashDistance - mapDoor.boxCollider2D.size.x - usrState.startColliderSize.x)
                    * usrState.currentDir;
        }
        else {
            transform.position = new Vector3(0, 0, -100);
        }
    }
}
