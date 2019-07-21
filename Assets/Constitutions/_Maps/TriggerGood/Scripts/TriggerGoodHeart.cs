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
            case 31:
                _3_1();
                break;
            case 32:
                _3_2();
                break;
            case 33:
                _3_3();
                break;
            case 41:
                _4_1();
                break;
            case 42:
                _4_2();
                break;
            case 43:
                _4_3();
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
                    new DialogueUnit("这不是当年的毕业照吗...",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那时候真快乐呢。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("要是可以的话，真想回到那个时候。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我好害怕现在的自己。", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("心 +1", DialogueKind.Sound,() => {
                        usrState.ChangeHeart(1);
                        usrState.isControlEnable = true;
                    })
                };
        uiManager.SetDialogues(temp);
    }

    private void _1_2() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("好久没出门运动了吧。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这个网球拍上都全是灰尘了。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不知道我还会不会打。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("心 +1", DialogueKind.Sound, () => {
                        usrState.ChangeHeart(1);
                        usrState.isControlEnable = true;
                    })
                };
        uiManager.SetDialogues(temp);
    }

    private void _1_3() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这个椅子已经修了很多次了。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("为什么他还是坏掉了！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNAngry),
                    new DialogueUnit("为什么我这么努力都修不好他？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNAngry),
                    new DialogueUnit("为什么坏的偏偏是我的椅子？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("心 -1", DialogueKind.Sound, () => {
                        usrState.ChangeHeart(-1);
                        usrState.isControlEnable = true;
                    })
                };
        uiManager.SetDialogues(temp);
    }

    #endregion

    #region DAY2_GOODS

    private void _2_1() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这幅画画的真棒。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("真想住在这幅画里。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("很安静。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("偶尔出去走走说不定也不错。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("心 +2", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(2);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    private void _2_2() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("好漂亮的盆栽。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("等我闲下来把它从地下室拿出去吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("心 +1", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(1);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    private void _2_3() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("唔- 好痛。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这个玻璃杯也是梦游时碰碎的吗...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("难道我还不够辛苦吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("为什么我还要遭受这种痛苦？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("心 -2", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(-2);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    #endregion

    #region DAY3_GOODS

    private void _3_1() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("好久没听磁带了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这里面的歌以前经常听。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("很好听。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("心 +2", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(2);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    private void _3_2() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("我记得这个奖杯是运动会时拿到的。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("当时跟大家在一起很开心呢。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("心 +1", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(1);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    private void _3_3() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这不是从地下室拿出来的盆栽吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这也许是个错误。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("心 -2", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(-2);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    #endregion

    #region DAY4_GOODS

    private void _4_1() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这个帽子真酷。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("不知道还能不能戴上。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("心 +1", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(1);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    private void _4_2() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这不是小C的欠条吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("好像是有这么一回事。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("心 -2", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(-2);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    private void _4_3() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("离职申请吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("工作什么的，真无聊。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("心 -3", DialogueKind.Sound,() => {
                            usrState.ChangeHeart(-3);
                            usrState.isControlEnable = true;
                    })
        };
        uiManager.SetDialogues(temp);
    }

    #endregion
}
