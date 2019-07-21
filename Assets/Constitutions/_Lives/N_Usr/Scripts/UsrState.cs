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
    [Range(0.1f, 0.5f)]
    public float fixCollisionInter = 0.1f;
    private float fixCollisionCounter = 0;

    [Range(0.05f, 0.2f)]
    public float nearAngleLoss = 0.1f;
    [Range(0.1f, 0.2f)]
    public float collisionLoss = 0.15f;
    [Range(0.01f, 0.2f)]
    public float onGroundLoss = 0.02f;

    public float moveForceValue = 0.01f;
    public float jumpForceValue = 0.01f;
    public float hurtForceValue = 0.01f;
    public float climbSlideSpeed = 0.8f;
    public float minSlideSpeed = 3.0f;
    public float maxSlideSpeed = 8.0f;
    public float maxMoveSpeed = 2.0f;
    public float climbJumpSpeed = 2.0f;
    public float slideOffSpeed = 5;

    public float slideSpeedDecA = 1;
    public float airDragA = 0.5f;//空气阻力
    public float slideG = 5;

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
    public bool isfixEnable = true;
    [SerializeField]
    private Vector3 lastLegalPos;

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
    public UIManager uiManager;

    public Vector2 startColliderOffset;
    public Vector2 startColliderSize;
    public Vector2 slideColliderOffset;
    public Vector2 slideColliderSize;
    public float startG;
    public Vector3 startCameraTargetLocalPos;

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
        currentDir = new Vector2(1, 0);
        climbingCounter = climbingInter;
        fixCollisionCounter = fixCollisionInter;
        currentManualPower = maxManualPower;
        currentMentalPower = maxMentalPower;
        
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

        startColliderSize = boxCollider2D.size;
        startColliderOffset = boxCollider2D.offset;
        startG = rigidbody2D.gravityScale;
        startCameraTargetLocalPos = transform.Find("Camera Target").transform.localPosition;

        uiManager.usrStatePanel.SetGreenKey(0, greenKey, transform);
        lastLegalPos = transform.position;
    }

    private void FixedUpdate() {
        UpdateOnGround();
        UpdateNearWall();
        UpdateNearSolid();
        UpdateNearDoor();
        UpdateJumpEnable();
        UpdateClimbingEnable();
        UpdateFixEnable();
        UpdateColor();
        UpdateUI();
        UpdateAni();
        UpdateLegalPos();
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
        uiManager.usrStatePanel.SetManualValue(transform.position, currentManualPower / maxManualPower);
        uiManager.usrStatePanel.SetMentalValue(currentMentalPower, maxMentalPower, myHeart);
        uiManager.usrStatePanel.SetGoldenKey(goldenKeyTime / maxGoldenKeyTime);
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

    private void UpdateFixEnable() {
        if (fixCollisionCounter >= fixCollisionInter)
            isfixEnable = true;
        else {
            isfixEnable = false;
            fixCollisionCounter += Time.deltaTime;
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
        for (int i = 0; i < nearWallsL.Count; i++)
            if (!(nearWallsL[i] ?? null))
                nearWallsL.RemoveAt(i);
        for (int i = 0; i < nearWallsR.Count; i++)
            if (!(nearWallsR[i] ?? null))
                nearWallsR.RemoveAt(i);

        if (nearWallsL.Count != 0)
            nearWall = LEFT_DIR;
        else if (nearWallsR.Count != 0)
            nearWall = RIGHT_DIR;
        else
            nearWall = 0;
    }

    private void UpdateNearDoor() {
        for (int i = 0; i < nearDoorL.Count; i++)
            if (!(nearDoorL[i] ?? null)) 
                nearDoorL.RemoveAt(i);
        for (int i = 0; i < nearDoorR.Count; i++)
            if (!(nearDoorR[i] ?? null)) 
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
        Collider2D[] hits = Physics2D.OverlapBoxAll(colliderPos + new Vector2(0, -onGroundLoss), boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hits == null || hits.Length == 0) {
            isOnGround = false;
        }
        else {
            foreach (Collider2D i in hits) {
                Vector2 tempPos = i.bounds.ClosestPoint(colliderPos);
                if (Mathf.Abs(colliderPos.y - tempPos.y - boxCollider2D.size.y / 2) > onGroundLoss ||
                    Mathf.Abs(Mathf.Abs(colliderPos.x - tempPos.x) - boxCollider2D.size.x / 2) < onGroundLoss) {
                    isOnGround = false;
                }
                else {
                    isOnGround = true;
                    break;
                }
            }
        }
    }

    private void UpdateLegalPos() {
        if (isOnGround && (currentAni.IsName("land")))
            lastLegalPos = transform.position;
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
        if (rigidbody2D.velocity.y > 0)
            animator.SetBool("isRise", true);
        else
            animator.SetBool("isRise", false);
        #endregion

        #region isSlide
        slideCounter += Time.deltaTime;
        if (animator.GetBool("isSlide")) {
            if (slideCounter >= slideInter) {
                Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y + 0.2f);
                Collider2D[] hits = Physics2D.OverlapBoxAll(colliderPos, startColliderSize, 0,
                    1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
                if (hits.Length == 0) {//站起碰撞盒检测
                    slideCounter = 0;
                    rigidbody2D.gravityScale = startG;
                    animator.SetBool("isSlide", false);
                }
                else {
                    int tempCount = 0;
                    foreach (Collider2D i in hits)
                        if (i.GetComponent<PlatformEffector2D>())
                            tempCount++;
                    if (tempCount == hits.Length) {
                        slideCounter = 0;
                        rigidbody2D.gravityScale = startG;
                        animator.SetBool("isSlide", false);
                    }
                }
            }
            if (Mathf.Abs(rigidbody2D.velocity.x) > minSlideSpeed) {
                Vector2 tempV = rigidbody2D.velocity;
                tempV.x= (Mathf.Abs(rigidbody2D.velocity.x) - slideSpeedDecA * Time.deltaTime) * currentDir.x;
                rigidbody2D.velocity = tempV;
                if (Mathf.Abs(rigidbody2D.velocity.x) < minSlideSpeed) {
                    rigidbody2D.velocity = minSlideSpeed * currentDir;
                }
            }
        }
        else
            slideCounter = 0;
        #endregion
        
        #region isAir
        if (isOnGround)
            animator.SetBool("isAir", false);
        else
            animator.SetBool("isAir", true);
        #endregion
    }

    #endregion

    public bool IsInUnActAni() =>
            currentAni.IsName("dashing") ||
            currentAni.IsName("dash_on") ||
            currentAni.IsName("dash_off") ||
            currentAni.IsName("sliding") ||
            currentAni.IsName("slide_on") ||
            currentAni.IsName("slide_off") ||
            currentAni.IsName("climb_on") ||
            currentAni.IsName("climb_off") ||
            currentAni.IsName("climbing_jump") ||
            //currentAni.IsName("land") ||
            currentAni.IsName("jump") ||
            animator.GetBool("isDash");

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
            if(nearDoorL.Count>0)
                return nearDoorL[0];
        }
        else if (nearDoor == RIGHT_DIR) {
            if(nearDoorR.Count>0)
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

    public void ResetFixCounter() {
        fixCollisionCounter = 0;
        isfixEnable = false;
    }

    public void SafeRealseDoor(MapDoor md) {
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

        uiManager.usrStatePanel.SetGreenKey(greenKey, greenKey - 1, transform);
        greenKey--;
        return true;
    }

    public void CollectGreenKey() {
        uiManager.usrStatePanel.SetGreenKey(greenKey, greenKey + 1, transform);
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
            uiManager.SetLoading(GMManager.BadEnd_2);
            Destroy(gameObject);
        }
    }

    public void SetHeart(int num) {
        myHeart = num;
        myHeart = Mathf.Clamp(myHeart, 0, myHeartMax);
        SetMental(100);
    }

    public void Hurted(Vector2 hurtDir, int damage) {
        if (colorRed != 0 || currentAni.IsName("dashing")) 
            return;

        ChangeMental(-damage);
        if(!IsInUnActAni())
            rigidbody2D.AddForce(hurtDir * hurtForceValue, ForceMode2D.Impulse);
        colorRed = 1;
        if (myHeart > 0)
            GetComponent<UsrAniCallBack>().HurtSound();
    }

    public void PositionBack() {
        transform.position = lastLegalPos;
        rigidbody2D.Sleep();
        isOnMove = false;
        ChangeHeart(-1);
    }

    #region PHYSICS_CALLBACK

    private void OnCollisionEnter2D(Collision2D collision) {
        //挤压判定
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        int contactNum = boxCollider2D.GetContacts(contacts);
        for (int i = 0; i < collision.contactCount; i++) {
            if (collision.GetContact(i).collider.GetComponent<PlatformEffector2D>() && Mathf.Abs(collision.GetContact(i).normal.y - 1) > nearAngleLoss)
                continue;
            if (!collision.GetContact(i).collider.GetComponent<MapWall>() && !collision.GetContact(i).collider.GetComponent<MapSolid>())
                continue;
            for (int j = 0; j < contactNum; j++) {
                if (contacts[j].collider.GetComponent<PlatformEffector2D>() && Mathf.Abs(contacts[j].normal.y - 1) > nearAngleLoss)
                    continue;
                if (!contacts[j].collider.GetComponent<MapWall>() && !contacts[j].collider.GetComponent<MapSolid>())
                    continue;
                float tempAngle = Vector2.Angle(collision.GetContact(i).normal, contacts[j].normal);
                if (tempAngle >= GMManager.JAM_ANGLE) {
                    PositionBack();
                    return;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        //临近物体更新
        if (Mathf.Abs(collision.GetContact(0).normal.x - 1) <= nearAngleLoss) {//左方碰撞
            if (collision.collider.GetComponent<MapWall>() && !nearWallsL.Contains(collision.collider.GetComponent<MapWall>())
                && collision.contactCount > 1
                && Mathf.Abs(collision.GetContact(0).point.y - collision.GetContact(1).point.y) > boxCollider2D.size.y / 2)
                nearWallsL.Add(collision.collider.GetComponent<MapWall>());
            if (collision.collider.GetComponent<MapWall>() && nearWallsL.Contains(collision.collider.GetComponent<MapWall>())
                && collision.contactCount > 1
                && Mathf.Abs(collision.GetContact(0).point.y - collision.GetContact(1).point.y) < boxCollider2D.size.y / 2)
                nearWallsL.Remove(collision.collider.GetComponent<MapWall>());

            if (collision.collider.GetComponent<MapDoor>() && !nearDoorL.Contains(collision.collider.GetComponent<MapDoor>()))
                nearDoorL.Add(collision.collider.GetComponent<MapDoor>());
            if (collision.collider.GetComponent<MapSolid>() && !nearSolidL.Contains(collision.collider.GetComponent<MapSolid>()))
                nearSolidL.Add(collision.collider.GetComponent<MapSolid>());
        }
        else if (Mathf.Abs(collision.GetContact(0).normal.x + 1) <= nearAngleLoss) {//右方碰撞
            if (collision.collider.GetComponent<MapWall>() && !nearWallsR.Contains(collision.collider.GetComponent<MapWall>())
                && collision.contactCount > 1
                && Mathf.Abs(collision.GetContact(0).point.y - collision.GetContact(1).point.y) > boxCollider2D.size.y / 2)
                nearWallsR.Add(collision.collider.GetComponent<MapWall>());
            if (collision.collider.GetComponent<MapWall>() && nearWallsR.Contains(collision.collider.GetComponent<MapWall>())
                && collision.contactCount > 1
                && Mathf.Abs(collision.GetContact(0).point.y - collision.GetContact(1).point.y) < boxCollider2D.size.y / 2)
                nearWallsR.Remove(collision.collider.GetComponent<MapWall>());

            if (collision.collider.GetComponent<MapDoor>() && !nearDoorR.Contains(collision.collider.GetComponent<MapDoor>()))
                nearDoorR.Add(collision.collider.GetComponent<MapDoor>());
            if (collision.collider.GetComponent<MapSolid>() && !nearSolidR.Contains(collision.collider.GetComponent<MapSolid>()))
                nearSolidR.Add(collision.collider.GetComponent<MapSolid>());
        }

        //微小碰撞修复
        if (Mathf.Abs(collision.GetContact(0).normal.x - 1) <= nearAngleLoss) {//左方碰撞
            if (isOnMove && isfixEnable) {
                float tempLoss;
                if (collision.contactCount == 1) {
                    tempLoss = 0;
                }
                else {
                    tempLoss = Mathf.Abs(collision.GetContact(0).point.y - collision.GetContact(1).point.y);
                }
                if (collision.contactCount > 0) {
                    if (Mathf.Abs(tempLoss) < collisionLoss) {
                        transform.Translate(new Vector3(-0.1f, tempLoss, 0), Space.Self);
                        ResetFixCounter();
                        Debug.Log("left-fixed");
                    }
                }
            }
        }
        else if (Mathf.Abs(collision.GetContact(0).normal.x + 1) <= nearAngleLoss) {//右方碰撞
            if (isOnMove && isfixEnable) {
                float tempLoss;
                if (collision.contactCount == 1) {
                    tempLoss = 0;
                }
                else {
                    tempLoss = Mathf.Abs(collision.GetContact(0).point.y - collision.GetContact(1).point.y);
                }
                if (collision.contactCount > 0) {
                    if (Mathf.Abs(tempLoss) < collisionLoss) {
                        transform.Translate(new Vector3(0.1f, tempLoss, 0), Space.Self);
                        ResetFixCounter();
                        Debug.Log("right-fixed");
                    }
                }
            }
        }
        else if (Mathf.Abs(collision.GetContact(0).normal.y - 1) <= nearAngleLoss) {//下方碰撞
            if (!isOnMove && isfixEnable) {
                float avContact;
                float tempLoss;
                if (collision.contactCount == 1) {
                    avContact = collision.GetContact(0).point.x;
                    tempLoss = 0;
                }
                else {
                    avContact = (collision.GetContact(0).point.x + collision.GetContact(1).point.x) / 2;
                    tempLoss = Mathf.Abs(collision.GetContact(0).point.x - collision.GetContact(1).point.x);
                }
                if (collision.contactCount > 0) {
                    if (avContact > ((Vector2)transform.position + boxCollider2D.offset).x)//障碍偏右
                        tempLoss = -tempLoss;
                    if (Mathf.Abs(tempLoss) < collisionLoss) {
                        transform.Translate(new Vector3(tempLoss, -0.1f, 0), Space.Self);
                        ResetFixCounter();
                        Debug.Log("down-fixed");
                    }
                }
            }
        }
        else if (Mathf.Abs(collision.GetContact(0).normal.y + 1) <= nearAngleLoss) {//上方碰撞
            if (!isOnGround && isfixEnable) {
                float avContact;
                float tempLoss;
                if (collision.contactCount == 1) {
                    avContact = collision.GetContact(0).point.x;
                    tempLoss = 0;
                }
                else {
                    avContact = (collision.GetContact(0).point.x + collision.GetContact(1).point.x) / 2;
                    tempLoss = Mathf.Abs(collision.GetContact(0).point.x - collision.GetContact(1).point.x);
                }
                if (collision.contactCount > 0) {
                    if (avContact > ((Vector2)transform.position + boxCollider2D.offset).x)//障碍偏右
                        tempLoss = -tempLoss;
                    if (Mathf.Abs(tempLoss) < collisionLoss) {
                        transform.Translate(new Vector3(tempLoss, 0.1f, 0), Space.Self);
                        ResetFixCounter();
                        Debug.Log("up-fixed");
                    }
                }
            }
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
            else 
                CollectGoldenKey(collision.transform.GetComponent<GoldenKey>().duringTime);
            Destroy(collision.gameObject);
        }
    }

    #endregion
}
