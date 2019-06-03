using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsrStatePanel : MonoBehaviour
{
    #region MANUAL
    public Transform manualTrans;
    public Image manualImage;

    private Vector3 currentManualPos = Vector3.zero;
    private float currentManualRemain = 0;//0-1
    [SerializeField]
    private float realManualRemain = 1;//0-1
    private float manualRotateSpeed = 2;

    private float manualRemainTime = 1;
    private float manualRemainCounter = 0;

    private float manualDistance = 0.5f;
    #endregion

    #region MENTAL
    public Image mentalImage;
    public Text mentalText;
    public Text heartText;
    #endregion

    #region KEY
    public Transform goldenKey;
    public Transform greenKey;
    public Image goldenKeyImage;
    public UIGreenKey greenKeyUI;
    #endregion

    void Awake()
    {

    }
    
    void Update()
    {
        UpdateManualBar();
    }

    private void UpdateManualBar() {
        realManualRemain = Mathf.Clamp(realManualRemain, 0, 1);
        manualTrans.position = currentManualPos;
        manualImage.fillAmount = currentManualRemain;

        if (currentManualRemain != realManualRemain) {
            manualRemainCounter = manualRemainTime;
            if (!manualTrans.gameObject.activeSelf)
                manualTrans.gameObject.SetActive(true);
        }
        else {
            manualRemainCounter -= Time.deltaTime;
            if (manualRemainCounter <= 0) {
                manualRemainCounter = 0;
                if (manualTrans.gameObject.activeSelf)
                    manualTrans.gameObject.SetActive(false);
            }
        }

        if (currentManualRemain < realManualRemain) {
            currentManualRemain += manualRotateSpeed * Time.deltaTime;
            if (currentManualRemain > realManualRemain)
                currentManualRemain = realManualRemain;
        }
        if (currentManualRemain > realManualRemain) {
            currentManualRemain -= manualRotateSpeed * Time.deltaTime;
            if (currentManualRemain < realManualRemain)
                currentManualRemain = realManualRemain;
        }
    }

    public void SetManualValue(Vector3 pos, float value) {
        realManualRemain = value;
        Vector2 finalPos = pos - new Vector3(manualDistance, 0, 0);
        Vector2 scPos = Camera.main.WorldToScreenPoint(finalPos);
        if (scPos.x < 0)
            scPos = Camera.main.WorldToScreenPoint(pos + new Vector3(manualDistance, 0, 0));
        currentManualPos = scPos;
    }

    public void SetMentalValue(float currentValue, float maxValue = 100, int heartCount = -1) {
        if (heartCount >= 0)
            heartText.text = heartCount.ToString();

        mentalText.text = currentValue.ToString();
        float value = currentValue / maxValue;
        value = Mathf.Clamp(value, 0, 1);
        mentalImage.fillAmount = value;
    }

    public void SetGoldenKey(float goldenKeyRemain = 0) {//update time
        goldenKeyRemain = Mathf.Clamp(goldenKeyRemain, 0, 1);
        if (goldenKeyRemain > 0) {
            if (!goldenKey.gameObject.activeSelf)
                goldenKey.gameObject.SetActive(true);
            if (greenKey.gameObject.activeSelf)
                greenKey.gameObject.SetActive(false);
            goldenKeyImage.fillAmount = goldenKeyRemain;
        }
        else {
            if (goldenKey.gameObject.activeSelf)
                goldenKey.gameObject.SetActive(false);
            if (!greenKey.gameObject.activeSelf)
                greenKey.gameObject.SetActive(true);
            goldenKeyImage.fillAmount = 0;
        }
    }

    public void SetGreenKey(int pre,int current) {
        if (current == pre)
            return;
        else if (current - pre == 1)
            greenKeyUI.Add();
        else if (current - pre == -1)
            greenKeyUI.Dec();
        else
            greenKeyUI.Set(current);
    }
}
