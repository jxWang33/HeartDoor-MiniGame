using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestControl : MonoBehaviour
{
    public float moveForceValue = 0.01f;
    public float jumpForceValue = 0.01f;
    public float maxMoveSpeed = 1f;
    public float horizonA = 0.5f;
    [SerializeField]
    private bool isOnGround = false;
    [SerializeField]
    private bool isOnMove = false;
    [SerializeField]
    private bool isOnWall = false;
    private List<TestWall> touchedWalls;
    private new Rigidbody2D rigidbody2D;
    private Vector2 currentDir;
    private CapsuleCollider2D capsuleCollider2D;
    void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        touchedWalls = new List<TestWall>();
        currentDir = new Vector2(0, 1);
    }

    // Update is called once per frame
    void Update() {
        UsrControl();
        UpdateOnGround();
        UpdateOnMove();
        UpdateOnWall();
        UpdateAni();
        Check();
    }
    private void Check() {//更新合法值
        if (Mathf.Abs(rigidbody2D.velocity.x) > maxMoveSpeed) {
            float tempy = rigidbody2D.velocity.y;
            rigidbody2D.velocity = maxMoveSpeed * currentDir + new Vector2(0, tempy);
        }
    }
    private void UpdateOnGround() {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, capsuleCollider2D.size.y / 2 + 0.1f, 1 << 8);
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
        //水平阻力
        if (isOnMove) {
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
    private void UpdateOnWall() {
        if (touchedWalls.Count != 0) 
            isOnWall = true;
        else
            isOnWall = false;
    }

    private void UpdateAni() {
        if (rigidbody2D.velocity.x > 0 && Mathf.Abs(rigidbody2D.velocity.x) > 0.01f)
            transform.localScale = new Vector3(1,1,1);
        else if (rigidbody2D.velocity.x < 0 && Mathf.Abs(rigidbody2D.velocity.x) > 0.01f)
            transform.localScale = new Vector3(-1,1,1);

        if (Mathf.Abs(rigidbody2D.velocity.x)>0.5)
            GetComponent<Animator>().SetBool("isRun",true);
        else
            GetComponent<Animator>().SetBool("isRun", false);
    }

    private void UsrControl() {//玩家交互
        if (Input.GetKey(KeyCode.A)) {//左移动
            currentDir = new Vector2(-1, 0);
            rigidbody2D.AddForce(currentDir * moveForceValue, ForceMode2D.Force);
        }
        if (Input.GetKey(KeyCode.D)) {//右移动
            currentDir = new Vector2(1, 0);
            rigidbody2D.AddForce(currentDir * moveForceValue, ForceMode2D.Force);
        }
        if (Input.GetKeyDown(KeyCode.W)) {//跳跃
            if (isOnGround) {
                Vector2 forceDir = new Vector2(0, 1);
                rigidbody2D.AddForce(forceDir * jumpForceValue, ForceMode2D.Impulse);
                isOnGround = false;
            }
        }
        if (Input.GetKey(KeyCode.S)) {//下蹲
            //
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == -1) {//左边碰撞
            if (collision.collider.GetComponent<TestWall>())
                touchedWalls.Add(collision.collider.GetComponent<TestWall>());
        }
        else if (collision.contacts[0].normal.x == 1) {//右边碰撞
            if (collision.collider.GetComponent<TestWall>())
                touchedWalls.Add(collision.collider.GetComponent<TestWall>());
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.GetComponent<TestWall>()) {
                touchedWalls.Remove(collision.transform.GetComponent<TestWall>());
        }
    }
}
