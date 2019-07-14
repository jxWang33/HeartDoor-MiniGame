using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TriggerGoodFinal : TriggerGood
{
    public int pointer = 1;
    public MapManager mapManager;
    public LevelScript1 level1Manager;

    protected override void Start() {
        base.Start();
    }

    protected override void TriggerCallBack() {
        uiManager.usrStatePanel.gameObject.SetActive(false);
        uiManager.timeCounter.gameObject.SetActive(false);
        switch (pointer) {
            case 1:
                _1();
                break;
        //    case 2:
        //        _2();
        //        break;
        //    case 3:
        //        _3();
        //        break;
        //    case 4:
        //        _4();
        //        break;
        //    case 5:
        //        _5();
        //        break;
        //    default:
        //        break;
        }
    }
    
    protected override void Update()
    {
        base.Update();
    }

    #region LEVELS

    void _1() {
        if (uiManager.timeCounter.minute < 2) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("喂，是小N吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("...嗯，是我。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你是?", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("听不出来我的声音了吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("哈哈哈也难怪，我们都一年没见了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("我是小B啊。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("小B？", DialogueKind.Mono),
                    new DialogueUnit("他不是我的大学同学吗？", DialogueKind.Mono),
                    new DialogueUnit("他找我做什么？", DialogueKind.Mono),
                    new DialogueUnit("肯定是件很麻烦的事。", DialogueKind.Mono),
                    new DialogueUnit("哦，我听出来了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你是小B。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那么，有什么事情吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("嗯，没什么大事。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("就是最近刚好国庆节。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("我想回学校看看。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("听小C说你留在了本地。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("想问一下你有没有时间一起回学校。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("小A和小C都会去的。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("回学校吗。", DialogueKind.Mono),
                    new DialogueUnit("听起来不错。", DialogueKind.Mono),
                    new DialogueUnit("不过一起去又能做些什么？", DialogueKind.Mono),
                    new DialogueUnit("见到他们能聊些什么？", DialogueKind.Mono),
                    new DialogueUnit("不过，万一......", DialogueKind.Mono),
                    new DialogueUnit("万一遇到什么有趣的事情。", DialogueKind.Mono),
                    new DialogueUnit("万一是一场愉快的旅行呢？", DialogueKind.Mono),
                    new DialogueUnit("嗯，我...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("别说傻话了！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你现在有什么成就？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你哪有那么多空闲的时间！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你去那里能改变什么？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("放弃吧！你不适合他们！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("嗯？小N，你说什么了吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("啊。我是说...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我是说我最近刚好有其他事情要忙。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这次就算了吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("欸？这样吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBSad),
                    new DialogueUnit("很遗憾呢。", DialogueKind.Talk, ()=>{
                            level1Manager.PlayHangSound();
                        },uiManager.dialoguePanel.photoBSad),
                    new DialogueUnit("挂断——", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("国庆节了吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("好累。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("有点困了，就在这里睡吧。", DialogueKind.Talk,() => {
                            usrState.ChangeHeart(2);
                            StartCoroutine(Level1End(5));
                        },uiManager.dialoguePanel.photoNSad)
                    };
                uiManager.SetDialogues(temp);
            }
        else if (uiManager.timeCounter.minute < 5) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("喂，是小N吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("...嗯，是我。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你是?", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("听不出来我的声音了吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("哈哈哈也难怪，我们都一年没见了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("我是小B啊。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("小B？", DialogueKind.Mono),
                    new DialogueUnit("他不是我的大学同学吗？", DialogueKind.Mono),
                    new DialogueUnit("他找我做什么？", DialogueKind.Mono),
                    new DialogueUnit("肯定是件很麻烦的事。", DialogueKind.Mono),
                    new DialogueUnit("哦，我听出来了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你是小B。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那么，有什么事情吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("嗯，没什么大事。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("就是最近刚好国庆节。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("我想回学校看看。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("听小C说你留在了本地。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("想问一下你有没有时间一起回学校。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("小A和小C都会去的。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("回学校吗。", DialogueKind.Mono),
                    new DialogueUnit("听起来不错。", DialogueKind.Mono),
                    new DialogueUnit("不过一起去又能做些什么？", DialogueKind.Mono),
                    new DialogueUnit("见到他们能聊些什么？", DialogueKind.Mono),
                    new DialogueUnit("不过，万一......", DialogueKind.Mono),
                    new DialogueUnit("万一遇到什么有趣的事情。", DialogueKind.Mono),
                    new DialogueUnit("万一是一场愉快的旅行呢？", DialogueKind.Mono),
                    new DialogueUnit("嗯，我...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("别说傻话了！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你现在有什么成就？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你哪有那么多空闲的时间！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你去那里能改变什么？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("放弃吧！你不适合他们！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("嗞——嗞——", DialogueKind.Sound),
                    new DialogueUnit("喂？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("没信号了吗", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("好累。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("有点困了，就在这里睡吧。", DialogueKind.Talk,() => {
                            usrState.ChangeHeart(1);
                            StartCoroutine(Level1End(5));
                        },uiManager.dialoguePanel.photoNSad)
                    };
            uiManager.SetDialogues(temp);
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                        new DialogueUnit("电话挂断了呢？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                        new DialogueUnit("是接的太晚了吗......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                        new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                        new DialogueUnit("好累。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                        new DialogueUnit("有点困了，就在这里睡吧。", DialogueKind.Talk,() => {
                            StartCoroutine(Level1End(5));
                        },uiManager.dialoguePanel.photoNSad)
            };
            uiManager.SetDialogues(temp);
        }
    }
    private IEnumerator Level1End(float time) {
        float tempTime = 0;
        float startVolume = mapManager.GetComponent<AudioSource>().volume;
        while (tempTime < time) {
            tempTime += Time.deltaTime;
            if (tempTime > time)
                tempTime = time;
            FindObjectOfType<CameraBlurShape>().brightness = 1 - tempTime / time;
            mapManager.GetComponent<AudioSource>().volume = startVolume * (1 - tempTime / time) * (1 - tempTime / time);
            yield return new WaitForEndOfFrame();
        }
        usrState.isControlEnable = true;
        uiManager.SetLoadingWithSave(GMManager.LEVEL_2, usrState.myHeart);
    }

    //void _2() {
    //    if (uiManager.timeCounter.minute < 10) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("你好，你的快递，请签收。", "快递员", null),
    //                new DialogueUnit("嗯。", "小N", null),
    //                new DialogueUnit("拆开-", "小N", null),
    //                new DialogueUnit("呃，一个鳗鱼玩偶。", "小N", null),
    //                new DialogueUnit("......", "小A", null),
    //                new DialogueUnit("很精致。", "小N", null),
    //                new DialogueUnit("不过是谁送来的呢。", "小N", null),
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("并没有写发件人的姓名。", "小N", null),
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("好像又有点困了。", "小N", null),
    //                new DialogueUnit("就在这里睡吧。", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.ChangeHeart(2);
    //            usrState.isControlEnable = true;
    //            uiManager.SetLoadingWithSave(GMManager.LEVEL_3, usrState.myHeart);
    //        });
    //    }
    //    if (uiManager.timeCounter.minute < 30) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("你好，你的快递，请签收。", "快递员", null),
    //                new DialogueUnit("嗯。", "小N", null),
    //                new DialogueUnit("拆开-", "小N", null),
    //                new DialogueUnit("呃，一个鳗鱼玩偶。", "小N", null),
    //                new DialogueUnit("......", "小A", null),
    //                new DialogueUnit("有点皱皱巴巴的了。", "小N", null),
    //                new DialogueUnit("不过是谁送来的呢。", "小N", null),
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("并没有写发件人的姓名。", "小N", null),
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("好像又有点困了。", "小N", null),
    //                new DialogueUnit("就在这里睡吧。", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.ChangeHeart(1);
    //            usrState.isControlEnable = true;
    //            uiManager.SetLoadingWithSave(GMManager.LEVEL_3, usrState.myHeart);
    //        });
    //    }

    //    else {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("额......", "小N", null),
    //                new DialogueUnit("等太久走掉了吗。", "小N", null),
    //                new DialogueUnit("开门-", "小N", null),
    //                new DialogueUnit("门口这个就是快递吗。", "小N", null),
    //                new DialogueUnit("是个鳗鱼玩偶。破破烂烂的。", "小A", null),
    //                new DialogueUnit("是谁的恶作剧吗。", "小N", null),
    //                new DialogueUnit("好像又有点困了。", "小N", null),
    //                new DialogueUnit("明天再说吧。", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 1)
    //                uiManager.SetLoading(GMManager.BadEnd_2);
    //            else
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_3, usrState.myHeart);
    //        });
    //    }
    //}

    //void _3() {
    //    if (uiManager.timeCounter.minute <5) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("这里是尽头了吗。", "小N", null),
    //                new DialogueUnit(".......", "小N", null),
    //                new DialogueUnit("手机好像在放什么视频...", "小N", null),
    //                new DialogueUnit("查看——", "小N", null),
    //                new DialogueUnit("小N这几个月不知道怎么了，一直把自己关在家里。去看他的时候也感觉他阴沉沉的", "小C", null),
    //                new DialogueUnit("欸，是吗。怪不得没有他的消息", "小A", null),
    //                new DialogueUnit("可能是长时间工作，很少有机会与人接触的原因吧。", "小B", null),
    //                new DialogueUnit("......", "沉默", null),
    //                new DialogueUnit("那我们想想办法帮帮他吧", "小A", null),
    //                new DialogueUnit("画面中小A拿出手机", "小A", null),

    //                new DialogueUnit("叮铃铃~", "手机", null),
    //                new DialogueUnit("喂，是小N吗？你还好吗？听说你前段时间离职了，之后就一直待在家里不出门了，真担心你啊。我和几个小伙伴明天打算过来看看你……", "小A", null),
    //                new DialogueUnit("嗞嗞嗞——", "电话", null),
    //                new DialogueUnit("......", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 4)
    //                uiManager.SetLoading(GMManager.NORMAL_END);
    //            else {
    //                usrState.ChangeHeart(2);
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_4, usrState.myHeart);
    //            }
    //        });
    //    }
    //    if (uiManager.timeCounter.minute < 10) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("这里是尽头了吗。", "小N", null),
    //                new DialogueUnit(".......", "小N", null),
    //                new DialogueUnit("手机好像在放什么视频...", "小N", null),
    //                new DialogueUnit("查看——", "小N", null),
    //                new DialogueUnit("那我们想想办法帮帮他吧", "小A", null),
    //                new DialogueUnit("画面中小A拿出手机", "小A", null),

    //                new DialogueUnit("叮铃铃~", "手机", null),
    //                new DialogueUnit("喂，是小N吗？你还好吗？听说你前段时间离职了，之后就一直待在家里不出门了，真担心你啊。我和几个小伙伴明天打算过来看看你……", "小A", null),
    //                new DialogueUnit("嗞嗞嗞——", "电话", null),
    //                new DialogueUnit("......", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 4)
    //                uiManager.SetLoading(GMManager.NORMAL_END);
    //            else {
    //                usrState.ChangeHeart(1);
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_4, usrState.myHeart);
    //            }
    //        });
    //    }
    //    else {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("叮铃铃~", "手机", null),
    //                new DialogueUnit("喂，是小N吗？你还好吗？听说你前段时间离职了，之后就一直待在家里不出门了，真担心你啊。我和几个小伙伴明天打算过来看看你……", "小A", null),
    //                new DialogueUnit("嗞嗞嗞——", "电话", null),
    //                new DialogueUnit("......", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 4)
    //                uiManager.SetLoading(GMManager.NORMAL_END);
    //            else {
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_4, usrState.myHeart);
    //            }
    //        });
    //    }
    //}

    //void _4() {
    //    if (uiManager.timeCounter.minute < 1) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit(".......", "小N", null),
    //                new DialogueUnit("真的要开门吗。", "小N", null),
    //                new DialogueUnit("现在的生活也不错。", "小N", null),
    //                new DialogueUnit("......", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.ChangeHeart(2);
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 10)
    //                uiManager.SetLoading(GMManager.NORMAL_END);
    //            else
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_5, usrState.myHeart);
    //        });
    //    }
    //    if (uiManager.timeCounter.minute < 2) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit(".......", "小N", null),
    //                new DialogueUnit("真的要开门吗。", "小N", null),
    //                new DialogueUnit("现在的生活也不错。", "小N", null),
    //                new DialogueUnit("......", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.ChangeHeart(1);
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 10)
    //                uiManager.SetLoading(GMManager.NORMAL_END);
    //            else
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_5, usrState.myHeart);
    //        });
    //    }

    //    else {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit(".......", "小N", null),
    //                new DialogueUnit("真的要开门吗。", "小N", null),
    //                new DialogueUnit("现在的生活也不错。", "小N", null),
    //                new DialogueUnit("......", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            if (usrState.myHeart < 10)
    //                uiManager.SetLoading(GMManager.NORMAL_END);
    //            else
    //                uiManager.SetLoadingWithSave(GMManager.LEVEL_5, usrState.myHeart);
    //        });
    //    }
    //}

    //void _5() {
    //    if (uiManager.timeCounter.minute < 10) {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("果然还是想和他们在一起。", "小N", null),
    //                new DialogueUnit("吱——", "开门", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            uiManager.SetLoading(GMManager.HAPPY_END);
    //        });
    //    }

    //    else {
    //        usrState.GetComponent<UsrControl>().Stand();
    //        List<DialogueUnit> temp = new List<DialogueUnit> {
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("等太久走掉了吗。", "小N", null),
    //                new DialogueUnit("......", "小N", null),
    //                new DialogueUnit("有一张纸条。", "小N", null),
    //                new DialogueUnit("我们都很担心你，这些天的东西都是我们送的，希望没有吓到你，也希望你能快点好起来。", "小A", null),
    //                new DialogueUnit("抱歉这么晚打扰你，这次很遗憾没有见到你，希望明年可以见到。", "小N", null)
    //            };
    //        uiManager.SetDialogues(temp, () => {
    //            usrState.isControlEnable = true;
    //            uiManager.SetLoading(GMManager.NORMAL_END);
    //        });
    //    }
    //}

    #endregion
}
