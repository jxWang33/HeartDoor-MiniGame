using UnityEngine;
using System.Collections;

public class TestUsrAniCallback : MonoBehaviour
{

    private new Rigidbody2D rigidbody2D;
    private Animator animator;

    public AnimationClip jump;
    public AnimationClip climbingJump;
    public AnimationClip idleBreathe;
    public AnimationClip idleBlink;

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
        jump.AddEvent(jumpEndEvent);
        climbingJump.AddEvent(climbingJumpEndEvent);
        idleBreathe.AddEvent(idleBreatheEndEvent);
        idleBlink.AddEvent(idleBlinkEndEvent);
    }

    public void JumpEnd() {
        animator.SetBool("isJump", false);
        Vector2 forceDir = new Vector2(0, 1);
        rigidbody2D.AddForce(forceDir * GetComponent<TestControl>().jumpForceValue, ForceMode2D.Impulse);
    }
    public void ClimbingJumpEnd() {
        animator.SetBool("isJump", false);
    }
    public void IdleBreatheEnd() {
        animator.SetBool("isIdleBreathe", false);
    }
    public void IdleBlinkEnd() {
        animator.SetBool("isIdleBlink", false);
    }
}