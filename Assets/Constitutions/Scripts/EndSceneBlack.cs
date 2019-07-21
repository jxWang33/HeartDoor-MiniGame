using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneBlack : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(Appear());
    }

    IEnumerator Appear() {
        while (Time.timeSinceLevelLoad < 0.5f) {
            yield return new WaitForEndOfFrame();
        }

        while (GetComponent<Image>().color.a > 0.4f) {
            Color tempColor = GetComponent<Image>().color;
            tempColor.a -= Time.deltaTime;
            if (tempColor.a < 0.4f) {
                tempColor.a = 0.4f;
            }
            GetComponent<Image>().color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        transform.SetSiblingIndex(transform.parent.childCount - 3);
    }
}
