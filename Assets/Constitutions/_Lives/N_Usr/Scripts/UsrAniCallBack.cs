using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UsrState))]
[RequireComponent(typeof(Rigidbody2D))]

public class UsrAniCallBack : MonoBehaviour
{
    const int AUDIO_RUN = 0;
    const int AUDIO_IDLE = 0;
    const int AUDIO_JUMP = 1;
    const int AUDIO_LAND = 1;
    const int AUDIO_SLIDE = 2;
    const int AUDIO_GOTKEY = 3;
    const int AUDIO_HURT = 4;
    const int AUDIO_ERROR = 5;

    public GameObject audioObject;
    private AudioSource[] audioSources;
    private new Rigidbody2D rigidbody2D;
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
    public AudioClip audioSlide;

    void Awake() {
        state = GetComponent<UsrState>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSources = audioObject.GetComponents<AudioSource>();

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
        SoundPlay(audioJump, AUDIO_JUMP, 0.4f);
    }

    private void ClimbingJumpEnd() {
        rigidbody2D.velocity = new Vector2(state.climbJumpSpeed * -state.currentDir.x, 0);
        rigidbody2D.AddForce(new Vector2(0, 1) * state.jumpForceValue, ForceMode2D.Impulse);
        GetComponent<UsrControl>().ChangeControlMode(ControlMode.COMMON);
        SoundPlay(audioJump, AUDIO_JUMP, 0.4f);
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
        SoundPlay(audioSlide,AUDIO_SLIDE);
        rigidbody2D.velocity = state.maxSlideSpeed * state.currentDir;
        state.isOnMove = false;
    }

    private void SlideOffEnd() {
    }

    private void SlideOffStart() {
        state.rigidbody2D.velocity = state.currentDir * state.slideOffSpeed;
    }

    private void DashOnEnd() {
        MapDoor mapDoor= GetComponent<UsrControl>().dashTarget;
        if (mapDoor == null||Mathf.Abs(transform.position.y-mapDoor.transform.position.y)>state.doorDistance) {//y轴再次确认
            DashFail();
            return;
        }
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + state.boxCollider2D.offset;
        Vector2 endColliderPos = colliderPos + state.currentDir * mapDoor.dashDistance - new Vector2(0, -0.1f);
        Collider2D hit = Physics2D.OverlapBox(endColliderPos, state.boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit == null || hit.GetComponent<PlatformEffector2D>())  {//瞬移目标位置无碰撞
            mapDoor.SetDash(state);

            Transform tempTarget = transform.Find("Camera Target");
            tempTarget.parent = mapDoor.transform;
            tempTarget.localPosition = state.startCameraTargetLocalPos;
        }
        else {
            state.SafeRealseDoor(mapDoor);
            mapDoor.DisAppear();
            DashFail();
        }
        state.isOnMove = false;
    }

    private void LandEnd() {
        state.isOnMove = false;
    }

    private void LandStart() {
        GetComponent<UsrControl>().LimitHorV();
        SoundPlay(audioLand, AUDIO_LAND, 0.4f);
        state.dustEffect.SetLandEffect();
        state.dustEffect.Invoke("CancelLandEffect", 0.1f);
    }

    #endregion

    private void SoundPlay(AudioClip ac, int source, float vol = 1.0f) {
        if (!audioObject)
            return;

        if (!audioSources[source].isPlaying) {
            audioSources[source].clip = ac;
            audioSources[source].volume = vol;
            audioSources[source].Play();
        }
    }

    private void UpdateSound() {//帧关联音效
        if (GetComponent<SpriteRenderer>().sprite.name == "run_2" && state.isOnGround) {
            SoundPlay(audioRunL, AUDIO_RUN, 0.4f);
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "run_6" && state.isOnGround) {
            SoundPlay(audioRunR, AUDIO_RUN, 0.4f);
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "idle_2" && state.isOnGround) {
            SoundPlay(audioIdle,AUDIO_IDLE);
        }
    }

    private void DashFail() {
        ErrorSound();
        animator.SetBool("isDash", false);
        state.isOnMove = false;
        state.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void HurtSound() {
        SoundPlay(audioHurt, AUDIO_HURT, 0.5f);
    }

    public void ErrorSound() {
        SoundPlay(audioError,AUDIO_ERROR);
    }

    public void GotKeySound() {
        SoundPlay(audioGotKey, AUDIO_GOTKEY, 0.5f);
    }

    private void FixedUpdate() {
        UpdateSound();
    }
}
