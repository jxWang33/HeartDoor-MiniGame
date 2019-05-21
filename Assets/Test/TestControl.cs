using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode {
    COMMON,CLIMBING
}

[RequireComponent(typeof(Rigidbody2D))]
public class TestControl : MonoBehaviour
{
    public ControlMode currentMode;
    public Vector2 m_v;
    const float MAX_CLIMBINGINTER = 0.5f;
    public float climbingCounter = 0;
    const float MAX_IDLEINTER = 2.0f;
    public float idleCounter = 0;

    public float moveForceValue = 0.01f;
    public float jumpForceValue = 0.01f;
    public float slideForceValue = 0.02f;
    public float climbJumpForceValue = 2.0f;
    public float maxMoveSpeed = 1f;
    public float horizonA = 0.5f;
    [SerializeField]
    private bool isJumpEnable = false;
    [SerializeField]
    private bool isClimbEnable = false;
    [SerializeField]
    private bool isOnGround = false;
    [SerializeField]
    private bool isOnMove = false;
    [SerializeField]
    private bool isOnWallL = false;
    [SerializeField]
    private bool isOnWallR = false;
    [SerializeField]
    private int climbing = 0;//1->left,2->right



    private List<TestWall> touchedWallsL;
    private List<TestWall> touchedWallsR;
    private new Rigidbody2D rigidbody2D;
    private Vector2 currentDir;
    private CapsuleCollider2D capsuleCollider2D;
    private Animator animator;

    private AudioSource player;
    private AudioClip run1;
    private AudioClip run2;
    readonly System.Random rd = new System.Random(GetRandomSeed());
    void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        touchedWallsL = new List<TestWall>();
        touchedWallsR = new List<TestWall>();
        currentDir = new Vector2(0, 1);
        currentMode = ControlMode.COMMON;
        climbingCounter = MAX_CLIMBINGINTER;

