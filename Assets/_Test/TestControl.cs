using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class TestControl : MonoBehaviour
{
    public ControlMode currentMode;
    public Vector2 m_v;
    public const int LEFT_DIR = 1;
    public const int RIGHT_DIR = 2;
    const float MAX_CLIMBINGINTER = 0.5f;
    public float climbingCounter = 0;
    const float MAX_IDLEINTER = 2.0f;
    public float idleCounter = 0;
    const float MIN_SLIDEINTER = 1.0f;
    public float slideCounter = 0;
    const float DASHINTER = 0.05f;
    public float dashCounter = 0;

    public float moveForceValue = 0.01f;
    public float jumpForceValue = 0.01f;
    public float slideForceValue = 0.02f;
    public float slideSpeed = 5.0f;
    public float dashSpeed = 50.0f;
    public Vector2 slideDir;
    public Vector2 dashDir;
    public float climbJumpForceValue = 2.0f;
    public float maxMoveSpeed = 2.0f;
    public float horizonA = 0.5f;
    [SerializeField]
    private bool isJumpEnable = false;
    [SerializeField]
    private bool isClimbEnable = false;
    [SerializeField]
    private bool isControlEnable = true;
    [SerializeField]
    private bool isOnGround = false;
    [SerializeField]
    private bool isOnMove = false;
    [SerializeField]
    private bool isOnSlide = false;
    [SerializeField]
    private bool isOnDash = false;
    [SerializeField]
    private bool isOnWallL = false;
    [SerializeField]
    private bool isOnWallR = false;
    [SerializeField]
    public int climbing = 0;//1->left,2->right

    private List<MapWall> touchedWallsL;
    private List<MapWall> touchedWallsR;
    private new Rigidbody2D rigidbody2D;
    private Vector2 currentDir;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private AnimatorStateInfo currentAni;
    private Vector2 startColliderOffset;
    private Vector2 startColliderSize;

    private AudioSource player;
    [SerializeField]
    private readonly AudioClip run1;
    [SerializeField]
    private readonly AudioClip run2;
    void Awake() {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        touchedWallsL = new List<MapWall>();
        touchedWallsR = new List<MapWall>();
        currentDir = new Vector2(0, 1);
        currentMode = ControlMode.COMMON;
        climbingCounter = MAX_CLIMBINGINTER;
        
        player = GetComponent<AudioSource>();
        startColliderSize = GetComponent<BoxCollider2D>().size;
        startColliderOffset= GetComponent<BoxCollider2D>().offset;
    }

    void Update() {
        if (isOnSlide) {
            rigidbody2D.velocity = slideSpeed * slideDir;
            return;
        }
        if (isOnDash) {
            rigidbody2D.velocity = dashSpeed * dashDir;
            return;
        }
        if (!isControlEnable|| currentAni.IsName("land"))
            return;
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
        UpdateOnWall();
        UpdateOnSlide();
        UpdateOnDash();
        UpdateAni();
        UpdateUsr();
        UpdateJumpEnable();
        UpdateClimbingEnable();
        UpdateControlEnable();
        currentAni = animator.GetCurrentAnimatorStateInfo(0);
        Check();
    }

    private void Check() {//更新合法值
        if (isOnMove && rigidbody2D.velocity.x > maxMoveSpeed) {
            float tempy = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(maxMoveSpeed, tempy);
        }
        else if (isOnMove && rigidbody2D.velocity.x < -maxMoveSpeed) {
            float tempy = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(-maxMoveSpeed, tempy);
        }
        if (climbing < 0)
            climbing = 0;
        if (climbing > 2)
            climbing = 2;
    }
    private void IsInControlableAni() {

    }

    private void UpdateJumpEnable() {
        if ((climbing != 0 || isOnGround) && !currentAni.IsName("land") && !currentAni.IsName("jump"))
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
    private void UpdateControlEnable() {
    }

    private void UpdateOnGround() {
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + boxCollider2D.offset;
        Collider2D hit = Physics2D.OverlapBox(colliderPos+ new Vector2(0, -0.1f), boxCollider2D.size + new Vector2(-0.1f, 0), 0,
            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
        if (hit != null) {
            isOnGround = true;
        }
        else {
            isOnGround = false;
        }
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
    private void UpdateOnSlide() {
        slideCounter += Time.deltaTime;
        if (isOnSlide) {
            if (!isOnGround || isOnWallL || isOnWallR) {
                slideCounter = 0;
                isOnSlide = false;
            }
            else if (slideCounter >= MIN_SLIDEINTER) {
                Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y + 0.2f) + startColliderOffset;
                Collider2D hit = Physics2D.OverlapBox(colliderPos, startColliderSize, 0,
                    1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
                if (hit == null) {
                    slideCounter = 0;
                    isOnSlide = false;
                }
            }
        }
        else
            slideCounter = 0;
    }
    private void UpdateOnDash() {
        dashCounter += Time.deltaTime;
        if (isOnDash) {
            if (dashCounter >= DASHINTER|| !isOnGround ) {
                dashCounter = 0;
                isOnDash = false;
            }
        }
        else
            dashCounter = 0;
    }

    private void UpdateAni() {
        #region scale
        if (currentDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (currentDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);

        if (climbing == LEFT_DIR)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (climbing == RIGHT_DIR)
            transform.localScale = new Vector3(1, 1, 1);
        #endregion

        #region isRun
        if (rigidbody2D.velocity.x < 0.2f&&!isOnMove)
            animator.SetBool("isRun", false);
        #endregion

        #region isIdle
        if (idleCounter >= MAX_IDLEINTER) {
            int temp = GMManager.rd.Next() % 2;
            if (temp == 0) {
                animator.SetBool("isIdleBlink", false);
                animator.SetBool("isIdleBreathe", true);
            }
            else if (temp == 1) {
                animator.SetBool("isIdleBreathe", false);
                animator.SetBool("isIdleBlink", true);
            }
            idleCounter = 0;
        }
        else if (rigidbody2D.velocity == Vector2.zero) {
            idleCounter += Time.deltaTime;
        }
        else
            idleCounter = 0;
        #endregion

        #region isClimb
        if (climbing != 0) {
            animator.SetBool("isClimb", true);
        }
        else
            animator.SetBool("isClimb", false);

        #endregion

        #region isFall
        if(rigidbody2D.velocity.y<=0 && !isOnGround && climbing == 0)
            animator.SetBool("isFall", true);
        else
            animator.SetBool("isFall", false);
        #endregion

        #region isRise
        if (rigidbody2D.velocity.y > 0 && !isOnGround && climbing == 0)
            animator.SetBool("isRise", true);
        else if(!currentAni.IsName("jump")&& !currentAni.IsName("climbing_jump"))
            animator.SetBool("isRise", false);
        #endregion

        #region isSlide
        if (isOnSlide == false) {
            animator.SetBool("isSlide", false);
        }
        #endregion

        #region isDash
        if (isOnDash == false) {
            animator.SetBool("isDash", false);
        }
        #endregion
    }
    private void UpdateUsr() {
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
                animator.SetBool("isClimb", false);
                climbingCounter = 0;
                rigidbody2D.gravityScale = 1;
                climbing = param;
                break;
            case ControlMode.CLIMBING:
                isOnMove = false;
                rigidbody2D.velocity = new Vector2(0, -slideForceValue);
                rigidbody2D.gravityScale = 0;
                climbing = param;
                break;
        }
        currentMode = cm;
    }

    private void UsrControlCommon() {//一般玩家交互
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            isOnMove = false;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            isOnMove = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) {
            isOnMove = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            Move(LEFT_DIR);//左移动
        }        
        else if (Input.GetKey(KeyCode.RightArrow)) {
            Move(RIGHT_DIR);//右移动
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Slide();
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            Jump();//跳跃
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            Dash();//特殊技
        }
    }
    private void UsrControlClimbing() {//爬墙交互
        if (Input.GetKey(KeyCode.LeftArrow)) {
            if (!isOnWallL|| isOnGround) 
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetKey(KeyCode.LeftArrow)) {
            if (climbing == LEFT_DIR)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            if (!isOnWallR || isOnGround)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetKey(KeyCode.RightArrow)) {
            if (climbing == RIGHT_DIR)
                ChangeControlMode(ControlMode.COMMON);
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            ClimbingJump();
        }
    }
//act
    public void Jump() {
        if (!isJumpEnable) 
            return;
        isJumpEnable = false;
        animator.SetBool("isJump", true);
    }
    public void ClimbingJump() {
        if (isJumpEnable&&currentAni.IsName("climbing")) {
            animator.SetBool("isJump", true);
            animator.SetBool("isCLimb", true);
            if (GetComponent<TestControl>().climbing == TestControl.LEFT_DIR) {
                rigidbody2D.AddForce(new Vector2(0, 1) * GetComponent<TestControl>().jumpForceValue, ForceMode2D.Impulse);
                rigidbody2D.velocity += new Vector2(GetComponent<TestControl>().climbJumpForceValue, 0);
            }
            if (GetComponent<TestControl>().climbing == TestControl.RIGHT_DIR) {
                rigidbody2D.AddForce(new Vector2(0, 1) * GetComponent<TestControl>().jumpForceValue, ForceMode2D.Impulse);
                rigidbody2D.velocity -= new Vector2(GetComponent<TestControl>().climbJumpForceValue, 0);
            }
        }
    }
    public void Move(int moveDir) {
        isOnMove = true;
        animator.SetBool("isRun", true);
        if (moveDir == LEFT_DIR) {
            currentDir = new Vector2(-1, 0);
            if (isOnWallL && isClimbEnable) {
                ChangeControlMode(ControlMode.CLIMBING, moveDir);
            }
        }
        if (moveDir == RIGHT_DIR) {
            currentDir = new Vector2(1, 0);
            if (isOnWallR && isClimbEnable) {
                ChangeControlMode(ControlMode.CLIMBING, moveDir);
            }
        }
        rigidbody2D.AddForce(currentDir * moveForceValue, ForceMode2D.Force);
    }
    public void Dash() {
        if (isOnDash || !currentAni.IsName("run") || !isOnMove) {
            return;
        }
        if (!isOnWallL) {
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + boxCollider2D.offset;
        Collider2D hit = Physics2D.OverlapBox(colliderPos+ new Vector2(-1.5f, 0), boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
            if (hit == null) {
                //transform.Translate(new Vector2(-1.5f, 0), Space.Self);
            }

            isOnMove = false;
            isOnDash = true;
            animator.SetBool("isDash", true);
            dashDir = currentDir;
        }
        if (isOnWallR) {
            Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + boxCollider2D.offset;
            Collider2D hit = Physics2D.OverlapBox(colliderPos + new Vector2(1.5f, 0), boxCollider2D.size, 0,
                1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall"));
            if (hit == null) {
                //transform.Translate(new Vector2(1.5f, 0), Space.Self);
            }

            isOnMove = false;
            isOnDash = true;
            animator.SetBool("isDash", true);
            dashDir = currentDir;
        }
    }
    public void Slide() {
        if (isOnSlide||!currentAni.IsName("run")||!isOnMove) {
            return;
        }
        isOnMove = false;
        isOnSlide = true;
        animator.SetBool("isSlide", true);
        slideDir = currentDir;
    }
    //
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == 1) {//左边碰撞
            if (collision.collider.GetComponent<MapWall>())
                touchedWallsL.Add(collision.collider.GetComponent<MapWall>());
        }
        else if (collision.contacts[0].normal.x == -1) {//右边碰撞
            if (collision.collider.GetComponent<MapWall>())
                touchedWallsR.Add(collision.collider.GetComponent<MapWall>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.GetComponent<MapWall>()) {
            if (touchedWallsL.Contains(collision.transform.GetComponent<MapWall>()))
                touchedWallsL.Remove(collision.transform.GetComponent<MapWall>());
            else if (touchedWallsR.Contains(collision.transform.GetComponent<MapWall>()))
                touchedWallsR.Remove(collision.transform.GetComponent<MapWall>());
        }
    }
}
