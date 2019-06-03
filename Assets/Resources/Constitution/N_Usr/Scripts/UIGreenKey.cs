using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGreenKey : MonoBehaviour
{
    public GameObject pbGreenKeyUI;
    public int currentNum = 0;
    public int maxNum=5;

    private Vector2 startPos = new Vector2(-100, 0);
    private float posInter = 50;

    void Awake()
    {
    }

    public void Set(int num) {
        while (currentNum > 0) {
            Dec();
        }

        for (int i = 0; i < num; i++) {
            Add();
        }
    }

    public void Add() {
        if (currentNum >= maxNum)
            return;
        GameObject temp = Instantiate(pbGreenKeyUI, transform);
        temp.transform.localPosition = new Vector2(currentNum * posInter, 0) + startPos;
        currentNum++;
    }

    public void Dec() {
        if (currentNum <= 0)
            return;
        Destroy(transform.GetChild(--currentNum).gameObject);
    }
}
