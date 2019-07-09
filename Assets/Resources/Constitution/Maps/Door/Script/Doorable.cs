using UnityEngine;

public abstract class Doorable : MonoBehaviour
{
    public abstract void OnDoorHurt(Vector2 dir);
}
