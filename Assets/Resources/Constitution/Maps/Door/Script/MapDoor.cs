using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    Animator animator;
    public BoxCollider2D boxCollider2D;
    public bool reCollectable = true;
    private AnimatorStateInfo currentAni;

    public float dashTime = 10f;
    private float dashCounter;
    public float dashDistance = 2.0f;
    public int useTime = 1;//可用次数,-10为无限

    public float openTime = 10f;
    private float openCounter;
    [SerializeField]
    private float aColorSpeed=2f;
    private float dashSpeed = 0;
    private Vector2 dashDir = Vector2.zero;

    public GameObject pbGreenKey;

    private UsrState record;
    public AnimationClip open;
    public AnimationClip close;

    public AudioClip audioOpen;
    public AudioSource audioSource;

    void Awake() {
        gameObject.layer = LayerMask.NameToLayer("door");
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        dashCounter = dashTime;
        openCounter = openTime;
        open.events = null;
        close.events = null;

        AnimationEvent OpenEndEvent = new AnimationEvent {
            time = open.length,
            functionName = "OpenEnd"
        };
        AnimationEvent OpenStartEvent = new AnimationEvent {
            time = 0,
            functionName = "OpenStart"
        };
        open.AddEvent(OpenStartEvent);
        open.AddEvent(OpenEndEvent);

        AnimationEvent CloseEndEvent = new AnimationEvent {
            time = close.length,
            functionName = "CloseEnd"
        };
        close.AddEvent(CloseEndEvent);
    }
    
    void Update() {
        UpdateAni();
        UpdateColorA();
        UpdateOpen();
        UpdateDash();
    }

    private void UpdateAni() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAni.IsName("idle"))
            animator.SetBool("isClose", false);
    }

    private void UpdateOpen() {
        if (openCounter < openTime) {
            openCounter += Time.deltaTime;
            if (openCounter >= openTime) {
                animator.SetBool("isClose", true);
            }
        }
    }

    private void UpdateDash() {
        if (dashCounter < dashTime) {
            dashCounter += Time.deltaTime;
            if (dashCounter >= dashTime) {
                float remain = dashTime - dashCounter;
                dashCounter = dashTime;
                transform.Translate(dashDir * dashSpeed * remain, Space.Self);

                animator.SetBool("isDash", false);
            }
            transform.Translate(dashDir * dashSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void UpdateColorA() {
        if (GetComponent<SpriteRenderer>().color.a < 1)
            GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, aColorSpeed * Time.deltaTime);
        else if (GetComponent<SpriteRenderer>().color.a > 1)
            SetColorA(1);
    }

    public void SetColorA(float a) {
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g,
            GetComponent<SpriteRenderer>().color.b,a);
    }

    public void SetDash(UsrState state) {
        record = state;
        dashCounter = 0;
        dashDir = state.currentDir;
        dashSpeed = (dashDistance-boxCollider2D.size.x-state.startColliderSize.x) / dashTime;

        boxCollider2D.isTrigger = true;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        animator.SetBool("isDash",true);

        if (dashDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (dashDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);

        if(useTime!=-10)
            useTime--;
    }

    public bool IsEnable() {
        if (currentAni.IsName("idle") && (useTime > 0 || useTime == -10)) {
            return true;
        }
        return false;
    }

    private void OpenEnd() {
        openCounter = 0;
        record.animator.SetBool("isDash", false);
        record.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        record.transform.Translate(record.currentDir * dashDistance, Space.Self);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        record.rigidbody2D.velocity = record.currentDir;
        boxCollider2D.isTrigger = false;

        Transform tempTarget = transform.Find("Camera Target");
        tempTarget.parent = record.transform;
        tempTarget.localPosition = record.startCameraTargetLocalPos;
    }

    private void OpenStart() {
        SoundPlay(audioOpen);
    }

    private void CloseEnd() {
        if (useTime == 0) 
            DisAppear();
    }

    public void DisAppear() {
        if (reCollectable) {
            GameObject temp = Instantiate(pbGreenKey, transform.position, Quaternion.identity);
            temp.transform.parent = GameObject.Find("MapManager").transform;
        }
        Destroy(gameObject);
    }

    private void SoundPlay(AudioClip ac, bool forceChange = false, float vol = 1.0f) {
        if (!audioSource.isPlaying || forceChange) {
            audioSource.volume = vol;
            audioSource.clip = ac;
            audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (currentAni.IsName("dash_on") || currentAni.IsName("dashing") || currentAni.IsName("open")) {
            if (collision.tag == "doorable") {
                collision.transform.GetComponentInParent<Doorable>().OnDoorHurt(dashDir);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        int contactNum = boxCollider2D.GetContacts(contacts);
        for (int i = 0; i < collision.contactCount; i++) {
            if (collision.GetContact(i).collider.GetComponent<PlatformEffector2D>() && Mathf.Abs(collision.GetContact(0).normal.y - 1) > 0.1f)
                continue;
            if (collision.GetContact(i).collider.GetComponent<UsrState>())
                continue;
            for (int j = 0; j < contactNum; j++) {
                if (contacts[j].collider.GetComponent<PlatformEffector2D>() && Mathf.Abs(contacts[j].normal.y - 1) > 0.1f)
                    continue;
                if (contacts[j].collider.GetComponent<UsrState>())
                    continue;
                float tempAngle = Vector2.Angle(collision.GetContact(i).normal, contacts[j].normal);                
                if (tempAngle >= GMManager.JAM_ANGLE) {
                    if(record)
                        record.SafeRealseDoor(this);
                    DisAppear();
                    return;
                }
            }
        }
    }
}
