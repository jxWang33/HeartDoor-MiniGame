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
            case 21:
                _2_1();
                break;
            case 22:
                _2_2();
                break;
            case 23:
                _2_3();
                break;
            default:
                break;
        }
    }

    protected override void Update() {
        base.Update();
    }

    #region DAY1_GOODS

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

    #region DAY2_GOODS

    private void _2_1() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("好漂亮的画。", "小N", null),
                    new DialogueUnit("仔细查看--", "小N", null),
                    new DialogueUnit("偶尔出去走走说不定也不错。", "小N", null)
                };
        uiManager.SetDialogues(temp, () => {
            usrState.ChangeHeart(2);
            usrState.isControlEnable = true;
        });
    }

    private void _2_2() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("好漂亮的盆栽。", "小N", null),
                    new DialogueUnit("等我闲下来把它从地下室拿出去吧。", "小N", null)
                };
        uiManager.SetDialogues(temp, () => {
            usrState.ChangeHeart(1);
            usrState.isControlEnable = true;
        });
    }

    private void _2_3() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("唔- 好痛", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("这个玻璃杯是梦游时碰碎的吗。", "小N", null),
                    new DialogueUnit("真可惜......", "小N", null)
                };
        uiManager.SetDialogues(temp, () => {
            usrState.ChangeHeart(-2);
            usrState.isControlEnable = true;
        });
    }

    #endregion

}
