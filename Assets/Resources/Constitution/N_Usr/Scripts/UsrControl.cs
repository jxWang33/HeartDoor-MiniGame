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
    private ControlMode currentMode;
    private UsrState state;
    private readonly bool isControlEnable = true;
    private float startG;

    public float horizonA = 0.15f;//空气阻力
    public float maxMoveSpeed = 2.0f;

    void Awake()
    {
        currentMode = ControlMode.COMMON;
        state = GetComponent<UsrState>();
        startG = state.rigidbody2D.gravityScale;
    }

    void Update()
    {
        if (!isControlEnable|| IsInUnActAni())
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


    public void ChangeControlMode(ControlMode cm, int param = 0) {
        switch (cm) {
            case ControlMode.COMMON:
                state.ResetClimbCounter();
                state.rigidbody2D.gravityScale = startG;
                break;
            case ControlMode.CLIMBING:
                state.isOnMove = false;
                state.rigidbody2D.velocity = new Vector2(0, -state.climbSlideSpeed);
                state.rigidbody2D.gravityScale = 0;
                state.OnClimb = param;
                break;
        }
        currentMode = cm;
    }
    private bool IsInUnActAni() {//检查是否在不可交互动画中
        return
            state.currentAni.IsName("dashing") ||
            state.currentAni.IsName("dash_on") ||
            state.currentAni.IsName("dash_off") ||
            state.currentAni.IsName("sliding") ||
            state.currentAni.IsName("slide_on") ||
            state.currentAni.IsName("slide_off") ||
            state.currentAni.IsName("climb_on") ||
            state.currentAni.IsName("climb_off") ||
            state.currentAni.IsName("land") ||
            state.currentAni.IsName("jump");
    }

    private void UsrControlCommon() {//一般玩家交互
        if (Input.GetButtonUp("Left")) {
            state.isOnMove = false;
        }
        if (Input.GetButtonUp("Right")) {
            state.isOnMove = false;
        }
        if (Input.GetButton("Left") && Input.GetButton("Right")) {
            state.isOnMove = false;
        }

        else if (Input.GetButton("Left")) {
            Move(UsrState.LEFT_DIR);//左移动
        }
        else if (Input.GetButton("Right")) {
            Move(UsrState.RIGHT_DIR);//右移动
        }
        if (Input.GetButtonDown("Down")) {
            Slide();
        }
        if (Input.GetButtonDown("Jump")) {
            Jump();//跳跃
        }
        if (Input.GetButtonDown("Dash")) {
            Dash();//瞬移
        }
    }
    private void UsrControlClimbing() {//爬墙交互
        if (Input.GetButton("Left")) {
            if (!state.IsNearWall(UsrState.LEFT_DIR) || state.isOnGround)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetButton("Left")) {
            if (state.OnClimb == UsrState.LEFT_DIR)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (Input.GetButton("Right")) {
            if (!state.IsNearWall(UsrState.RIGHT_DIR) || state.isOnGround)
                ChangeControlMode(ControlMode.COMMON);
        }
        if (!Input.GetButton("Right")) {
            if (state.OnClimb == UsrState.RIGHT_DIR)
                ChangeControlMode(ControlMode.COMMON);
        }

        if (Input.GetButtonDown("Jump")) {
            ClimbingJump();
        }
    }
    //act
    public void Jump() {
        if (!state.isJumpEnable)
            return;
        state.isJumpEnable = false;
        state.animator.SetBool("isJump", true);
    }
    public void ClimbingJump() {
        if (state.isJumpEnable && state.currentAni.IsName("climbing")) {
            state.animator.SetBool("isJump", true);
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
        if (Mathf.Abs(state.rigidbody2D.velocity.x) < maxMoveSpeed) {
            state.rigidbody2D.AddForce(state.currentDir * state.moveForceValue, ForceMode2D.Force);
        }
    }
    public void Dash() {
        if (!state.animator.GetBool("isDash") && state.currentAni.IsName("run") && state.isOnMove && state.nearDoor != 0 && state.GetDoor().IsEnable()) {
            state.animator.SetBool("isDash", true);
        }
    }

    public void Slide() {
        if (!state.animator.GetBool("isSlide") && state.currentAni.IsName("run") && state.isOnMove) {
            state.animator.SetBool("isSlide", true);
        }
    }
    //
    private void SetF() {
        if (state.rigidbody2D.velocity.x > 0) {
            state.rigidbody2D.velocity -= new Vector2(horizonA, 0);
            if (state.rigidbody2D.velocity.x < 0)
                state.rigidbody2D.velocity = new Vector2(0, state.rigidbody2D.velocity.y);
        }
        else if (state.rigidbody2D.velocity.x < 0) {
            state.rigidbody2D.velocity += new Vector2(horizonA, 0);
            if (state.rigidbody2D.velocity.x > 0)
                state.rigidbody2D.velocity = new Vector2(0, state.rigidbody2D.velocity.y);
        }
    }
    public void LimitV() {//限制最大移速
        if (state.rigidbody2D.velocity.x > maxMoveSpeed) {
            float tempy = state.rigidbody2D.velocity.y;
            state.rigidbody2D.velocity = new Vector2(maxMoveSpeed, tempy);
        }
        else if (state.rigidbody2D.velocity.x < -maxMoveSpeed) {
            float tempy = state.rigidbody2D.velocity.y;
            state.rigidbody2D.velocity = new Vector2(-maxMoveSpeed, tempy);
        }
    }
}
