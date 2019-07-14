using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueKind {Talk,Sound,Mono};
public delegate void DialogueDelegate();

public struct DialogueUnit {
    public string content;
    public DialogueKind kind;
    public Sprite cg;
    public DialogueDelegate OnDialogueEnd;

    public DialogueUnit(string content, DialogueKind kind = DialogueKind.Talk, DialogueDelegate OnDialogueEnd = null, Sprite cg = null) {
        this.content = content;
        this.kind = kind;
        this.cg = cg;
        this.OnDialogueEnd = OnDialogueEnd;
    }
}

public class DialoguePanel : MonoBehaviour
{
    private int counter = 0;
    private DialogueUnit currentUnit;
    private List<DialogueUnit> unitList;

    public GameObject talkPanel;
    public GameObject soundPanel;
    public GameObject monoPanel;

    [SerializeField]
    private bool isTyping = false;
    private Coroutine TypingCor;
    public Text talkText;
    public Image photoImage;

    public Text soundText;

    public Text monoText;

    private void Awake() {
        unitList = new List<DialogueUnit>();
    }

    private void OnEnable() {
        counter = 0;
        talkPanel.SetActive(false);
        monoPanel.SetActive(false);
        soundPanel.SetActive(false);
    }

    private void OnDisable() {
        unitList = null;
        counter = 0;
        talkPanel.SetActive(false);
        monoPanel.SetActive(false);
        soundPanel.SetActive(false);
    }

    public void SetDialogues(List<DialogueUnit> units) {
        counter = 0;
        unitList = units;
        GoNext();
    }

    private void SetDialogue(DialogueUnit unit) {
        currentUnit = unit;
        if (unit.kind == DialogueKind.Talk) {
            talkPanel.SetActive(true);
            monoPanel.SetActive(false);
            soundPanel.SetActive(false);

            photoImage.sprite = unit.cg;
            if (photoImage.sprite == null) {
                Color tempColor = photoImage.color;
                tempColor.a = 0;
                photoImage.color = tempColor;
            }
            else {
                Color tempColor = photoImage.color;
                tempColor.a = 1;
                photoImage.color = tempColor;
            }
            TypingCor = StartCoroutine(TypeContent(unit.content));
        }
        else if (unit.kind == DialogueKind.Sound) {
            talkPanel.SetActive(false);
            monoPanel.SetActive(false);
            soundPanel.SetActive(true);

            soundText.text = unit.content;
            Invoke("GoNext", 2);
        }
        else if (unit.kind == DialogueKind.Mono) {
            talkPanel.SetActive(false);
            monoPanel.SetActive(true);
            soundPanel.SetActive(false);

            StartCoroutine(MonoContent(unit.content));
        }
    }

    private void GoNext() {
        if (counter > 0) {
            unitList[counter - 1].OnDialogueEnd?.Invoke();
        }
        if (counter >= unitList.Count) {
            gameObject.SetActive(false);
        }
        else {
            SetDialogue(unitList[counter]);
            counter++;
        }
    }

    public IEnumerator TypeContent(string content, float typeInterval = 0.05f) {
        isTyping = true;
        talkText.text = "";
        foreach (char letter in content.ToCharArray()) {
            talkText.text += letter;
            yield return new WaitForSeconds(typeInterval);
        }
        isTyping = false;
    }
    public IEnumerator MonoContent(string content, float aSpeed = 1, float lastTime = 2) {
        monoText.text = content;
        Color tempColor = monoText.color;
        tempColor.a = 0;
        monoText.color = tempColor;
        while (monoText.color.a < 1) {
            tempColor.a += aSpeed * Time.deltaTime;
            if (tempColor.a > 1)
                tempColor.a = 1;
            monoText.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(lastTime);
        while (monoText.color.a > 0) {
            tempColor.a -= aSpeed * Time.deltaTime;
            if (tempColor.a < 0)
                tempColor.a = 0;
            monoText.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(lastTime/2);
        GoNext();
    }


    private void OnFuncKey() {
        if (currentUnit.kind != DialogueKind.Talk)
            return;
        if (isTyping) {
            StopCoroutine(TypingCor);
            talkText.text = currentUnit.content;
            isTyping = false;
        }
        else
            GoNext();
    }

    private void Update() {
        if (Input.GetKeyDown(GMManager.FUNC_KEY)) {
            OnFuncKey();
        }
    }


    #region Photos
    public Sprite photoNHappy;
    public Sprite photoNSad;
    public Sprite photoNAngry;
    public Sprite photoNPanic;
    public Sprite photoAHappy;
    public Sprite photoASad;
    public Sprite photoBHappy;
    public Sprite photoBSad;
    public Sprite photoCHappy;
    public Sprite photoCSad;
    public Sprite photoCo;//快递员
    public Sprite photoX;//陌生人
    #endregion
}
//使用流程
//设置panel可见
//指定UnitList
//设置回调函数
//游戏暂停处理