using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPropmt : MonoBehaviour
{
    public SpriteRenderer heartSprite;
    public TextMesh textMesh;
    public int changeNum;

    [SerializeField]
    private float lastCount = 1;

    public void Set(int num)
    {
        changeNum = num;
        if (num > 0)
            textMesh.text = "+";
        else
            textMesh.text = "";
        textMesh.text += num.ToString();
    }
    
    void Update()
    {
        if (lastCount > 0) {
            lastCount -= Time.deltaTime;
            Vector2 tempPos = transform.position;
            tempPos.y += Time.deltaTime;
            transform.position = tempPos;

            Color tempColor = textMesh.color;
            tempColor.a = lastCount;
            textMesh.color = tempColor;

            tempColor = heartSprite.color;
            tempColor.a = lastCount;
            heartSprite.color = tempColor;
        }
        else
            Destroy(gameObject);
    }
}
