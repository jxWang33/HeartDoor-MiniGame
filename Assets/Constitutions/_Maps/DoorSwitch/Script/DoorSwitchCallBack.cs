using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DoorSwitchDelegate();

public class DoorSwitchCallBack : MonoBehaviour
{
    public string pointer = "thorn";

    public DoorSwitchDelegate OnSwitchOn;
    public DoorSwitchDelegate OnSwitchOff;
    public DoorSwitchDelegate OnChange;
    
    public Vector2 startPos;
    public Vector2 move;
    private Vector2 targetPos;
    public float moveSpeed;


    void Start()
    {
        startPos = transform.position;
        targetPos = startPos;
        switch (pointer) {
            case "thorn":
                OnSwitchOn = ThornFuncOn;
                OnSwitchOff = ThornFuncOff;
                OnChange = ThornFuncChange;
                break;
            case "move":
                OnSwitchOn = MoveFuncOn;
                OnSwitchOff = MoveFuncOff;
                OnChange = MoveFuncChange;
                break;
        }
    }
    private void Update() {
        if (pointer == "move") {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }

    public void ThornFuncOn() {
        GetComponent<MapThorn>().SetDown();
    }
    public void ThornFuncOff() {
        GetComponent<MapThorn>().SetUp();
    }
    public void ThornFuncChange() {
        GetComponent<MapThorn>().Change();
    }

    public void MoveFuncOn() {
        targetPos = startPos + move;
    }
    public void MoveFuncOff() {
        targetPos = startPos;
    }
    public void MoveFuncChange() {
        if (targetPos == startPos + move)
            targetPos = startPos;
        else if (targetPos == startPos)
            targetPos = startPos + move;
    }


}
