using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    public AnimatorStateInfo currentAni;
    public float dashTime = 10f;
    private float dashCounter;

    public float openTime = 10f;
    private float openCounter;

    private float dashSpeed = 0;
    private Vector2 dashDir = Vector2.zero;

    private UsrState record;
    public AnimationClip open;

    void Awake() {
        gameObject.layer = LayerMask.NameToLayer("door");
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dashCounter = dashTime;
        openCounter = openTime;

        AnimationEvent OpenEndEvent = new AnimationEvent {
            time = open.length,
            functionName = "OpenEnd"
        };
        open.AddEvent(OpenEndEvent);
    }
    
    void Update() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);
        if (currentAni.IsName("idle"))
            animator.SetBool("isClose", false);

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

    public void SetDash(UsrState state) {
        record = state;
        dashCounter = 0;
        dashDir = state.currentDir;
        dashSpeed = (state.dashDistance-boxCollider2D.size.x-state.startColliderSize.x) / dashTime;
        animator.SetBool("isDash",true);

        if (dashDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (dashDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public bool IsEnable() {
        if (currentAni.IsName("idle")) {
            return true;
        }
        return false;
    }

    private void OpenEnd() {
        openCounter = 0;
        record.animator.SetBool("isDash", false);
        record.rigidbody2D.velocity = record.currentDir;
    }
}
