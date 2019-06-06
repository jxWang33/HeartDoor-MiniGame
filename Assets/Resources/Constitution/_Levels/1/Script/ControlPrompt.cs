using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TextMesh))]
public class ControlPrompt : MonoBehaviour
{
    TextMesh mText;
    public enum PromptKind {
        Move,Jump,Dash,Climb, Door, Slide
    };
    public PromptKind mKind;
    // Start is called before the first frame update
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
                mText.text = GMManager.RIGHT_KEY +" + "+ GMManager.DASH_KEY;
                break;
            case PromptKind.Climb:
                mText.text = "     < " + GMManager.JUMP_KEY+ "\n" + GMManager.JUMP_KEY + "+" + GMManager.RIGHT_KEY + " >";
                break;
            case PromptKind.Door:
                mText.text = GMManager.DOOR_KEY.ToString();
                break;
            case PromptKind.Slide:
                mText.text = GMManager.DOWN_KEY + "↓";
                break;
        }
    }
}
