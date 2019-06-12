using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public float rotateSpeed = 5.0f;
    public float scaleValue = 3.0f;
    public float scaleChangeValue = 0.1f;
    public GameObject boss;
    public Color evilColor = Color.red;
    public Color pureColor = Color.white;
    void Update()
    {
        Vector3 pos = gameObject.transform.parent.position + offset;
        pos.z = 0;
        gameObject.transform.position = pos;

        Vector3 tempRot = gameObject.transform.localEulerAngles;
        tempRot.z += Time.deltaTime * rotateSpeed;
        gameObject.transform.localEulerAngles = tempRot;

        gameObject.transform.localScale = Vector3.one * scaleValue + Vector3.one * scaleChangeValue * Mathf.Sin(Time.time);

        if (boss == null)
            return;
        int maxHp = boss.GetComponent<HeartBoss>().maxHp;
        int hp = boss.GetComponent<HeartBoss>().hp;
        gameObject.GetComponent<SpriteRenderer>().color = pureColor * ((maxHp - hp) * 1.0f / maxHp) + evilColor * (hp * 1.0f / maxHp);
    }
}
