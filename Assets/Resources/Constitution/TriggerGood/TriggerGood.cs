using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerGood : MonoBehaviour
{
    public UsrState usrState;
    public UIManager uiManager;

    public bool needInquire = false;
    public bool isInquired = false;

    private BoxCollider2D boxCollider2D;
    private TextMesh text;
    public float textColorAShowSpeed = 1f;
    public float textColorADisappearSpeed =2f;
    private float textColorA;

    private bool inPlace = false;

    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("interact");
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        text = GetComponentInChildren<TextMesh>();
        text.text = GMManager.UP_KEY.ToString();
        text.color = new Color(1, 1, 1, 0);
        text.transform.localPosition = new Vector3(0, 1.5f, 0);
    }

    protected virtual void Update()
    {
        if (inPlace && needInquire) {
            textColorA += textColorAShowSpeed * Time.deltaTime;
        }
        else
            textColorA -= textColorADisappearSpeed * Time.deltaTime;
        textColorA =  Mathf.Clamp(textColorA, 0, 1);
        Color tempColor = text.color;
        tempColor.a = textColorA;
        text.color = tempColor;

        if (inPlace && !needInquire && !isInquired) {
            if (!uiManager.dialoguePanel.gameObject.activeSelf) {
                TriggerCallBack();
                isInquired = true;
            }
        }
        else if (inPlace && needInquire && !isInquired) {
            if (Input.GetKeyDown(GMManager.UP_KEY) && !uiManager.dialoguePanel.gameObject.activeSelf) {
                TriggerCallBack();
                isInquired = true;
            }
        }
        else if (inPlace && needInquire && isInquired) {
            if (Input.GetKeyDown(GMManager.UP_KEY)&& !uiManager.dialoguePanel.gameObject.activeSelf) {
                usrState.GetComponent<UsrControl>().Stand();
                List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("已经没什么好调查的了", "小N", null)
                };
                uiManager.SetDialogues(temp, () => {
                    usrState.isControlEnable = true;
                });
            }
        }
    }

    protected virtual void TriggerCallBack() {
        Debug.Log("inquring");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<UsrState>()) {
            inPlace = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<UsrState>())
            inPlace = false;
    }
}
