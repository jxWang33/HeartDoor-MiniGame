using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleFlash : MonoBehaviour
{
    public Text dataText;
    public Text titleText;
    public Text markText;

    [SerializeField]
    private float lastTime = 1.0f;
    public float flashSpeed = 0.5f;

    void Update() {
        FlashText(dataText);
        FlashText(titleText);
        FlashText(markText);
    }

    void FlashText(Text t) {
        if (t.color.a < 1 && lastTime >= 0) {
            Color color = t.color;
            color.a += flashSpeed * Time.deltaTime;
            t.color = color;
        }

        if (t.color.a >= 1 && lastTime >= 0) {
            lastTime -= Time.deltaTime;
            Color color = t.color;
            color.a = 1;
            t.color = color;
        }
        if (lastTime < 0) {
            Color color = t.color;
            color.a -= flashSpeed * Time.deltaTime;
            t.color = color;
            if (t.color.a < 0) {
                Destroy(gameObject);
            }
        }
    }

    public void Set(string d,string t,string m) {
        dataText.text = d;
        titleText.text = t;
        markText.text = m;
    }
}
