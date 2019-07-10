using UnityEngine;
using UnityEngine.UI;

public class StartScenePrompt : MonoBehaviour
{
    public float twinkleSpeed = 1.0f;
    public float twinkleOffset = 0.0f;
    public float twinkleRangeMin = 0;
    public float twinkleRangeMax = 1;

    void Update() {
        Color color = gameObject.GetComponent<Text>().color;
        color.a = (Mathf.Sin(Time.time * twinkleSpeed + twinkleOffset) + twinkleRangeMin + 1) * (twinkleRangeMax - twinkleRangeMin) / 2;
        gameObject.GetComponent<Text>().color = color;
    }
}