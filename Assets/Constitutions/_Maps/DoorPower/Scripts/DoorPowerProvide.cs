using UnityEngine;

public class DoorPowerProvide : MonoBehaviour
{
    public DoorPower provider;
    public string pointer;

    public float provideSpeed = 1;
    
    void Update()
    {
        if (pointer == "move") {
            if(provider.isProvide)
                GetComponent<MoveMapP2P>().moveSpeed = provideSpeed;
            else
                GetComponent<MoveMapP2P>().moveSpeed = 0;
        }
    }
}
