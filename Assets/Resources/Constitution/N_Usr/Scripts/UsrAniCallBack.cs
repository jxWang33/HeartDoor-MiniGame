using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UsrState))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class UsrAniCallBack : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private AudioSource audioSource;
    private Animator animator;
    private UsrState state;

    [Space()]
    [Header("响应片段")]
    public AnimationClip jump;
    public AnimationClip climbingJump;
    public AnimationClip idleBreathe;
    public AnimationClip idleBlink;
    public AnimationClip slideOn;
    public AnimationClip slideOff;
    public AnimationClip dashOn; 
    public AnimationClip land;

    [Space()]
    [Header("音频源")]
    public AudioClip audioRunL;
    public AudioClip audioRunR;
    public AudioClip audioIdle;
    public AudioClip audioLand;
    public AudioClip audioJump;
    public AudioClip audioHurt;
    public AudioClip audioGotKey;
    public AudioClip audioError;

    void Awake() {
        state = GetComponent<UsrState>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        jump.events = null;
        climbingJump.events = null;
        idleBreathe.events = null;
        idleBlink.events = null;
        slideOn.events = null;
        slideOff.events = null;
        dashOn.events = null;
        land.events = null;


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

    #region CALLBACK

    private void JumpEnd() {
        animator.SetBool("isJump", false);
        Vector2 forceDir = new Vector2(0, 1);
        rigidbody2D.AddForce(forceDir * state.jumpForceValue, ForceMode2D.Impulse);
        state.isOnMove = false;
    }

    private void JumpStart() {
        SoundPlay(audioJump, true, 0.4f);
    }

    private void ClimbingJumpEnd() {
        rigidbody2D.velocity = new Vector2(state.climbJumpSpeed * -state.currentDir.x, 0);
        rigidbody2D.AddForce(new Vector2(0, 1) * state.jumpForceValue, ForceMode2D.Impulse);
        GetComponent<UsrControl>().ChangeControlMode(ControlMode.COMMON);
        SoundPlay(audioJump, true, 0.4f);
    }

    private void ClimbingJumpStart() {
        animator.SetBool("isJump", false);
        animator.SetBool("isClimb", false);
    }

    private void IdleBreatheEnd() {
        animator.SetBool("isIdleBreathe", false);
    }

    private void IdleBlinkEnd() {
        animator.SetBool("isIdleBlink", false);
    }

    private void SlideOnEnd() {
        rigidbody2D.velocity = state.maxSlideSpeed * state.currentDir;
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
        if (mapDoor == null||Mathf.Abs(transform.position.y-mapDoor.transform.position.y)>state.doorDistance) {
            state.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("isDash", false);
            state.isOnMove = false;
            state.rigidbody2D.velocity = Vector2.zero;
            return;
        }
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + state.boxCollider2D.offset;
        Vector2 endColliderPos = colliderPos + state.currentDir * mapDoor.dashDistance - new Vector2(0, -0.1f);
        Collider2D hit = Physics2D.OverlapBox(endColliderPos, state.boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit == null) {//瞬移目标位置无碰撞
            transform.Translate(state.currentDir * mapDoor.dashDistance, Space.Self);
            mapDoor.SetDash(state);
        }
        else {
            ErrorSound();
            state.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            state.CheckDoor(mapDoor);
            mapDoor.DisAppear();
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
        SoundPlay(audioLand, true, 0.4f);
    }

    #endregion
    private void SoundPlay(AudioClip ac, bool forceChange = false, float vol = 1.0f) {
        if (!audioSource.isPlaying || forceChange) {
            audioSource.volume = vol;
            audioSource.clip = ac;
            audioSource.Play();
        }
    }

    private void UpdateSound() {//帧关联音效
        if (GetComponent<SpriteRenderer>().sprite.name == "run_2" && state.isOnGround) {
            SoundPlay(audioRunL, false, 0.4f);
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "run_6" && state.isOnGround) {
            SoundPlay(audioRunR, false, 0.4f);
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "idle_2" && state.isOnGround) {
            SoundPlay(audioIdle);
        }
    }

    public void HurtSound() {
        SoundPlay(audioHurt, true, 0.5f);
    }

    public void ErrorSound() {
        SoundPlay(audioError);
    }
    public void GotKeySound() {
        SoundPlay(audioGotKey, true, 0.5f);
    }

    private void FixedUpdate() {
        UpdateSound();
    }
}