        run1 = Resources.Load("run1") as AudioClip;
        run2 = Resources.Load("run2") as AudioClip;
        player = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        switch (currentMode) {
            case ControlMode.COMMON:
                UsrControlCommon();
                break;
            case ControlMode.CLIMBING:
                UsrControlClimbing();
                break;
        }
        m_v = rigidbody2D.velocity;
    }
    private void FixedUpdate() {
        UpdateSound();
        UpdateOnGround();
        UpdateOnMove();
        UpdateOnWall();
        UpdateAni();
        UpdateUsr();
        UpdateJumpEnable();
        UpdateClimbingEnable();
        Check();
    }
    private void Check() {//更新合法值
        if (rigidbody2D.velocity.x > maxMoveSpeed) {
            float tempy = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(maxMoveSpeed, tempy);
        }
        else if (rigidbody2D.velocity.x < -maxMoveSpeed) {
            float tempy = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(-maxMoveSpeed, tempy);
        }
        if (climbing < 0)
            climbing = 0;
        if (climbing > 2)
            climbing = 2;
    }

    private void UpdateJumpEnable() {
        if (climbing != 0 || isOnGround)
            isJumpEnable = true;
        else
            isJumpEnable = false;
    }
    private void UpdateClimbingEnable() {
        if (climbingCounter >= MAX_CLIMBINGINTER)
            isClimbEnable = true;
        else {
            isClimbEnable = false;
            climbingCounter += Time.deltaTime;
        }
    }

    private void UpdateOnGround() {
        Collider2D hit = Physics2D.OverlapCapsule(transform.position + new Vector3(0, -0.2f), capsuleCollider2D.size + new Vector2(-0.1f, 0.1f), capsuleCollider2D.direction, 0,
            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
        if (hit != null) {
            isOnGround = true;
        }
        else {
            isOnGround = false;
        }
    }
    private void UpdateOnMove() {
        if (Mathf.Abs(rigidbody2D.velocity.x) >= 0.0001f)
            isOnMove = true;
        else
            isOnMove = false;
    }
    private void UpdateOnWall() {
        if (touchedWallsL.Count == 0)
            isOnWallL = false;
        else
            isOnWallL = true;

        if (touchedWallsR.Count == 0)
            isOnWallR = false;
        else
            isOnWallR = true;

    }

    private void UpdateAni() {
        if (currentDir==new Vector2(1,0))
            transform.localScale = new Vector3(1,1,1);
        else if (currentDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1,1,1);

        if (Mathf.Abs(rigidbody2D.velocity.x)>0.5)
            animator.SetBool("isRun",true);
        else
            animator.SetBool("isRun", false);
        //
        if (idleCounter >= MAX_IDLEINTER) {
            int temp = rd.Next() % 2;
            if (temp == 0) {
                animator.SetBool("isIdle2", false);
                animator.SetBool("isIdle1", true);
            }
            else if (temp == 1) {
                animator.SetBool("isIdle1", false);
                animator.SetBool("isIdle2", true);
            }
            idleCounter = 0;
        }
        else if (rigidbody2D.velocity == Vector2.zero) {
            idleCounter += Time.deltaTime;
        }
        else
            idleCounter = 0;
        //
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f) {
            if (info.IsName("idle1")) {
                animator.SetBool("isIdle1", false);
            }
            if (info.IsName("idle2")) {
                animator.SetBool("isIdle2", false);
            }
            if (info.IsName("jump")&&isOnGround) {
                animator.SetBool("isJump", false);
            }
        }
    }
    private void UpdateUsr() {
        if (isOnMove) {//水平阻力
            if (rigidbody2D.velocity.x > 0) {
                rigidbody2D.velocity -= new Vector2(horizonA, 0);
                if (rigidbody2D.velocity.x < 0)
                    rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
            else if (rigidbody2D.velocity.x < 0) {
                rigidbody2D.velocity += new Vector2(horizonA, 0);
                if (rigidbody2D.velocity.x > 0)
                    rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
        }
    }
    private void UpdateSound() {
        if (GetComponent<SpriteRenderer>().sprite.name == "run_2" && isOnGround) {
            if (!player.isPlaying) {
                player.clip = run1;
                player.Play();
            }
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "run_6" && isOnGround) {
            if (!player.isPlaying) {
                player.clip = run2;
                player.Play();
            }
        }
    }

    private void ChangeControlMode(ControlMode cm, int param = 0) {
        switch (cm) {
            case ControlMode.COMMON:
                climbingCounter = 0;
                rigidbody2D.gravityScale = 1;
                climbing = param;
                break;
            case ControlMode.CLIMBING:
                rigidbody2D.velocity = new Vector2(0, -slideForceValue);
                rigidbody2D.gravityScale = 0;
                climbing = param;
                break;
        }
        currentMode = cm;
    }

    private void UsrControlCommon() {//一般玩家交互
        if (Input.GetKey(KeyCode.LeftArrow)) {//左移动
            currentDir = new Vector2(-1, 0);
            rigidbody2D.AddForce(currentDir * moveForceValue, ForceMode2D.Force);
            if (isOnWallL&& isClimbEnable) {
                ChangeControlMode(ControlMode.CLIMBING,1);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow)) {//右移动
            currentDir = new Vector2(1, 0);
            rigidbody2D.AddForce(currentDir * moveForceValue, ForceMode2D.Force);
            if (isOnWallR && isClimbEnable) {
                ChangeControlMode(ControlMode.CLIMBING,2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z)) {//跳跃
            if (isJumpEnable) {
                Vector2 forceDir = new Vector2(0, 1);
                rigidbody2D.AddForce(forceDir * jumpForceValue, ForceMode2D.Impulse);
                animator.SetBool("isJump", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.X)) {//特殊技
            if (isOnWallL) {
                Collider2D hit = Physics2D.OverlapCapsule(transform.position + new Vector3(-1.5f, 0), capsuleCollider2D.size, capsuleCollider2D.direction, 0,
                                                            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
                if (hit == null) {
                    transform.Translate(new Vector2(-1.5f, 0), Space.Self);
                }
            }
            if (isOnWallR) {
                Collider2D hit = Physics2D.OverlapCapsule(transform.position + new Vector3(1.5f, 0), capsuleCollider2D.size, capsuleCollider2D.direction, 0,
                                                            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
                if (hit == null) {
                    transform.Translate(new Vector2(1.5f, 0), Space.Self);
                }
            }
        }
    }
    private void UsrControlClimbing() {//爬墙交互
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (isOnWallL) {//左爬墙维持
                if (isOnGround) {
                    ChangeControlMode(ControlMode.COMMON);
                }
            }
            else
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetKey(KeyCode.LeftArrow)) {
            if (climbing == 1)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            if (isOnWallR) {
                if (isOnGround) {
                    ChangeControlMode(ControlMode.COMMON);
                }
            }
            else
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetKey(KeyCode.RightArrow)) {
            if (climbing == 2)
                ChangeControlMode(ControlMode.COMMON);
        }

        if (Input.GetKeyDown(KeyCode.Z)) {//跳跃
            if (isJumpEnable) {
                if (climbing == 1) {
                    rigidbody2D.AddForce(new Vector2(0, 1) * jumpForceValue, ForceMode2D.Impulse);
                    rigidbody2D.velocity += new Vector2(climbJumpForceValue,0);
                }
                if (climbing == 2) {
                    Debug.Log(climbing + "Z");
                    rigidbody2D.AddForce(new Vector2(0, 1) * jumpForceValue, ForceMode2D.Impulse);
                    rigidbody2D.velocity -= new Vector2(climbJumpForceValue, 0);
                }
            }
        }
    }


    public static int GetRandomSeed() {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == 1) {//左边碰撞
            if (collision.collider.GetComponent<TestWall>())
                touchedWallsL.Add(collision.collider.GetComponent<TestWall>());
        }
        else if (collision.contacts[0].normal.x == -1) {//右边碰撞
            if (collision.collider.GetComponent<TestWall>())
                touchedWallsR.Add(collision.collider.GetComponent<TestWall>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.GetComponent<TestWall>()) {
            if (touchedWallsL.Contains(collision.transform.GetComponent<TestWall>()))
                touchedWallsL.Remove(collision.transform.GetComponent<TestWall>());
            else if (touchedWallsR.Contains(collision.transform.GetComponent<TestWall>()))
                touchedWallsR.Remove(collision.transform.GetComponent<TestWall>());
        }
    }
}
