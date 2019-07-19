using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class TriggerGood : MonoBehaviour
{
    public UsrState usrState;//调查发起者
    public UIManager uiManager;//控制对话

    public bool needInquire = false;
    public bool isInquired = false;

    private TextMesh textPrompt;
    public string textContent = "";
    public float textColorAShowSpeed = 1f;
    public float textColorADisappearSpeed =2f;
    private float textColorA;
    public float textAboveDistance = 1.5f;
    public Color textStartColor = Color.white;

    private bool inPlace = false;
    private BoxCollider2D boxCollider2D;

    protected virtual void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("interact");
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        textPrompt = GetComponentInChildren<TextMesh>();
        if (textContent == "")
            textPrompt.text = "按下 "+ GMManager.UP_KEY.ToString()+ " 键调查";
        else
            textPrompt.text = textContent;

        textPrompt.color = textStartColor;
        textPrompt.transform.localPosition = new Vector3(0, textAboveDistance, -5);
        textPrompt.anchor = TextAnchor.MiddleCenter;
        textPrompt.alignment = TextAlignment.Center;
    }

    protected virtual void Update()
    {
        if (inPlace && needInquire) {
            textColorA += textColorAShowSpeed * Time.deltaTime;
        }
        else
            textColorA -= textColorADisappearSpeed * Time.deltaTime;
        textColorA =  Mathf.Clamp(textColorA, 0, 1);
        Color tempColor = textPrompt.color;
        tempColor.a = textColorA;
        textPrompt.color = tempColor;

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
                    new DialogueUnit("已经没什么好调查的了", DialogueKind.Talk,()=>{
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoNSad)
                };
                uiManager.SetDialogues(temp);
            }
        }
    }

    protected abstract void TriggerCallBack();

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
