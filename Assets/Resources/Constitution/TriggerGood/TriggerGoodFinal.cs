using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TriggerGoodFinal : TriggerGood
{
    public LoadingPanel loadingPanel;
    public TimeCounter timeCounter;
    public int pointer = 1;

    protected override void Start() {
        base.Start();
    }

    protected override void TriggerCallBack() {
        base.TriggerCallBack();
        switch (pointer) {
            case 1:
                _1();
                break;
            case 2:
                _2();
                break;
            case 3:
                _3();
                break;
            case 4:
                _4();
                break;
            case 5:
                _5();
                break;
            default:
                break;
        }
    }
    
    protected override void Update()
    {
        base.Update();

    }

    #region LEVELS

    void _1() {
        if (timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("喂，是小N吗？毕业后这几年混的怎么样啦？国庆节的时候，大家好像都要回学校看看呢，之后正好我们几个一起聚一聚吧！", "小A", null),
                    new DialogueUnit("对啊，对啊。都好久没见了，不知道大家都变成什么样了.", "小B", null),
                    new DialogueUnit("呃....", "小N", null),
                    new DialogueUnit("还是算了吧，我还有好多事情。", "小N", null),
                    new DialogueUnit("这样啊，本来还很期待看到小N的，那还真是遗憾呢...", "小A", null),
                    new DialogueUnit("嘟~嘟~嘟~", "电话", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("有点困了。", "小N", null),
                    new DialogueUnit("就在这里睡吧。", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.ChangeHeart(2);
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.SetAndSave(GMManager.LEVEL_2, usrState.myHeart);
            });
        }
        if (timeCounter.minute < 30) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("喂，是小N吗？毕业后这几年混的怎么样啦？国庆节的时候，大家好像都要回学校看看呢，之后正好我们几个一起聚一聚吧！", "小A", null),
                    new DialogueUnit("对啊，对啊。都好久没见了，不知道大家都变成什么样了.", "小B", null),
                    new DialogueUnit("呃....", "小N", null),
                    new DialogueUnit("还是算了吧，我还有好多事情。", "小N", null),
                    new DialogueUnit("这样啊，本来还很期待看到小N的，那还真是遗憾呢...", "小A", null),
                    new DialogueUnit("嗞嗞嗞——", "电话", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("没信号了吗。", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("有点困了。", "小N", null),
                    new DialogueUnit("就在这里睡吧。", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.ChangeHeart(1);
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.SetAndSave(GMManager.LEVEL_2, usrState.myHeart);
            });
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("电话挂断了呢....", "小N", null),
                    new DialogueUnit("是接的太晚了吗....", "小N", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.Set(GMManager.BadEnd_1);
            });
        }
    }

    void _2() {
        if (timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("你好，你的快递，请签收。", "快递员", null),
                    new DialogueUnit("嗯。", "小N", null),
                    new DialogueUnit("拆开-", "小N", null),
                    new DialogueUnit("呃，一个鳗鱼玩偶。", "小N", null),
                    new DialogueUnit("......", "小A", null),
                    new DialogueUnit("很精致。", "小N", null),
                    new DialogueUnit("不过是谁送来的呢。", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("并没有写发件人的姓名。", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("好像又有点困了。", "小N", null),
                    new DialogueUnit("就在这里睡吧。", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.ChangeHeart(2);
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.SetAndSave(GMManager.LEVEL_3, usrState.myHeart);
            });
        }
        if (timeCounter.minute < 30) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("你好，你的快递，请签收。", "快递员", null),
                    new DialogueUnit("嗯。", "小N", null),
                    new DialogueUnit("拆开-", "小N", null),
                    new DialogueUnit("呃，一个鳗鱼玩偶。", "小N", null),
                    new DialogueUnit("......", "小A", null),
                    new DialogueUnit("有点皱皱巴巴的了。", "小N", null),
                    new DialogueUnit("不过是谁送来的呢。", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("并没有写发件人的姓名。", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("好像又有点困了。", "小N", null),
                    new DialogueUnit("就在这里睡吧。", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.ChangeHeart(1);
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.SetAndSave(GMManager.LEVEL_3, usrState.myHeart);
            });
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("额......", "小N", null),
                    new DialogueUnit("等太久走掉了吗。", "小N", null),
                    new DialogueUnit("开门-", "小N", null),
                    new DialogueUnit("门口这个就是快递吗。", "小N", null),
                    new DialogueUnit("是个鳗鱼玩偶。破破烂烂的。", "小A", null),
                    new DialogueUnit("是谁的恶作剧吗。", "小N", null),
                    new DialogueUnit("好像又有点困了。", "小N", null),
                    new DialogueUnit("明天再说吧。", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 1) 
                    loadingPanel.Set(GMManager.BadEnd_2);
                else
                    loadingPanel.SetAndSave(GMManager.LEVEL_3, usrState.myHeart);
            });
        }
    }

    void _3() {
        if (timeCounter.minute <5) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这里是尽头了吗。", "小N", null),
                    new DialogueUnit(".......", "小N", null),
                    new DialogueUnit("手机好像在放什么视频...", "小N", null),
                    new DialogueUnit("查看——", "小N", null),
                    new DialogueUnit("小N这几个月不知道怎么了，一直把自己关在家里。去看他的时候也感觉他阴沉沉的", "小C", null),
                    new DialogueUnit("欸，是吗。怪不得没有他的消息", "小A", null),
                    new DialogueUnit("可能是长时间工作，很少有机会与人接触的原因吧。", "小B", null),
                    new DialogueUnit("......", "沉默", null),
                    new DialogueUnit("那我们想想办法帮帮他吧", "小A", null),
                    new DialogueUnit("画面中小A拿出手机", "小A", null),

                    new DialogueUnit("叮铃铃~", "手机", null),
                    new DialogueUnit("喂，是小N吗？你还好吗？听说你前段时间离职了，之后就一直待在家里不出门了，真担心你啊。我和几个小伙伴明天打算过来看看你……", "小A", null),
                    new DialogueUnit("嗞嗞嗞——", "电话", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 4)
                    loadingPanel.Set(GMManager.NormalEnd);
                else {
                    usrState.ChangeHeart(2);
                    loadingPanel.SetAndSave(GMManager.LEVEL_4, usrState.myHeart);
                }
            });
        }
        if (timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这里是尽头了吗。", "小N", null),
                    new DialogueUnit(".......", "小N", null),
                    new DialogueUnit("手机好像在放什么视频...", "小N", null),
                    new DialogueUnit("查看——", "小N", null),
                    new DialogueUnit("那我们想想办法帮帮他吧", "小A", null),
                    new DialogueUnit("画面中小A拿出手机", "小A", null),

                    new DialogueUnit("叮铃铃~", "手机", null),
                    new DialogueUnit("喂，是小N吗？你还好吗？听说你前段时间离职了，之后就一直待在家里不出门了，真担心你啊。我和几个小伙伴明天打算过来看看你……", "小A", null),
                    new DialogueUnit("嗞嗞嗞——", "电话", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 4)
                    loadingPanel.Set(GMManager.NormalEnd);
                else {
                    usrState.ChangeHeart(1);
                    loadingPanel.SetAndSave(GMManager.LEVEL_4, usrState.myHeart);
                }
            });
        }
        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("叮铃铃~", "手机", null),
                    new DialogueUnit("喂，是小N吗？你还好吗？听说你前段时间离职了，之后就一直待在家里不出门了，真担心你啊。我和几个小伙伴明天打算过来看看你……", "小A", null),
                    new DialogueUnit("嗞嗞嗞——", "电话", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 4)
                    loadingPanel.Set(GMManager.NormalEnd);
                else {
                    loadingPanel.SetAndSave(GMManager.LEVEL_4, usrState.myHeart);
                }
            });
        }
    }

    void _4() {
        if (timeCounter.minute < 1) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit(".......", "小N", null),
                    new DialogueUnit("真的要开门吗。", "小N", null),
                    new DialogueUnit("现在的生活也不错。", "小N", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.ChangeHeart(2);
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 10)
                    loadingPanel.Set(GMManager.NormalEnd);
                else
                    loadingPanel.SetAndSave(GMManager.LEVEL_5, usrState.myHeart);
            });
        }
        if (timeCounter.minute < 2) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit(".......", "小N", null),
                    new DialogueUnit("真的要开门吗。", "小N", null),
                    new DialogueUnit("现在的生活也不错。", "小N", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.ChangeHeart(1);
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 10)
                    loadingPanel.Set(GMManager.NormalEnd);
                else
                    loadingPanel.SetAndSave(GMManager.LEVEL_5, usrState.myHeart);
            });
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit(".......", "小N", null),
                    new DialogueUnit("真的要开门吗。", "小N", null),
                    new DialogueUnit("现在的生活也不错。", "小N", null),
                    new DialogueUnit("......", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                if (usrState.myHeart < 10)
                    loadingPanel.Set(GMManager.NormalEnd);
                else
                    loadingPanel.SetAndSave(GMManager.LEVEL_5, usrState.myHeart);
            });
        }
    }

    void _5() {
        if (timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("果然还是想和他们在一起。", "小N", null),
                    new DialogueUnit("吱——", "开门", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.SetAndSave(GMManager.HappyEnd, usrState.myHeart);
            });
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("等太久走掉了吗。", "小N", null),
                    new DialogueUnit("......", "小N", null),
                    new DialogueUnit("有一张纸条。", "小N", null),
                    new DialogueUnit("我们都很担心你，这些天的东西都是我们送的，希望没有吓到你，也希望你能快点好起来。", "小A", null),
                    new DialogueUnit("抱歉这么晚打扰你，这次很遗憾没有见到你，希望明年可以见到。", "小N", null)
                };
            uiManager.SetDialogues(temp, () => {
                usrState.isControlEnable = true;
                loadingPanel.gameObject.SetActive(true);
                loadingPanel.Set(GMManager.NormalEnd);
            });
        }
    }
    #endregion
}
