using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;
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

    void Awake() {
        gameObject.layer = LayerMask.NameToLayer("door");
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dashCounter = dashTime;
        openCounter = openTime;
        open.events = null;
        close.events = null;
        AnimationEvent OpenEndEvent = new AnimationEvent {
            time = open.length,
            functionName = "OpenEnd"
        };
        open.AddEvent(OpenEndEvent);

        AnimationEvent CloseEndEvent = new AnimationEvent {
            time = close.length,
            functionName = "CloseEnd"
        };
        close.AddEvent(CloseEndEvent);
    }
    
    void Update() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAni.IsName("idle"))
            animator.SetBool("isClose", false);

        UpdateColorA();
        UpdateOpen();
        UpdateDash();
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
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        animator.SetBool("isDash",true);

        boxCollider2D.isTrigger = true;

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
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        record.rigidbody2D.velocity = record.currentDir;
        boxCollider2D.isTrigger = false;
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.GetComponent<EasyBoss>() != null) {
            collision.transform.GetComponent<EasyBoss>().ChangeHp(-20);
            collision.transform.GetComponent<EasyBoss>().Hurted(dashDir);
        }
    }
}
