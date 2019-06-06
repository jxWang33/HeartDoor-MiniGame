using UnityEngine;

public class BgStarTwinkle : MonoBehaviour
{
    public float twinkleSpeed = 1.0f;
    public float twinkleOffset = 0.0f;

    void Update() {
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        color.a = Mathf.Abs(Mathf.Sin(Time.time * twinkleSpeed + twinkleOffset));
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
