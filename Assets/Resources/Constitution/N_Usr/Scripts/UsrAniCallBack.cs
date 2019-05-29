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

    public AudioClip audioLand;
    public AudioClip audioJump;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        state = GetComponent<UsrState>();
        AnimationEvent jumpEndEvent = new AnimationEvent {
            time = jump.length,
            functionName = "JumpEnd"
        };
        AnimationEvent jumpStartEvent = new AnimationEvent {
            time = 0,
            functionName = "JumpStart"
        };
        AnimationEvent climbingJumpEndEvent = new AnimationEvent {
            time = climbingJump.length,
            functionName = "ClimbingJumpEnd"
        };
        AnimationEvent climbingJumpStartEvent = new AnimationEvent {
            time = 0,
            functionName = "ClimbingJumpStart"
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
        jump.AddEvent(jumpStartEvent);
        climbingJump.AddEvent(climbingJumpEndEvent);
        climbingJump.AddEvent(climbingJumpStartEvent);
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

    private void JumpStart() {
        if (!state.audioSource.isPlaying) {
            state.audioSource.volume = 0.1f;
            state.audioSource.clip = audioJump;
            state.audioSource.Play();
        }
    }

    private void ClimbingJumpEnd() {
        rigidbody2D.velocity = new Vector2(state.climbJumpSpeed * -state.currentDir.x, 0);
        rigidbody2D.AddForce(new Vector2(0, 1) * state.jumpForceValue, ForceMode2D.Impulse);
    }

    private void ClimbingJumpStart() {
        animator.SetBool("isJump", false);
        animator.SetBool("isClimb", false);
        if (!state.audioSource.isPlaying) {
            state.audioSource.volume = 0.1f;
            state.audioSource.clip = audioJump;
            state.audioSource.Play();
        }
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
        MapDoor mapDoor= GetComponent<UsrControl>().dashTarget;
        if (mapDoor == null||Mathf.Abs(transform.position.y-mapDoor.transform.position.y)>state.dashOffset) {
            state.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("isDash", false);
            state.isOnMove = false;
            state.rigidbody2D.velocity = Vector2.zero;
            return;
        }
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + state.boxCollider2D.offset;
        Vector2 endColliderPos = colliderPos + state.currentDir * mapDoor.dashDistance - new Vector2(0, -0.1f);
        Collider2D hit = Physics2D.OverlapBox(endColliderPos, state.boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit == null) {//瞬移目标位置无碰撞
            transform.Translate(state.currentDir * mapDoor.dashDistance, Space.Self);
            mapDoor.SetDash(state);
        }
        else {
            state.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("isDash",false);
        }
        state.isOnMove = false;
        state.rigidbody2D.velocity = Vector2.zero;
    }

    private void LandEnd() {
        state.isOnMove = false;
    }

    private void LandStart() {
        GetComponent<UsrControl>().LimitV();
        if (!state.audioSource.isPlaying) {
            state.audioSource.volume = 0.4f;
            state.audioSource.clip = audioLand;
            state.audioSource.Play();
        }
    }
}
