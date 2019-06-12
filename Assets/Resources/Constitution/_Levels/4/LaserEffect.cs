using UnityEngine;

public class LaserEffect : MonoBehaviour
{
    public GameObject lineObj;
    public GameObject hitObj;
    public GameObject launchObj;
    public float transparencyMin = 30;
    public float transparencyMax = 70;
    void Update()
    {
        float randomA = Random.Range(transparencyMin, transparencyMax) / 100;
        if (lineObj != null) {
            LineRenderer line = lineObj.GetComponent<LineRenderer>();
            Color startColor = line.startColor;
            Color endColor = line.endColor;
            startColor.a = endColor.a = randomA;
            line.startColor = startColor;
            line.endColor = endColor;
        } else {
            Debug.Log("lineObj == null");
        }
        if (hitObj != null) {
            Color color = hitObj.GetComponent<SpriteRenderer>().color;
            color.a = randomA;
            hitObj.GetComponent<SpriteRenderer>().color = color;
        } else {
            Debug.Log("hitObj == null");
        }
        if (launchObj != null) {
            Color color = launchObj.GetComponent<SpriteRenderer>().color;
            color.a = randomA;
            launchObj.GetComponent<SpriteRenderer>().color = color;
        } else {
            Debug.Log("launchObj == null");
        }
    }
}
