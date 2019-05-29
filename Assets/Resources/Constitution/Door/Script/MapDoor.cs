﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDoor : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    public AnimatorStateInfo currentAni;
    public float dashTime = 10f;
    private float dashCounter;
    public float dashDistance = 2.0f;

    public float openTime = 10f;
    private float openCounter;
    [SerializeField]
    private float aSpeed=2f;

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
            GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, aSpeed * Time.deltaTime);
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
        record.rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        record.rigidbody2D.velocity = record.currentDir;
    }
}
