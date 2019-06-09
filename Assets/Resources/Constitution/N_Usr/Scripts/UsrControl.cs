using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlMode
{
    COMMON, CLIMBING
}

public class UsrControl : MonoBehaviour
{
    private UsrState state;
    private ControlMode currentMode;
    [SerializeField]
    private GameObject pbDoor;
    public MapDoor myCreate = null;
    public MapDoor dashTarget = null;

    private float startG;

    void Start()
    {
        currentMode = ControlMode.COMMON;
        state = GetComponent<UsrState>();
        startG = state.rigidbody2D.gravityScale;
    }

    void Update()
    {
        if (!state.isControlEnable|| IsInUnActAni())
            return;
        switch (currentMode) {
            case ControlMode.COMMON:
                UsrControlCommon();
                break;
            case ControlMode.CLIMBING:
                UsrControlClimbing();
                break;
        }
        SetF();//水平阻力
    }

    private void UsrControlCommon() {//一般玩家交互
        if (Input.GetKeyUp(GMManager.LEFT_KEY)) {
            state.isOnMove = false;
        }
        if (Input.GetKeyUp(GMManager.RIGHT_KEY)) {
            state.isOnMove = false;
        }
        if (Input.GetKey(GMManager.LEFT_KEY) && Input.GetKey(GMManager.RIGHT_KEY)) {
            state.isOnMove = false;
        }

        else if (Input.GetKey(GMManager.LEFT_KEY)) {
            Move(UsrState.LEFT_DIR);//左移动
        }
        else if (Input.GetKey(GMManager.RIGHT_KEY)) {
            Move(UsrState.RIGHT_DIR);//右移动
        }
        if (Input.GetKeyDown(GMManager.DOWN_KEY)) {
            Slide();
        }
        if (Input.GetKeyDown(GMManager.JUMP_KEY)) {
            Jump();//跳跃
        }
        if (Input.GetKeyDown(GMManager.DASH_KEY)) {
            Dash();//瞬移
        }
        if (Input.GetKeyDown(GMManager.DOOR_KEY)) {
            Create();
        }
    }

