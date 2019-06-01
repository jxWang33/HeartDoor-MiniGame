using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct DialogueUnit {
    public string content;
    public string name;
    public Sprite cg;

    public DialogueUnit(string content,string name,Sprite cg) {
        this.content = content;
        this.name = name;
        this.cg = cg;
    }
}

public delegate void DialogueDelegate();

public class DialoguePanel : MonoBehaviour
{
    public Text nameText;
    public Text contentText;
    public Image cgImage;


    public DialogueDelegate OnDialogueEnd = null;

    [SerializeField]
    private bool isTyping = false;

    private string currentContent;

    private Coroutine TypingCor;

    private List<DialogueUnit> unitList;

    private int counter = 0;

    private void Awake() {
        unitList = new List<DialogueUnit>();
    }

    private void OnEnable() {
        counter = 0;
    }

    private void OnDisable() {
        unitList = null;
        OnDialogueEnd = null;
        counter = 0;
    }

    public void SetDialogues(List<DialogueUnit> units) {
        counter = 0;
        unitList = units;
        GoNext();
    }

    private void SetDialogue(DialogueUnit unit) {
        nameText.text = unit.name;
        cgImage.sprite = unit.cg;
        if (cgImage.sprite == null)
            cgImage.color = new Color(0, 0, 0, 0);
        else
            cgImage.color = new Color(0, 0, 0, 1);
        isTyping = true;
        TypingCor = StartCoroutine(TypeContent(unit.content));
        currentContent = unit.content;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            OnClick();
        }
    }

    private void OnClick() {
        if (isTyping) {
            StopCoroutine(TypingCor);
            contentText.text = currentContent;
            isTyping = false;
        }
        else {
            GoNext();
        }
    }

    private void GoNext() {
        if (counter >= unitList.Count) {
            OnDialogueEnd?.Invoke();
            gameObject.SetActive(false);
        }
        else {
            SetDialogue(unitList[counter]);
            counter++;
        }
    }

    public IEnumerator TypeContent(string content, float typeInterval = 0.05f) {
        contentText.text = "";
        foreach (char letter in content.ToCharArray()) {
            contentText.text += letter;
            yield return new WaitForSeconds(typeInterval);
        }
        isTyping = false;
    }

}
//使用流程
//设置panel可见
//指定UnitList
//设置回调函数
//游戏暂停处理