using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBoss : Doorable
{
    public UIManager uiManager;
    public TimeCounter timeCounter;

    public float floatSpeed = 1.0f;
    public float floatOffset = 0.0f;
    public float floatDistance = 0.2f;

    public UsrState usrState;
    public Vector2 currentDir;
    public Vector3 centerPosition;


    public int maxHp = 100;
    public int hp;
    public float colorRed = 0;
    public float colorRedSpeed = 2f;

    public GameObject pbHandL;
    public GameObject pbHandR;

    public BossHandL bossHandL;
    public BossHandR bossHandR;

    public Transform posL;
    public Transform posR;

    public float lrInter = 0.5f;
    private float lrCounter = 0;
    public float readyInter = 5;
    private float readyCounter = 0;
    public float attackInter = 2;
    private float attackCounter = 0;

    public Transform finalPos;
    public AudioClip audioTh;
    public MapManager mapManager;


    void Awake() {
        hp = maxHp;
        readyCounter = readyInter;
        lrCounter = lrInter;
        attackCounter = attackInter;
    }


    void Update() {
        UpdateColor();
        AIControl();
    }

    void AIControl() {
        if (lrCounter > 0) {
            lrCounter -= Time.deltaTime;
        }
        if (readyCounter > 0) {
            readyCounter -= Time.deltaTime;
        }
        if (attackCounter > 0) {
            attackCounter -= Time.deltaTime;
        }
        if (readyCounter < 0) {
            readyCounter = readyInter;
            Ready();
        }
        if (attackCounter < 0) {
            attackCounter = attackInter;
            AttackL();
        }
        if (lrCounter < 0) {
            lrCounter = lrInter;
            AttackR();
        }
    }

    private void FixedUpdate() {
        if (usrState == null)
            return; 

        currentDir = new Vector2(usrState.transform.position.x - transform.position.x, 0).normalized;

        if (currentDir == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (currentDir == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);

        
        centerPosition.y = usrState.transform.position.y;
        Vector3 tempPos = transform.position;
        tempPos.y = Mathf.Sin(Time.time * floatSpeed + floatOffset) * floatDistance + centerPosition.y;
        transform.position = tempPos;
    }

    #region ACT
    public void AttackL() {
        if(bossHandL!=null)
            bossHandL.animator.SetBool("isReady", true);
    }
    public void AttackR() {
        if (bossHandR != null)
            bossHandR.animator.SetBool("isReady", true);
    }

    public void Ready() {
        GameObject temp = Instantiate(pbHandL, transform);
        temp.transform.localPosition = posL.localPosition;
        bossHandL = temp.GetComponent<BossHandL>();
        bossHandL.centerPosition = posL.localPosition;
        bossHandL.heartBoss = this;

        temp = Instantiate(pbHandR, transform);
        temp.transform.localPosition = posR.localPosition;
        bossHandR = temp.GetComponent<BossHandR>();
        bossHandR.centerPosition = posR.localPosition;
        bossHandR.heartBoss = this;

    }
    #endregion

    public void Hurted() {
        colorRed = 1;
    }

    public void ChangeHp(int change) {
        hp += change;
        hp = Mathf.Clamp(hp, 0, maxHp);
        if (hp <= 0) {
            usrState.transform.position = finalPos.position;
            mapManager.GetComponent<AudioSource>().clip = audioTh;
            mapManager.GetComponent<AudioSource>().loop = false;
            mapManager.GetComponent<AudioSource>().Play();

            Destroy(gameObject);
        }
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

    public override void OnDoorHurt(Vector2 dir) {
        ChangeHp(-10);
        Hurted();
    }
}
