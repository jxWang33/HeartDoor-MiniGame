using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGoodHeart : TriggerGood
{
    public int pointer = 0;
    protected override void Start() {
        base.Start();
    }

    protected override void TriggerCallBack() {
        base.TriggerCallBack();
        switch (pointer) {
            case 11:
                _1_1();
                break;
            case 12:
                _1_2();
                break;
            case 13:
                _1_3();
                break;
            default:
                break;
        }
    }

    protected override void Update() {
        base.Update();
    }

    #region GOODS

    private void _1_1() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("毕业照吗...", "小N", null),
                    new DialogueUnit("有点怀念呢。", "小N", null)
                };
        uiManager.SetDialogues(temp, () => {
            usrState.ChangeHeart(1);
            usrState.isControlEnable = true;
        });
    }

    private void _1_2() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("好久没出门运动了吧", "小N", null)
                };
        uiManager.SetDialogues(temp, () => {
            usrState.ChangeHeart(1);
            usrState.isControlEnable = true;
        });
    }

    private void _1_3() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这个椅子已经修了很多次了", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("一会扔了吧...", "小N", null)
                };
        uiManager.SetDialogues(temp, () => {
            usrState.ChangeHeart(-1);
            usrState.isControlEnable = true;
        });
    }

    #endregion

}
