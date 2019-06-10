using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumbFish : MonoBehaviour
{
    public Animator animator;
    public new Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;

    public AnimatorStateInfo currentAni;
    public Vector2 currentDir;

    [SerializeField]
    private float maxMoveSpeed = 2;
    [SerializeField]
    private float moveForceValue = 5;
    [SerializeField]
    private float airDragA = 0.5f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        
    }
    
    void Update()
    {
        UpdateAni();
        AIControl();
        SetF();
    }

    void AIControl() {
        Vector2 colliderPos = (Vector2)transform.position + boxCollider2D.offset + new Vector2(0.2f, 0.2f);
        Collider2D hit = Physics2D.OverlapBox(colliderPos, boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));//站起碰撞盒检测
        if (hit == null) {
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
        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxMoveSpeed, maxMoveSpeed), 0);
    }

    public void Dead() {
        animator.SetBool("isDead", true);
        rigidbody2D.Sleep();
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.GetComponent<DoorSwitch>() != null) {
            collision.transform.GetComponent<DoorSwitch>().Change();
        }
    }
}
