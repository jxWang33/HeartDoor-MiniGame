using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EasyBoss : MonoBehaviour
{
    public Vector2 currentDir;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    public AnimatorStateInfo currentAni;
    public AnimationClip fire;

    public int maxHp = 100;
    public int hp;
    public float colorRed = 0;
    public float colorRedSpeed = 2f;

    [SerializeField]
    private float maxMoveSpeed = 2;
    [SerializeField]
    private float moveForceValue = 5;
    [SerializeField]
    private float hurtForceValue = 5;
    [SerializeField]
    private float airDragA = 0.5f;

    [SerializeField]
    private float moveTime = 2.0f;
    private float moveTimeCounter;

    public GameObject pbBullet;
    public GameObject barricade;

    private bool shotL = false;
    public Transform fireL;
    private bool shotR = false;
    public Transform fireR;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fire.events = null;
        AnimationEvent fireEndEvent = new AnimationEvent {
            time = fire.length,
            functionName = "FireEnd"
        };
        fire.AddEvent(fireEndEvent);

        currentDir = new Vector2(-1, 0);
        moveTimeCounter = moveTime;
        hp = maxHp;
    }
    
    void Update()
    {
        UpdateAni();
        UpdateColor();
        AIControl();
        SetF();
    }

    /// <summary>
    /// 简单AI
    /// 移动开火交替
    /// 碰到solid转头
    /// </summary>
    private void AIControl() {
        if (moveTimeCounter > 0) {
            moveTimeCounter -= Time.deltaTime;
            Move();
        }
        else if (!animator.GetBool("isFire")) {
            Fire();
        }
    }

    private void UpdateAni() {
        currentAni = animator.GetCurrentAnimatorStateInfo(0);

        if (currentDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (currentDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);

        if (GetComponent<SpriteRenderer>().sprite.name == "robot_1" && !shotL) {
            shotL = true;
            GameObject temp = Instantiate(pbBullet, fireL.position, Quaternion.identity);
            temp.GetComponent<EasyBullet>().Set(currentDir,20);
        }
        if (GetComponent<SpriteRenderer>().sprite.name == "robot_2" && !shotR) {
            shotR = true;
            GameObject temp = Instantiate(pbBullet, fireR.position, Quaternion.identity);
            temp.GetComponent<EasyBullet>().Set(currentDir, 20);
        }

        animator.SetBool("isMove", false);
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

    #region ACT

    private void Move() {
        animator.SetBool("isMove", true);
        
        rigidbody2D.AddForce(currentDir * moveForceValue, ForceMode2D.Force);
        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxMoveSpeed, maxMoveSpeed),0);
    }

    private void Fire() {
        animator.SetBool("isMove", false);
        animator.SetBool("isFire", true);
    }

    #endregion

    private void FireEnd() {
        shotL = false;
        shotR = false;
        animator.SetBool("isFire", false);

        moveTimeCounter = moveTime;
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

    public void ChangeHp(int change) {
        hp += change;
        hp = Mathf.Clamp(hp, 0, maxHp);
        if (hp <= 0) {
            if(barricade!=null)
                Destroy(barricade);
            Destroy(gameObject);
        }
    }

    public void Hurted(Vector2 hurtDir) {
        rigidbody2D.AddForce(hurtDir * hurtForceValue, ForceMode2D.Impulse);
        colorRed = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.contacts[0].normal.x == 1 && collision.transform.GetComponent<MapSolid>() != null) {
            currentDir.x = 1;
        }
        if (collision.contacts[0].normal.x == -1 && collision.transform.GetComponent<MapSolid>() != null) {
            currentDir.x = -1;
        }

        if (collision.transform.GetComponent<UsrState>() != null) {
            collision.transform.GetComponent<UsrState>().ChangeMental(-10);
            if (transform.position.x > collision.transform.position.x) 
                collision.transform.GetComponent<UsrState>().Hurted(new Vector2(1,0));
            else if (transform.position.x > collision.transform.position.x) 
                collision.transform.GetComponent<UsrState>().Hurted(new Vector2(-1, 0));
            else
                collision.transform.GetComponent<UsrState>().Hurted(currentDir);
        }
    }
    
}
