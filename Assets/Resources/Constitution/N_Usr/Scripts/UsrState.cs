using System;
using System.Collections.Generic;
using UnityEngine;

public class UsrState : MonoBehaviour
{
    public const int LEFT_DIR = 1;
    public const int RIGHT_DIR = 2;

    [Space()]
    [Header("常量设置")]

    [Range(3, 10)]
    public float idelInter = 5.0f;//闲置idle播放间隔
    private float idleCounter = 0;
    [Range(0, 1)]
    public float slideInter = 0.4f;//基本滑行时间
    private float slideCounter = 0;
    [Range(0.3f, 1)]
    public float climbingInter = 0.5f;//两次climb最小间隔
    private float climbingCounter = 0;

    public float moveForceValue = 0.01f;
    public float jumpForceValue = 0.01f;
    public float climbSlideSpeed = 0.8f;
    public float minSlideSpeed = 3.0f;
    public float maxSlideSpeed = 8.0f;
    public float maxMoveSpeed = 2.0f;
    public float climbJumpSpeed = 2.0f;

    public float slideSpeedDecA = 1;
    public float airDragA = 0.5f;//空气阻力

    public float createDistance = 0.5f;
    public float doorDistance = 0.5f;//垂直距离

    public float ManualCostOnClimbingJump = 40;
    public float ManualCostOnSlide = 20;

    public float colorRed = 0;
    public float colorRedSpeed = 2f;

    public GameObject pbHeartPrompt;

    [Space()]
    [Header("状态反馈")]

    public Vector2 currentDir;
    public AnimatorStateInfo currentAni;
    public bool isOnGround = false;
    public bool isOnMove = false;//是否被控制移动
    public bool isJumpEnable = false;
    public bool isClimbEnable = false;
    public bool isControlEnable = true;
    public int OnClimb = 0;//0->否，1->左，2->右

    public int nearSolid = 0;
    private List<MapSolid> nearSolidL;
    private List<MapSolid> nearSolidR;

    public int nearWall = 0;
    private List<MapWall> nearWallsL;
    private List<MapWall> nearWallsR;

    public int nearDoor = 0;
    private List<MapDoor> nearDoorL;
    private List<MapDoor> nearDoorR;

    [Space()]
    [Header("信息")]

    public Animator animator;
    public new Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;
    public UsrDustEffect dustEffect;
    public UsrStatePanel stateUI;
    public LoadingPanel loadingPanel;

    public Vector2 startColliderOffset;
    public Vector2 startColliderSize;

    [Space()]
    [Header("人物属性")]

    public float maxManualPower = 100;//体力
    public float currentManualPower = 0;
    public float remanualSpeed = 10f;//恢复速度

    public int myHeart = 2;//心
    public int myHeartMax = 20;//心
    public float maxMentalPower = 100;//精神力
    public float currentMentalPower = 100;

    public int greenKey = 3;
    public float maxGoldenKeyTime = 5;
    public float goldenKeyTime = 5;

    void Awake() {
        currentDir = new Vector2(0, 1);
        climbingCounter = climbingInter;
        currentManualPower = maxManualPower;
        currentMentalPower = maxMentalPower;
        startColliderSize = GetComponent<BoxCollider2D>().size;
        startColliderOffset = GetComponent<BoxCollider2D>().offset;

        nearDoorL = new List<MapDoor>();
        nearDoorR = new List<MapDoor>();
        nearWallsL = new List<MapWall>();
        nearWallsR = new List<MapWall>();
        nearSolidL = new List<MapSolid>();
        nearSolidR = new List<MapSolid>();
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        dustEffect = GetComponentInChildren<UsrDustEffect>();

        stateUI.SetGreenKey(0, greenKey, transform);
    }

    private void FixedUpdate() {
        UpdateOnGround();
        UpdateNearWall();
        UpdateNearSolid();
        UpdateNearDoor();
        UpdateJumpEnable();
        UpdateClimbingEnable();
        UpdateColor();
        UpdateUI();
        UpdateAni();

        ResumeManual();
        GoldenKeyTimeLose();

        if (rigidbody2D.velocity.y < -maxMoveSpeed*10) {
            Vector2 tempV = rigidbody2D.velocity;
            tempV.y = -maxMoveSpeed * 10;
            rigidbody2D.velocity = tempV;
        }
    }

