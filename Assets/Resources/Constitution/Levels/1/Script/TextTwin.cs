using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTwin : MonoBehaviour
{
    public float twinkleSpeed = 1.0f;
    public float twinkleOffset = 0.0f;
    public float twinkRange = 1;

    void Update() {
        Color color = gameObject.GetComponent<TextMesh>().color;
        color.a = Mathf.Abs(Mathf.Sin(Time.time * twinkleSpeed + twinkleOffset))*twinkRange;
        gameObject.GetComponent<TextMesh>().color = color;
    }
}
