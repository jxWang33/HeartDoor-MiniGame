using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsrAniCallBack : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    public AnimationClip jump;
    public AnimationClip climbingJump;
    public AnimationClip idleBreathe;
    public AnimationClip idleBlink;
    public AnimationClip slideOn;
    public AnimationClip dashOn;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        AnimationEvent jumpEndEvent = new AnimationEvent {
            time = jump.length,
            functionName = "JumpEnd"
        };
        AnimationEvent climbingJumpEndEvent = new AnimationEvent {
            time = climbingJump.length,
            functionName = "ClimbingJumpEnd"
        };
        AnimationEvent idleBreatheEndEvent = new AnimationEvent {
            time = idleBreathe.length,
            functionName = "IdleBreatheEnd"
        };
        AnimationEvent idleBlinkEndEvent = new AnimationEvent {
            time = idleBlink.length,
            functionName = "IdleBlinkEnd"
        };
        AnimationEvent slideOnEndEvent = new AnimationEvent {
            time = slideOn.length,
            functionName = "SlideOnEnd"
        };
        AnimationEvent dashOnEndEvent = new AnimationEvent {
            time = dashOn.length,
            functionName = "DashOnEnd"
        };
        jump.AddEvent(jumpEndEvent);
        climbingJump.AddEvent(climbingJumpEndEvent);
        idleBreathe.AddEvent(idleBreatheEndEvent);
        idleBlink.AddEvent(idleBlinkEndEvent);
        slideOn.AddEvent(slideOnEndEvent);
        dashOn.AddEvent(dashOnEndEvent);
    }

    public void JumpEnd() {
        animator.SetBool("isJump", false);
        Vector2 forceDir = new Vector2(0, 1);
        rigidbody2D.AddForce(forceDir * GetComponent<UsrState>().jumpForceValue, ForceMode2D.Impulse);
    }
    public void ClimbingJumpEnd() {
        if (GetComponent<UsrState>().OnClimb == UsrState.LEFT_DIR) {
            rigidbody2D.velocity = new Vector2(GetComponent<UsrState>().climbJumpSpeed, 0);
            rigidbody2D.AddForce(new Vector2(0, 1) * GetComponent<UsrState>().jumpForceValue, ForceMode2D.Impulse);
        }
        if (GetComponent<UsrState>().OnClimb == UsrState.RIGHT_DIR) {
            rigidbody2D.velocity = new Vector2(-GetComponent<UsrState>().climbJumpSpeed, 0);
            rigidbody2D.AddForce(new Vector2(0, 1) * GetComponent<UsrState>().jumpForceValue, ForceMode2D.Impulse);
        }
        GetComponent<UsrControl>().ChangeControlMode(ControlMode.COMMON);
        animator.SetBool("isJump", false);
    }
    public void IdleBreatheEnd() {
        animator.SetBool("isIdleBreathe", false);
    }
    public void IdleBlinkEnd() {
        animator.SetBool("isIdleBlink", false);
    }
    public void SlideOnEnd() {
        rigidbody2D.velocity = GetComponent<UsrState>().slideSpeed * GetComponent<UsrState>().currentDir;
        GetComponent<UsrState>().isOnMove = false;
    }
    public void DashOnEnd() {
        animator.SetBool("isIdleBlink", false);
    }
}
