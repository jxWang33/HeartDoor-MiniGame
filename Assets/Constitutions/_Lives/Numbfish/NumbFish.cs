using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumbFish : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    new Rigidbody2D rigidbody2D;

    public Vector2 currentDir;
    public AnimatorStateInfo currentAni;
    
    public float maxMoveSpeed = 2;
    public float moveForceValue = 5;
    public float airDragA = 0.5f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();        
    }

    private void FixedUpdate() {
        UpdateAni();
        AIControl();
        SetF();
    }

    void AIControl() {
        Vector2 colliderPos = (Vector2)transform.position + boxCollider2D.offset + new Vector2(0.2f, 0.2f);
        Collider2D hit = Physics2D.OverlapBox(colliderPos, boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit == null && !animator.GetBool("isDead")) {
            Move();
        }
    }

    void UpdateAni() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);
        if (currentDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (currentDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);
        animator.SetBool("isMove", false);
    }

    private void SetF() {
        if (rigidbody2D.velocity.x > 0) {
            rigidbody2D.velocity -= new Vector2(airDragA, 0);
            if (rigidbody2D.velocity.x < 0)
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
        else if (rigidbody2D.velocity.x < 0) {
            rigidbody2D.velocity += new Vector2(airDragA, 0);
            if (rigidbody2D.velocity.x > 0)
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
    }

    #region ACT 

    private void Move() {
        animator.SetBool("isMove", true);
        rigidbody2D.AddForce(currentDir* moveForceValue, ForceMode2D.Force);
        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxMoveSpeed, maxMoveSpeed), rigidbody2D.velocity.y);
    }

    public void Dead() {
        animator.SetBool("isDead", true);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.GetComponent<DoorSwitch>() != null) {
            collision.transform.GetComponent<DoorSwitch>().Change();
        }
    }
}
