using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandR : MonoBehaviour
{
    public float floatSpeed = 1.0f;
    public float floatOffset = 0.0f;
    public float floatDistance = 0.1f;

    public bool isAttack = false;
    public Vector2 attackDir;
    public float attackSpeed = 3;
    public AnimatorStateInfo currentAni;
    public Animator animator;
    public Vector2 screenPos;

    public Vector3 centerPosition;
    public HeartBoss heartBoss;
    public AnimationClip attackOn;
    public AnimationClip disappear;

    void Awake() {
        animator = GetComponent<Animator>();
        centerPosition = transform.position;

        attackOn.events = null;
        AnimationEvent attackOnEndEvent = new AnimationEvent {
            time = attackOn.length,
            functionName = "AttackOnEnd"
        };
        attackOn.AddEvent(attackOnEndEvent);

        disappear.events = null;
        AnimationEvent disappearEndEvent = new AnimationEvent {
            time = disappear.length,
            functionName = "DisappearOnEnd"
        };
        disappear.AddEvent(disappearEndEvent);
    }


    void Update() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);

        if (currentAni.IsName("HandRightAppear") || currentAni.IsName("HandRightReady")) {
            centerPosition.y = heartBoss.transform.position.y;
            Vector3 tempPos = transform.position;
            tempPos.y = Mathf.Sin(Time.time * floatSpeed + floatOffset) * floatDistance + centerPosition.y;
            transform.position = tempPos;
        }

        if (isAttack) {
            screenPos += attackDir * attackSpeed * Time.deltaTime;
            Vector2 tempV = Camera.main.ScreenToWorldPoint(screenPos);
            transform.localPosition = tempV;
            //transform.Translate(attackDir * attackSpeed * Time.deltaTime, Space.Self);
            if (Mathf.Abs(transform.position.x) > 100) {
                isAttack = false;
                animator.SetBool("isDead", true);
            }
        }
    }

    void DisappearOnEnd() {
        Destroy(gameObject);
    }

    void AttackOnEnd() {
        attackDir = heartBoss.usrState.transform.position - transform.position;
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        isAttack = true;
        transform.parent = null;
    }
    

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isAttack) {
            if (collision.GetComponent<UsrState>() != null) {
                collision.GetComponent<UsrState>().Hurted(attackDir, 25);
                isAttack = false;
                animator.SetBool("isDead", true);
            }
        }
    }
}
