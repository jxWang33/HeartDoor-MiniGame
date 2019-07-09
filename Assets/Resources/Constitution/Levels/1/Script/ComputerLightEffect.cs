using UnityEngine;

public class ComputerLightEffect : MonoBehaviour
{
    public Vector2 range = new Vector2(0, 5);
    private void FixedUpdate()
    {
        float randomI = (float)GMManager.rd.NextDouble()*(range.y-range.x)+range.x;
        GetComponentInChildren<Light>().intensity = randomI;
    }
}
