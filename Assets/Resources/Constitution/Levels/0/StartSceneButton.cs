using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartSceneButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image curImage;
    private Text buttonText;

    void Start() {
        buttonText = GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Vector2 tempPos = curImage.transform.position;
        tempPos.y = transform.position.y;
        curImage.transform.position = tempPos;

        buttonText.fontStyle = FontStyle.Bold;
    }

    public void OnPointerExit(PointerEventData eventData) {
        buttonText.fontStyle = FontStyle.Normal;
    }
}
