using System.Collections.Generic;
using UnityEngine;

public class UsrState : MonoBehaviour
{
    public float idelInter = 5.0f;//闲置idle播放间隔
    public float slideInter = 0.4f;//基本滑行时间
    public float climbingInter = 0.5f;//两次climb最小间隔
    public const int LEFT_DIR = 1;
    public const int RIGHT_DIR = 2;
    public bool isOnGround = false;
    public bool isOnMove = false;//是否被控制移动
    public int OnClimb = 0;//0->未处在爬墙状态，1->Left，2->Right

    public int nearWall = 0;
    private List<TestWall> nearWallsL;
    private List<TestWall> nearWallsR;

    public int nearDoor = 0;
    private List<MapDoor> nearDoorL;
    private List<MapDoor> nearDoorR;

    private float idleCounter = 0;
    private float slideCounter = 0;
    private float climbingCounter = 0;
    //Compnents
    public Animator animator;
    public AudioSource audioSource;
    public new Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public AnimatorStateInfo currentAni;

    public Vector2 startColliderOffset;
    public Vector2 startColliderSize;
    public AudioClip audioRunL;
    public AudioClip audioRunR;
    public AudioClip audioIdle;
    //
    public Vector2 currentDir;
    public float moveForceValue = 0.01f;
    public float jumpForceValue = 0.01f;
    public float climbSlideSpeed = 0.8f;
    public float slideSpeed = 5.0f;
    public float climbJumpSpeed = 2.0f;
    public float dashOffset = 0.5f;

    public bool isJumpEnable = false;
    public bool isClimbEnable = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        nearWallsL = new List<TestWall>();
        nearWallsR = new List<TestWall>();
        nearDoorL = new List<MapDoor>();
        nearDoorR = new List<MapDoor>();
        currentDir = new Vector2(0, 1);
        climbingCounter = climbingInter;
        