    private void UsrControlClimbing() {//爬墙交互
        if (Input.GetKey(GMManager.LEFT_KEY)) {
            if (!state.IsNearWall(UsrState.LEFT_DIR) || state.isOnGround)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetKey(GMManager.LEFT_KEY)) {
            if (state.OnClimb == UsrState.LEFT_DIR)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (Input.GetKey(GMManager.RIGHT_KEY)) {
            if (!state.IsNearWall(UsrState.RIGHT_DIR) || state.isOnGround)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetKey(GMManager.RIGHT_KEY)) {
            if (state.OnClimb == UsrState.RIGHT_DIR)
                ChangeControlMode(ControlMode.COMMON);
        }

        if (Input.GetKeyDown(GMManager.JUMP_KEY)) {
            ClimbingJump();
        }

        if (Input.GetKeyDown(GMManager.DASH_KEY)) {
            ClimbingDash();
        }
    }

    public void ChangeControlMode(ControlMode cm, int param = 0) {
        switch (cm) {
            case ControlMode.COMMON:
                state.dustEffect.CancelEffect();
                state.ResetClimbCounter();
                state.rigidbody2D.gravityScale = startG;
                break;
            case ControlMode.CLIMBING:
                state.dustEffect.SetEffect();
                state.isOnMove = false;
                state.rigidbody2D.velocity = new Vector2(0, -state.climbSlideSpeed);
                state.rigidbody2D.gravityScale = 0;
                state.OnClimb = param;
                break;
        }
        currentMode = cm;
    }

    #region ACT

    public void Jump() {
        if (!state.isJumpEnable)
            return;
        state.isJumpEnable = false;
        state.animator.SetBool("isJump", true);
    }

    public void ClimbingJump() {
        if (state.isJumpEnable && state.currentAni.IsName("climbing") && state.currentManualPower >= state.ManualCostOnClimbingJump) {
            state.animator.SetBool("isJump", true);
            state.currentManualPower -= state.ManualCostOnClimbingJump;
        }
    }

    public void Move(int moveDir) {
        state.isOnMove = true;
        state.animator.SetBool("isRun", true);
        if (moveDir == UsrState.LEFT_DIR) {
            state.currentDir = new Vector2(-1, 0);
            if (state.IsNearWall(UsrState.LEFT_DIR) && state.isClimbEnable) {
                ChangeControlMode(ControlMode.CLIMBING, moveDir);
            }
        }
        if (moveDir == UsrState.RIGHT_DIR) {
            state.currentDir = new Vector2(1, 0);
            if (state.IsNearWall(UsrState.RIGHT_DIR) && state.isClimbEnable) {
                ChangeControlMode(ControlMode.CLIMBING, moveDir);
            }
        }
        if (Mathf.Abs(state.rigidbody2D.velocity.x) < state.maxMoveSpeed) {
            state.rigidbody2D.AddForce(state.currentDir * state.moveForceValue, ForceMode2D.Force);
        }
    }

    public void Dash() {
        if (!state.animator.GetBool("isDash") && state.currentAni.IsName("run") && state.isOnMove && state.nearDoor != 0) {
            //
            dashTarget = state.GetDoor();
            if (dashTarget == null||!dashTarget.IsEnable())
                return;

            if (Mathf.Abs(transform.position.y - dashTarget.transform.position.y) <= state.doorDistance) {
                state.animator.SetBool("isDash", true);
                state.rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    public bool Create() {
        if (state.goldenKeyTime <= 0 && state.greenKey <= 0) {
            GetComponent<UsrAniCallBack>().ErrorSound();
            return false;
        }
        Vector2 colliderPos = new Vector2(transform.position.x, transform.position.y) + state.boxCollider2D.offset;
        Vector2 endColliderPos = colliderPos + state.currentDir * state.createDistance - new Vector2(0, -0.1f);
        Collider2D hit = Physics2D.OverlapBox(endColliderPos, state.boxCollider2D.size, 0,
            1 << LayerMask.NameToLayer("solid") | 1 << LayerMask.NameToLayer("wall") | 1 << LayerMask.NameToLayer("door"));
        if (hit != null) {
            GetComponent<UsrAniCallBack>().ErrorSound();
            return false;
        }
        if (myCreate != null) {
            state.CheckDoor(myCreate);
            myCreate.DisAppear();
        }
        GameObject temp = Instantiate(pbDoor, (Vector2)transform.position + state.currentDir * state.createDistance, Quaternion.identity);
        temp.transform.parent = GameObject.Find("MapManager").transform;
        myCreate = temp.GetComponent<MapDoor>();
        myCreate.SetColorA(0);
        myCreate.dashDistance = 3.0f;
        if (state.goldenKeyTime > 0) {
            myCreate.reCollectable = false;
        }
        else {
            myCreate.reCollectable = true;
            state.UseGreenKey();
        }
        return true;
    }

    public void ClimbingDash() {
        if (!state.currentAni.IsName("climbing"))
            return;

        if (state.nearWall == UsrState.LEFT_DIR)
            state.currentDir = new Vector2(1, 0);
        if (state.nearWall == UsrState.RIGHT_DIR)
            state.currentDir = new Vector2(-1, 0);

        if (!Create()) {
            if (state.nearWall == UsrState.LEFT_DIR)
                state.currentDir = new Vector2(-1, 0);
            if (state.nearWall == UsrState.RIGHT_DIR)
                state.currentDir = new Vector2(1, 0);
            return;
        }    

        dashTarget = myCreate;
        if (Mathf.Abs(transform.position.y - dashTarget.transform.position.y) <= state.doorDistance) {
            myCreate.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            ChangeControlMode(ControlMode.COMMON);
            state.animator.SetBool("isDash", true);
            state.rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
        else {
            if (state.nearWall == UsrState.LEFT_DIR)
                state.currentDir = new Vector2(-1, 0);
            if (state.nearWall == UsrState.RIGHT_DIR)
                state.currentDir = new Vector2(1, 0);
        }
    }

    public void Slide() {
        if (!state.animator.GetBool("isSlide") && state.currentAni.IsName("run") && state.isOnMove && state.currentManualPower >= state.ManualCostOnSlide) {
            state.animator.SetBool("isSlide", true);
            state.currentManualPower -= state.ManualCostOnSlide;
        }
    }

    #endregion

    private bool IsInUnActAni() => state.currentAni.IsName("dashing") ||
            state.currentAni.IsName("dash_on") ||
            state.currentAni.IsName("dash_off") ||
            state.currentAni.IsName("sliding") ||
            state.currentAni.IsName("slide_on") ||
            state.currentAni.IsName("slide_off") ||
            state.currentAni.IsName("climb_on") ||
            state.currentAni.IsName("climb_off") ||
            state.currentAni.IsName("climbing_jump") ||
            //            state.currentAni.IsName("land") ||
            state.currentAni.IsName("jump") ||
            state.animator.GetBool("isDash");

    private void SetF() {
        if (state.rigidbody2D.velocity.x > 0) {
            state.rigidbody2D.velocity -= new Vector2(state.airDragA, 0);
            if (state.rigidbody2D.velocity.x < 0)
                state.rigidbody2D.velocity = new Vector2(0, state.rigidbody2D.velocity.y);
        }
        else if (state.rigidbody2D.velocity.x < 0) {
            state.rigidbody2D.velocity += new Vector2(state.airDragA, 0);
            if (state.rigidbody2D.velocity.x > 0)
                state.rigidbody2D.velocity = new Vector2(0, state.rigidbody2D.velocity.y);
        }
    }

    public void LimitV() {//限制最大移速
        if (state.rigidbody2D.velocity.x > state.maxMoveSpeed) {
            float tempy = state.rigidbody2D.velocity.y;
            state.rigidbody2D.velocity = new Vector2(state.maxMoveSpeed, tempy);
        }
        else if (state.rigidbody2D.velocity.x < -state.maxMoveSpeed) {
            float tempy = state.rigidbody2D.velocity.y;
            state.rigidbody2D.velocity = new Vector2(-state.maxMoveSpeed, tempy);
        }
    }

    public void Stand() {
        state.isControlEnable = false;
        state.rigidbody2D.Sleep();
        state.isOnMove = false;
    }

}
