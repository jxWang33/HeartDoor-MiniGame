using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBoss : Doorable
{
    public float floatSpeed = 1.0f;
    public float floatOffset = 0.0f;
    public float floatDistance = 0.2f;
    public float colorRed = 0;
    public float colorRedSpeed = 2f;

    public Vector2 currentDir;
    public Vector3 centerPosition;

    public GameObject pbHandL;
    public GameObject pbHandR;
    public GameObject pbShadow;

    public Transform posL;
    public Transform posR;
    public BossHandL bossHandL;
    public BossHandR bossHandR;

    public UsrState usrState;
    public UIManager uiManager;
    public MapReborn mapReborn;

    public int maxHp = 200;
    public int hp;

    public float lrInter = 0.5f;
    public float lrCounter = 0;
    public float readyInter = 5;
    public float readyCounter = 0;
    public float attackInter = 2;
    public float attackCounter = 0;

    void Awake() {
        hp = maxHp;
        readyCounter = readyInter;
        lrCounter = lrInter;
        attackCounter = attackInter;
    }

    void Update() {
        UpdateColor();
        UpdateCounter();
    }

    void AIControl() {
        if (hp <= 50) {
            if (readyCounter < 0) {
                readyCounter = readyInter;
                Ready();
                AttackShadow();
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

        else if (hp <= 100) {
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

        else if (hp <= 150) {
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

        else {                        
            if (readyCounter < 0) {
                readyCounter = readyInter;
                AttackShadow();
            }
        }
    }

    private void FixedUpdate() {
        UpdatePos();
        AIControl();
    }

    #region ACT
    public void AttackL() {
        if (bossHandL != null) {
            bossHandL.animator.SetBool("isReady", true);
            bossHandL = null;
        }
    }
    public void AttackR() {
        if (bossHandR != null) {
            bossHandR.animator.SetBool("isReady", true);
            bossHandR = null;
        }
    }
    public void AttackShadow() {
        Vector3 startPos = Vector2.zero;
        startPos = usrState.transform.position - Camera.main.transform.position;
        startPos.z = 10;
        if (currentDir == new Vector2(1, 0)) {
            startPos.x = -12;
        }
        if (currentDir == new Vector2(-1, 0)) {
            startPos.x = 12;
        }
        GameObject tempObject = Instantiate(pbShadow);
        tempObject.GetComponent<BlackShadow>().speed = 15;
        tempObject.transform.parent = Camera.main.transform;
        tempObject.transform.localPosition = startPos;
        if (currentDir == new Vector2(1, 0)) {
            tempObject.GetComponent<BlackShadow>().Set(new Vector2(1, 0), 20);
        }
        if (currentDir == new Vector2(-1, 0)) {
            tempObject.GetComponent<BlackShadow>().Set(new Vector2(-1, 0), 20);
        }
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
        if (hp == 150) {
            pbHandL.GetComponent<BossHandL>().attackSpeed = 300;
            pbHandR.GetComponent<BossHandR>().attackSpeed = 300;
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("哈哈哈哈，还不错啊！",DialogueKind.Sound),
                    new DialogueUnit("接下来我可不会手软了！",DialogueKind.Sound)
                };
            uiManager.SetDialogues(temp);
        }
        if (hp == 100) {
            pbHandL.GetComponent<BossHandL>().attackSpeed = 600;
            pbHandR.GetComponent<BossHandR>().attackSpeed = 600;
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("不可能！",DialogueKind.Sound),
                    new DialogueUnit("你什么时候这么坚定了",DialogueKind.Sound)
                };
            uiManager.SetDialogues(temp);
        }
        if (hp == 50) {
            readyInter = 3;
            attackInter = 1;
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("有必要这样吗！",DialogueKind.Sound),
                    new DialogueUnit("你怎么可能战胜我！！",DialogueKind.Sound)
                };
            uiManager.SetDialogues(temp);
        }
        if (hp <= 0) {
            mapReborn.Set();
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

    private void UpdatePos() {
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

    private void UpdateCounter() {
        if (lrCounter > 0 && !bossHandL && bossHandR) {
            lrCounter -= Time.deltaTime;
        }
        if (readyCounter > 0 && !bossHandL && !bossHandR) {
            readyCounter -= Time.deltaTime;
        }
        if (attackCounter > 0 && bossHandL) {
            attackCounter -= Time.deltaTime;
        }
    }

    public override void OnDoorHurt(Vector2 dir) {
        ChangeHp(-10);
        Hurted();
    }
}
