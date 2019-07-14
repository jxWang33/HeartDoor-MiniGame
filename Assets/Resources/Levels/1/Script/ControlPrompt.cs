using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TextMesh))]
public class ControlPrompt : MonoBehaviour
{
    TextMesh mText;
    public UsrState usrState;
    public enum PromptKind {
        Move,Jump,Dash,Climb, Door, Slide, ClimbingJump
    };
    public PromptKind mKind;

    void Start()
    {
        mText = GetComponent<TextMesh>();
        switch (mKind) {
            case PromptKind.Move:
                mText.text = "< >\n" + GMManager.LEFT_KEY + GMManager.RIGHT_KEY ;
                break;
            case PromptKind.Jump:
                mText.text = "^\n" + GMManager.JUMP_KEY;
                break;
            case PromptKind.Dash:
                mText.text = "   向门移动\n并按下 " + GMManager.DASH_KEY + " 键";
                break;
            case PromptKind.Climb:
                Color tempColor = mText.color;
                tempColor.a = 0;
                mText.color = tempColor;
                break;
            case PromptKind.Door:
                mText.text = "拾取后在近处无障碍时\n按 "+ GMManager.DOOR_KEY.ToString() + " 键创建/回收 门";
                break;
            case PromptKind.Slide:
                mText.text = GMManager.DOWN_KEY + "↓";
                break;
            case PromptKind.ClimbingJump:
                mText.text = GMManager.DASH_KEY + " >";
                break;
        }
    }
    private void Update() {
        if (!usrState)
            return;
        if (mKind == PromptKind.Climb) {
            Color tempColor = mText.color;
            if (Vector2.Distance(usrState.transform.position, transform.position) < 1)
                tempColor.a += Time.deltaTime;            
            else
                tempColor.a -= 2 * Time.deltaTime;
            tempColor.a = Mathf.Clamp(tempColor.a, 0, 1);
            mText.color = tempColor;
        }
    }
}