    #region UPDATE

    private void UpdateUI() {
        stateUI.SetManualValue(transform.position, currentManualPower / maxManualPower);
        stateUI.SetMentalValue(currentMentalPower, maxMentalPower, myHeart);
        stateUI.SetGoldenKey(goldenKeyTime / maxGoldenKeyTime);
    }

    private void UpdateColor() {
        if (colorRed > 0) {
            colorRed -= colorRedSpeed * Time.deltaTime;
            colorRed = Mathf.Clamp(colorRed, 0, 1);
        }
        Color tempColor = GetComponent<SpriteRenderer>().color;
        tempColor.g = 1 - colorRed;
        tempColor.b = 1 - colorRed;
        GetComponent<SpriteRenderer>().color = tempColor;
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

    private void UpdateNearSolid() {
        if (nearSolidL.Count != 0)
            nearSolid = LEFT_DIR;
        else if (nearSolidR.Count != 0)
            nearSolid = RIGHT_DIR;
        else
            nearSolid = 0;
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
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit != null)
            isOnGround = true;
        else
            isOnGround = false;
    }

    private void UpdateAni() {//动画状态机更新
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
        if (OnClimb != 0 && !currentAni.IsName("climbing_jump")) {
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
            if (!isOnGround || slideCounter >= slideInter) {
                Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y + 0.2f);
                Collider2D hit = Physics2D.OverlapBox(colliderPos, startColliderSize, 0,
                    1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));//站起碰撞盒检测
                if (hit == null) {
                    slideCounter = 0;
                    animator.SetBool("isSlide", false);
                }
            }
            if (Mathf.Abs(rigidbody2D.velocity.x) > minSlideSpeed) {
                rigidbody2D.velocity = (Mathf.Abs(rigidbody2D.velocity.x) - slideSpeedDecA * Time.deltaTime) * currentDir;
                if (Mathf.Abs(rigidbody2D.velocity.x) < minSlideSpeed) {
                    rigidbody2D.velocity = minSlideSpeed * currentDir;
                }
            }

        }
        else
            slideCounter = 0;
        #endregion
    }

    #endregion

    private void ResumeManual() {
        currentManualPower += remanualSpeed * Time.deltaTime;
        currentManualPower = Mathf.Clamp(currentManualPower, 0, maxManualPower);
    }

    private void GoldenKeyTimeLose() {
        if (goldenKeyTime > 0)
            goldenKeyTime -= Time.deltaTime;
        if (goldenKeyTime < 0)
            goldenKeyTime = 0;
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

    public bool IsNearWall(int param) {
        return nearWall == param;
    }

    public void ResetClimbCounter() {
        climbingCounter = 0;
        isClimbEnable = false;
        animator.SetBool("isClimb", false);
        OnClimb = 0;
    }

    public void CheckDoor(MapDoor md) {
        if (nearDoorL.Contains(md))
            nearDoorL.Remove(md);
        else if (nearDoorR.Contains(md))
            nearDoorR.Remove(md);
        nearDoor = 0;
    }

    public bool UseGreenKey() {
        if (greenKey <= 0) {
            GetComponent<UsrAniCallBack>().ErrorSound();
            return false;
        }

        stateUI.SetGreenKey(greenKey, greenKey - 1, transform);
        greenKey--;
        return true;
    }

    public void CollectGreenKey() {
        stateUI.SetGreenKey(greenKey, greenKey + 1, transform);
        GetComponent<UsrAniCallBack>().GotKeySound();

        greenKey++;
    }

    public void CollectGoldenKey(float dt) {
        maxGoldenKeyTime = dt;
        goldenKeyTime = maxGoldenKeyTime;
        GetComponent<UsrAniCallBack>().GotKeySound();
    }

    public void SetMental(int num) {
        if (num < 0)
            return;
        currentMentalPower = num;
        currentMentalPower = Mathf.Clamp(currentMentalPower,0,maxMentalPower);
    }

    public void ChangeMental(int change) {
        currentMentalPower += change;
        currentMentalPower = Mathf.Clamp(currentMentalPower, 0, maxMentalPower);
        if (currentMentalPower <= 0) {
            ChangeHeart(-1);
        }
    }

    public void SetHeartPrompt(int num) {
        GameObject temp = Instantiate(pbHeartPrompt, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        temp.GetComponent<HeartPropmt>().Set(num);
    }

    public void ChangeHeart(int change) {
        SetHeartPrompt(change);
        myHeart += change;
        myHeart = Mathf.Clamp(myHeart, 0, myHeartMax);
        SetMental(100);

        if (myHeart <= 0) {
            //
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.Set(GMManager.BadEnd_2);
            Destroy(gameObject);
        }
    }

    public void SetHeart(int num) {
        myHeart = num;
        myHeart = Mathf.Clamp(myHeart, 0, myHeartMax);
        SetMental(100);
    }

    public void Hurted(Vector2 hurtDir, int damage) {
        if (colorRed != 0)
            return;

        ChangeMental(-damage);
        rigidbody2D.AddForce(hurtDir * jumpForceValue, ForceMode2D.Impulse);
        colorRed = 1;
        GetComponent<UsrAniCallBack>().HurtSound();
    }

    #region PHYSICS_CALLBACK

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == 1) {//左边碰撞
            if (collision.collider.GetComponent<MapWall>())
                nearWallsL.Add(collision.collider.GetComponent<MapWall>());
            if (collision.collider.GetComponent<MapDoor>())
                nearDoorL.Add(collision.collider.GetComponent<MapDoor>());
            if (collision.collider.GetComponent<MapSolid>())
                nearSolidL.Add(collision.collider.GetComponent<MapSolid>());
        }
        else if (collision.contacts[0].normal.x == -1) {//右边碰撞
            if (collision.collider.GetComponent<MapWall>())
                nearWallsR.Add(collision.collider.GetComponent<MapWall>());
            if (collision.collider.GetComponent<MapDoor>())
                nearDoorR.Add(collision.collider.GetComponent<MapDoor>());
            if (collision.collider.GetComponent<MapSolid>())
                nearSolidR.Add(collision.collider.GetComponent<MapSolid>());
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.GetComponent<MapWall>()) {
            if (nearWallsL.Contains(collision.transform.GetComponent<MapWall>()))
                nearWallsL.Remove(collision.transform.GetComponent<MapWall>());
            else if (nearWallsR.Contains(collision.transform.GetComponent<MapWall>()))
                nearWallsR.Remove(collision.transform.GetComponent<MapWall>());
        }
        if (collision.transform.GetComponent<MapDoor>()) {
            if (nearDoorL.Contains(collision.transform.GetComponent<MapDoor>()))
                nearDoorL.Remove(collision.transform.GetComponent<MapDoor>());
            else if (nearDoorR.Contains(collision.transform.GetComponent<MapDoor>()))
                nearDoorR.Remove(collision.transform.GetComponent<MapDoor>());
        }
        if (collision.transform.GetComponent<MapSolid>()) {
            if (nearSolidL.Contains(collision.transform.GetComponent<MapSolid>()))
                nearSolidL.Remove(collision.transform.GetComponent<MapSolid>());
            else if (nearSolidR.Contains(collision.transform.GetComponent<MapSolid>()))
                nearSolidR.Remove(collision.transform.GetComponent<MapSolid>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.GetComponent<GoldenKey>() != null || collision.transform.GetComponent<GreenKey>() != null) {
            if (collision.transform.GetComponent<GreenKey>() != null)
                CollectGreenKey();
            else {
                CollectGoldenKey(collision.transform.GetComponent<GoldenKey>().duringTime);
            }
            Destroy(collision.gameObject);
        }
    }

    #endregion
}