        audioSource = GetComponent<AudioSource>();
        startColliderSize = GetComponent<BoxCollider2D>().size;
        startColliderOffset = GetComponent<BoxCollider2D>().offset;
    }

    private void FixedUpdate() {
        UpdateOnGround();
        UpdateNearWall();
        UpdateNearDoor();
        UpdateJumpEnable();
        UpdateClimbingEnable();
        UpdateAni();
        UpdateSound();

        Check();
    }

    private void UpdateSound() {
        if (GetComponent<SpriteRenderer>().sprite.name == "run_2" && isOnGround) {
            if (!audioSource.isPlaying) {
                audioSource.volume = 0.4f;
                audioSource.clip = audioRunL;
                audioSource.Play();
            }
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "run_6" && isOnGround) {
            if (!audioSource.isPlaying) {
                audioSource.volume = 0.4f;
                audioSource.clip = audioRunR;
                audioSource.Play();
            }
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "idle_2" && isOnGround) {
            if (!audioSource.isPlaying) {
                audioSource.volume = 1.0f;
                audioSource.clip = audioIdle;
                audioSource.Play();
            }
        }
    }

    private void UpdateClimbingEnable() {
        if (climbingCounter >= climbingInter)
            isClimbEnable = true;
        else {
            isClimbEnable = false;
            climbingCounter += Time.deltaTime;
        }
    }

    private void UpdateJumpEnable() {
        if ((OnClimb != 0 || isOnGround))
            isJumpEnable = true;
        else
            isJumpEnable = false;
    }

    private void UpdateNearWall() {
        if (nearWallsL.Count != 0)
            nearWall = LEFT_DIR;
        else if (nearWallsR.Count != 0)
            nearWall = RIGHT_DIR;
        else
            nearWall = 0;
    }

    private void UpdateNearDoor() {
        for (int i = 0; i < nearDoorL.Count; i++)
            if (nearDoorL[i] == null)
                nearDoorL.RemoveAt(i);
        for (int i = 0; i < nearDoorR.Count; i++)
            if (nearDoorR[i] == null)
                nearDoorR.RemoveAt(i);

        if (nearDoorL.Count != 0)
            nearDoor = LEFT_DIR;
        else if (nearDoorR.Count != 0)
            nearDoor = RIGHT_DIR;
        else
            nearDoor = 0;
    }

    private void UpdateOnGround() {
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + boxCollider2D.offset;
        Collider2D hit = Physics2D.OverlapBox(colliderPos + new Vector2(0, -0.1f), boxCollider2D.size + new Vector2(-0.1f, 0), 0,
            1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit != null) 
            isOnGround = true;
        else 
            isOnGround = false;
    }

    private void UpdateAni() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);

        #region scale
        if (currentDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (currentDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);

        if (OnClimb == LEFT_DIR)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (OnClimb == RIGHT_DIR)
            transform.localScale = new Vector3(1, 1, 1);
        #endregion

        #region isRun
        if (rigidbody2D.velocity.x < 0.2f && !isOnMove)
            animator.SetBool("isRun", false);
        #endregion

        #region isIdle
        if (idleCounter >= idelInter) {
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
        if (OnClimb != 0&&!currentAni.IsName("climbing_jump")) {
            animator.SetBool("isClimb", true);
        }
        else
            animator.SetBool("isClimb", false);

        #endregion

        #region isFall
        if (rigidbody2D.velocity.y <= 0 && !isOnGround)
            animator.SetBool("isFall", true);
        else
            animator.SetBool("isFall", false);
        #endregion

        #region isRise
        if (rigidbody2D.velocity.y > 0 && !isOnGround)
            animator.SetBool("isRise", true);
        else if (!currentAni.IsName("jump") && !currentAni.IsName("climbing_jump"))
            animator.SetBool("isRise", false);
        #endregion

        #region isSlide
        slideCounter += Time.deltaTime;
        if (animator.GetBool("isSlide")) {
            if (!isOnGround || nearWall != 0 || nearDoor != 0|| slideCounter >= slideInter) {
                Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y + 0.2f);
                Collider2D hit = Physics2D.OverlapBox(colliderPos, startColliderSize, 0,
                    1 << LayerMask.NameToLayer("soild") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));//站起碰撞盒检测
                if (hit == null) {
                    slideCounter = 0;
                    animator.SetBool("isSlide", false);
                }
            }
        }
        else
            slideCounter = 0;
        #endregion
    }

    private void Check() {//更新合法值
        if (OnClimb < 0)
            OnClimb = 0;
        if (OnClimb > 2)
            OnClimb = 2;
    }

    public bool IsNearWall(int param) {
        return nearWall == param;
    }

    public void ResetClimbCounter() {
        climbingCounter = 0;
        isClimbEnable = false;
        animator.SetBool("isClimb", false);
        OnClimb = 0;
    }

    public MapDoor GetDoor() {//return neardoor
        if (nearDoor == LEFT_DIR) {
            return nearDoorL[0];
        }
        else if (nearDoor == RIGHT_DIR) {
            return nearDoorR[0];
        }
        return null;
    }

    public void CheckDoor(MapDoor md) {
        if (nearDoorL.Contains(md))
            nearDoorL.Remove(md);
        else if (nearDoorR.Contains(md))
            nearDoorR.Remove(md);
        nearDoor = 0;
    }

    #region Physics CallBack
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == 1) {//左边碰撞
            if (collision.collider.GetComponent<TestWall>())
                nearWallsL.Add(collision.collider.GetComponent<TestWall>());
            if (collision.collider.GetComponent<MapDoor>())
                nearDoorL.Add(collision.collider.GetComponent<MapDoor>());
        }
        else if (collision.contacts[0].normal.x == -1) {//右边碰撞
            if (collision.collider.GetComponent<TestWall>())
                nearWallsR.Add(collision.collider.GetComponent<TestWall>());
            if (collision.collider.GetComponent<MapDoor>())
                nearDoorR.Add(collision.collider.GetComponent<MapDoor>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.GetComponent<TestWall>()) {
            if (nearWallsL.Contains(collision.transform.GetComponent<TestWall>()))
                nearWallsL.Remove(collision.transform.GetComponent<TestWall>());
            else if (nearWallsR.Contains(collision.transform.GetComponent<TestWall>()))
                nearWallsR.Remove(collision.transform.GetComponent<TestWall>());
        }
        if (collision.transform.GetComponent<MapDoor>()) {
            if (nearDoorL.Contains(collision.transform.GetComponent<MapDoor>()))
                nearDoorL.Remove(collision.transform.GetComponent<MapDoor>());
            else if (nearDoorR.Contains(collision.transform.GetComponent<MapDoor>()))
                nearDoorR.Remove(collision.transform.GetComponent<MapDoor>());
        }
    }
    #endregion
}
