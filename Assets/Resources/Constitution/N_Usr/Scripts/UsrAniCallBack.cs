using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsrAniCallBack : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private UsrState state;

    public AnimationClip jump;
    public AnimationClip climbingJump;
    public AnimationClip idleBreathe;
    public AnimationClip idleBlink;
    public AnimationClip slideOn;
    public AnimationClip slideOff;
    public AnimationClip dashOn; 
    public AnimationClip land;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        state = GetComponent<UsrState>();
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
        AnimationEvent slideOffEndEvent = new AnimationEvent {
            time = slideOff.length,
            functionName = "SlideOffEnd"
        };
        AnimationEvent slideOffStartEvent = new AnimationEvent {
            time = 0,
            functionName = "SlideOffStart"
        };
        AnimationEvent dashOnEndEvent = new AnimationEvent {
            time = dashOn.length,
            functionName = "DashOnEnd"
        };
        AnimationEvent landEndEvent = new AnimationEvent {
            time = land.length,
            functionName = "LandEnd"
        };
        AnimationEvent landStartEvent = new AnimationEvent {
            time = 0,
            functionName = "LandStart"
        };
        jump.AddEvent(jumpEndEvent);
        climbingJump.AddEvent(climbingJumpEndEvent);
        idleBreathe.AddEvent(idleBreatheEndEvent);
        idleBlink.AddEvent(idleBlinkEndEvent);
        slideOn.AddEvent(slideOnEndEvent);
        slideOff.AddEvent(slideOffEndEvent);
        slideOff.AddEvent(slideOffStartEvent);
        dashOn.AddEvent(dashOnEndEvent);
        land.AddEvent(landEndEvent);
        land.AddEvent(landStartEvent);
    }

    private void JumpEnd() {
        animator.SetBool("isJump", false);
        Vector2 forceDir = new Vector2(0, 1);
        rigidbody2D.AddForce(forceDir * state.jumpForceValue, ForceMode2D.Impulse);
    }

    private void ClimbingJumpEnd() {
        if (state.OnClimb == UsrState.LEFT_DIR) {
            rigidbody2D.velocity = new Vector2(state.climbJumpSpeed, 0);
            rigidbody2D.AddForce(new Vector2(0, 1) * state.jumpForceValue, ForceMode2D.Impulse);
        }
        if (state.OnClimb == UsrState.RIGHT_DIR) {
            rigidbody2D.velocity = new Vector2(-state.climbJumpSpeed, 0);
            rigidbody2D.AddForce(new Vector2(0, 1) * state.jumpForceValue, ForceMode2D.Impulse);
        }
        GetComponent<UsrControl>().ChangeControlMode(ControlMode.COMMON);
        animator.SetBool("isJump", false);
    }

    private void IdleBreatheEnd() {
        animator.SetBool("isIdleBreathe", false);
    }

    private void IdleBlinkEnd() {
        animator.SetBool("isIdleBlink", false);
    }

    private void SlideOnEnd() {
        rigidbody2D.velocity = state.slideSpeed * state.currentDir;
        state.isOnMove = false;
    }

    private void SlideOffEnd() {
        GetComponent<UsrControl>().LimitV();
    }

    private void SlideOffStart() {
        rigidbody2D.velocity = Vector2.zero;
    }

    private void DashOnEnd() {
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + state.boxCollider2D.offset;
        Vector2 endColliderPos = colliderPos + state.currentDir * state.dashDistance - new Vector2(0, -0.1f);
        Collider2D hit = Physics2D.OverlapBox(endColliderPos, state.boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit == null) {
            transform.Translate(state.currentDir * state.dashDistance, Space.Self);
            state.GetDoor().SetDash(state);
        }
        else
            Debug.Log(hit.name);
        state.isOnMove = false;
        state.rigidbody2D.velocity = Vector2.zero;
    }

    private void LandEnd() {
        state.isOnMove = false;
    }

    private void LandStart() {
        GetComponent<UsrControl>().LimitV();
    }
}
