using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TriggerGoodFinal : TriggerGood
{
    public int pointer = 1;
    public MapManager mapManager;

    public LevelManager levelManager;

    public GameObject pbBlackShadow;

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

    private IEnumerator LevelEnd(float time, string nextLevel, bool ifSave = true) {
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
        if (ifSave)
            uiManager.SetLoadingWithSave(nextLevel, usrState.myHeart);
        else
            uiManager.SetLoading(nextLevel);
    }

    private IEnumerator AwakeEnd(float time, string nextLevel, bool ifSave = true) {
        float tempTime = 0;
        float startVolume = mapManager.GetComponent<AudioSource>().volume;
        float startRainVolume = GameObject.Find("Rain").GetComponent<AudioSource>().volume;
        while (tempTime < time) {
            tempTime += Time.deltaTime;
            if (tempTime > time)
                tempTime = time;
            FindObjectOfType<CameraBlurShape>().brightness = 1 - tempTime / time;
            mapManager.GetComponent<AudioSource>().volume = startVolume * (1 - tempTime / time) * (1 - tempTime / time);
            GameObject.Find("Rain").GetComponent<AudioSource>().volume = startRainVolume * (1 - tempTime / time) * (1 - tempTime / time);
            yield return new WaitForEndOfFrame();
        }
        usrState.isControlEnable = true;
        if (ifSave)
            uiManager.SetLoadingWithSave(nextLevel, usrState.myHeart);
        else
            uiManager.SetLoading(nextLevel);
    }

    #region LEVELS

    void _1() {
        if (uiManager.timeCounter.minute < 5) {
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
                            levelManager.SoundInvoke("Hang");
                        },uiManager.dialoguePanel.photoBSad),
                    new DialogueUnit("挂断——", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("国庆节了吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("好累。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("有点困了，就在这里睡吧。", DialogueKind.Talk,() => {
                            usrState.ChangeHeart(2);
                            StartCoroutine(LevelEnd(5,GMManager.LEVEL_2));
                        },uiManager.dialoguePanel.photoNSad)
                    };
            uiManager.SetDialogues(temp);
        }
        else if (uiManager.timeCounter.minute < 10) {
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
                            StartCoroutine(LevelEnd(5,GMManager.LEVEL_2));
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
                            StartCoroutine(LevelEnd(5,GMManager.BadEnd_1,false));
                        },uiManager.dialoguePanel.photoNSad)
            };
            uiManager.SetDialogues(temp);
        }
    }

    void _2() {
        if (uiManager.timeCounter.minute < 5) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("还是走到门口了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("感觉好久没有这样跟人面对面交流了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("他一定等的不耐烦了吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("要是他责怪我该怎么办。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("算了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("反正也不是我买的东西，说不定是谁寄错了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你好，你到门口了吗，请开一下门吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("他好像听到我的声音了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("现在不开门的话他会感觉我很奇怪吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("管不了那么多了。", DialogueKind.Talk, ()=>{
                            levelManager.SoundInvoke("OpenDoor");
                        },uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("开门", DialogueKind.Sound),
                    new DialogueUnit("你好，你的快递。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("这是你的名字没错吧？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("嗯......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过我确实不记得我有买过东西，事实上我已经很久没有在网上购买物品了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那可能是你的朋友送给你的，没有人和你说吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("没...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那可能他想给你个惊喜，真羡慕你呢。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit(".....", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("...羡慕...我吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("嗯？你说了什么吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("没。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("那没什么问题我就先走了，还有其他工作要做。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("好的，辛苦你了。", DialogueKind.Talk, ()=>{
                            levelManager.SoundInvoke("CloseDoor");
                        },uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("关门", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("好像也没有想象中的那么可怕。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("我看看送过来的是什么。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("会是一个惊喜吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("拆开", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("是一个鳗鱼玩偶。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("而且看起来很新。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("果然是个惊喜呢。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过是谁送的呢？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("并没有写发件人的姓名。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("肯定是谁寄错了吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不然呢！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("谁会送给你东西！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你不清楚自己的情况吗！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("居然还拆开了！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("一会真正的收件人找过来怎么办！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("又有点困了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("被吵醒后没睡好吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, () => {
                            usrState.ChangeHeart(2);
                            StartCoroutine(LevelEnd(5,GMManager.LEVEL_3));
                        },uiManager.dialoguePanel.photoNSad)
                };
            uiManager.SetDialogues(temp);
        }
        if (uiManager.timeCounter.minute < 20) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("还是走到门口了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("感觉好久没有这样跟人面对面交流了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("他一定等的不耐烦了吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("要是他责怪我该怎么办。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("算了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("反正也不是我买的东西，说不定是谁寄错了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你好，你到门口了吗，请开一下门吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("他好像听到我的声音了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("现在不开门的话他会感觉我很奇怪吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("管不了那么多了。", DialogueKind.Talk, ()=>{
                            levelManager.SoundInvoke("OpenDoor");
                        },uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("开门", DialogueKind.Sound),
                    new DialogueUnit("你好，你的快递。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("这是你的名字没错吧？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("嗯......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过我确实不记得我有买过东西，事实上我已经很久没有在网上购买物品了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那可能是你的朋友送给你的，没有人和你说吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("没...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("那可能他想给你个惊喜，真羡慕你呢。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit(".....", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("...羡慕...我吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("嗯？你说了什么吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("没。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("那没什么问题我就先走了，还有其他工作要做。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("好的，辛苦你了。", DialogueKind.Talk, ()=>{
                            levelManager.SoundInvoke("CloseDoor");
                        },uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("关门", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("好像也没有想象中的那么可怕。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("我看看送过来的是什么。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("会是一个惊喜吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("拆开", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("是一个鳗鱼玩偶。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("就是有点皱皱巴巴的了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过是谁送的呢？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("并没有写发件人的姓名。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("肯定是谁寄错了吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不然呢！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("谁会送给你东西！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你不清楚自己的情况吗！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("居然还拆开了！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("一会真正的收件人找过来怎么办！", DialogueKind.Talk, null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("又有点困了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("被吵醒后没睡好吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, () => {
                            usrState.ChangeHeart(1);
                            StartCoroutine(LevelEnd(5,GMManager.LEVEL_3));
                        },uiManager.dialoguePanel.photoNSad)
                };
            uiManager.SetDialogues(temp);
        }
        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("还是走到门口了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("感觉好久没有这样跟人面对面交流了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("他一定等的不耐烦了吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("要是他责怪我该怎么办。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("算了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("反正也不是我买的东西，说不定是谁寄错了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你好，你到门口了吗，请开一下门吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                    new DialogueUnit("他好像听到我的声音了...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("现在不开门的话他会感觉我很奇怪吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("管不了那么多了。", DialogueKind.Talk, ()=>{
                            levelManager.SoundInvoke("OpenDoor");
                        },uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("开门", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("等太久走掉了吗.....", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("门口这个就是快递吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我拆开看看...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("是个鳗鱼玩偶，不过破破烂烂的了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("是谁的恶作剧吗？", DialogueKind.Talk, ()=>{
                            levelManager.SoundInvoke("CloseDoor");
                        },uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("关门", DialogueKind.Sound),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过是谁送的呢？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("并没有写发件人的姓名。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("果然是恶作剧吗...", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("又有点困了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("被吵醒后没睡好吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, () => {
                            if(usrState.myHeart<=1)
                                StartCoroutine(LevelEnd(5,GMManager.BadEnd_2,false));
                            else
                                StartCoroutine(LevelEnd(5,GMManager.LEVEL_3));
                        },uiManager.dialoguePanel.photoNSad)
                };
            uiManager.SetDialogues(temp);
        }
    }

    void _3() {
        if (uiManager.timeCounter.minute < 5) {
            usrState.GetComponent<UsrControl>().Stand();
            levelManager.SoundInvoke("OpenDoor");
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("开门。",DialogueKind.Sound),
                    new DialogueUnit("你做到了。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("没有那么糟，对吧。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("真的要这样草草的遗忘过去的伤痛吗？", DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("什么惊喜，什么逃避。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("有什么存在的价值！", DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("看来你需要我帮你一把。", DialogueKind.Talk,()=>{                        
                        GameObject tempObject = Instantiate(pbBlackShadow);
                        tempObject.transform.position = new Vector3(22, -1.5f, 0);
                        tempObject.GetComponent<BlackShadow>().Set(new Vector2(1, 0), 10);
                    },uiManager.dialoguePanel.photoX),
                    new DialogueUnit("————",DialogueKind.Sound),
                    new DialogueUnit("不要！", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("为什么！", DialogueKind.Talk, () => {
                        usrState.isControlEnable = true;
                        if (usrState.myHeart < 4)
                            StartCoroutine(AwakeEnd(2,GMManager.NORMAL_END,false));
                        else {
                            usrState.ChangeHeart(2);
                            StartCoroutine(AwakeEnd(2,GMManager.LEVEL_4));
                        }
                    },uiManager.dialoguePanel.photoNPanic)
                };
            uiManager.SetDialogues(temp);
        }
        if (uiManager.timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            levelManager.SoundInvoke("OpenDoor");
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("开门。",DialogueKind.Sound),
                    new DialogueUnit("没有那么糟，对吧。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("什么惊喜，什么逃避。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("有什么存在的价值！", DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
                    new DialogueUnit("看来你需要我帮你一把。", DialogueKind.Talk,()=>{
                        GameObject tempObject = Instantiate(pbBlackShadow);
                        tempObject.transform.position = new Vector3(22, -1.5f, 0);
                        tempObject.GetComponent<BlackShadow>().Set(new Vector2(1, 0), 10);
                    },uiManager.dialoguePanel.photoX),
                    new DialogueUnit("————",DialogueKind.Sound),
                    new DialogueUnit("不要！", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("为什么！", DialogueKind.Talk, () => {
                        usrState.isControlEnable = true;
                        if (usrState.myHeart < 4)
                            StartCoroutine(AwakeEnd(2,GMManager.NORMAL_END,false));

                        else {
                            usrState.ChangeHeart(1);
                            StartCoroutine(AwakeEnd(2,GMManager.LEVEL_4));
                        }
                    },uiManager.dialoguePanel.photoNPanic)
                };
            uiManager.SetDialogues(temp);
        }
        else {
            usrState.GetComponent<UsrControl>().Stand();
            levelManager.SoundInvoke("OpenDoor");
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("开门。",DialogueKind.Sound),
                    new DialogueUnit("看来你需要我帮你一把。", DialogueKind.Talk,()=>{
                        GameObject tempObject = Instantiate(pbBlackShadow);
                        tempObject.transform.position = new Vector3(22, -1.5f, 0);
                        tempObject.GetComponent<BlackShadow>().Set(new Vector2(1, 0), 10);
                    },uiManager.dialoguePanel.photoX),
                    new DialogueUnit("————",DialogueKind.Sound),
                    new DialogueUnit("不要！", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("为什么！", DialogueKind.Talk, () => {
                        usrState.isControlEnable = true;
                        if (usrState.myHeart < 4)
                            StartCoroutine(AwakeEnd(2,GMManager.NORMAL_END,false));
                        else
                            StartCoroutine(AwakeEnd(2,GMManager.LEVEL_4));                        
                    },uiManager.dialoguePanel.photoNPanic)
                };
            uiManager.SetDialogues(temp);
        }
    }

    void _4() {
        if (uiManager.timeCounter.minute < 5) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit(".......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("真的要开门吗。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我怎么可以在这里犹豫。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNAngry),
                    new DialogueUnit("等一下！", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你真的有仔细考虑过吗？", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("我已经不想再考虑了！", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNAngry),
                    new DialogueUnit("我已经受够你了！", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNAngry),
                    new DialogueUnit("哈哈哈哈哈哈哈。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("受够我了？", DialogueKind.Talk,()=>{
                        mapManager.GetComponent<AudioSource>().volume=0;
                    }, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你觉得，", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你能，", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("战胜我吗？", DialogueKind.Talk,()=>{
                        GameObject tempObject = Instantiate(pbBlackShadow);
                        tempObject.transform.position = new Vector3(22, -1.5f, 0);
                        tempObject.GetComponent<BlackShadow>().Set(new Vector2(1, 0), 10);
                    }, uiManager.dialoguePanel.photoBoss),
                    new DialogueUnit("————", DialogueKind.Sound,()=>{
                         usrState.ChangeHeart(2);
                        if (usrState.myHeart < 10)
                            StartCoroutine(LevelEnd(2,GMManager.NORMAL_END,false));
                        else
                            StartCoroutine(LevelEnd(0.5f,GMManager.LEVEL_5));
                    })
                };
            uiManager.SetDialogues(temp);
        }
        if (uiManager.timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit(".......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("真的要开门吗。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你真的有仔细考虑过吗？", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("我已经受够你了！", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNAngry),
                    new DialogueUnit("哈哈哈哈哈哈哈。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你觉得，", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你能，", DialogueKind.Talk,null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("战胜我吗？", DialogueKind.Talk,()=>{
                        GameObject tempObject = Instantiate(pbBlackShadow);
                        tempObject.transform.position = new Vector3(22, -1.5f, 0);
                        tempObject.GetComponent<BlackShadow>().Set(new Vector2(1, 0), 10);
                    }, uiManager.dialoguePanel.photoBoss),
                    new DialogueUnit("————", DialogueKind.Sound,()=>{
                         usrState.ChangeHeart(1);
                        if (usrState.myHeart < 10)
                            StartCoroutine(LevelEnd(2,GMManager.NORMAL_END,false));
                        else
                            StartCoroutine(LevelEnd(0.5f,GMManager.LEVEL_5));
                    })
                };
            uiManager.SetDialogues(temp);
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit(".......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("真的要开门吗。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit(".......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("现在的生活也还不错。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("还是...", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("算了吧。", DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("没人吗...", DialogueKind.Talk,null, uiManager.dialoguePanel.photoCSad),
                    new DialogueUnit("......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoASad),
                    new DialogueUnit("......", DialogueKind.Talk,null, uiManager.dialoguePanel.photoBSad),
                    new DialogueUnit("......", DialogueKind.Talk,()=>{
                            StartCoroutine(LevelEnd(5,GMManager.NORMAL_END,false));
                    }, uiManager.dialoguePanel.photoNSad)
                };
            uiManager.SetDialogues(temp);
        }
    }

    void _5() {
        if (uiManager.timeCounter.minute < 10) {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不知道他们还在不在。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("小N你在门旁边吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("快出来吧，我们都听到你了。", DialogueKind.Talk, ()=>{
                        levelManager.SoundInvoke("OpenDoor");
                    },uiManager.dialoguePanel.photoCHappy),
                    new DialogueUnit("开门", DialogueKind.Sound),
                    new DialogueUnit("你们怎么会来？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("呀，小N好像变帅了呢。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoAHappy),
                    new DialogueUnit("我们听小C说你好像最近不太顺利。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBSad),
                    new DialogueUnit("所以想过来帮你排解一下。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoBHappy),
                    new DialogueUnit("没事多出门走走，别总在屋子里坐着。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCHappy),
                    new DialogueUnit("怎么样，现在还会难过吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoAHappy),
                    new DialogueUnit("我曾经是受了些折磨，不过都已经过去了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("谢谢你们过来邀请我。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("那么，", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNHappy),
                    new DialogueUnit("让我们出发吧。", DialogueKind.Talk, () => {
                        StartCoroutine(LevelEnd(5, GMManager.HAPPY_END, false));
                    },uiManager.dialoguePanel.photoNHappy)
                };
            uiManager.SetDialogues(temp);
        }

        else {
            usrState.GetComponent<UsrControl>().Stand();
            List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不知道他们还在不在。", DialogueKind.Talk, ()=>{
                        levelManager.SoundInvoke("OpenDoor");
                    },uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("开门", DialogueKind.Sound),

                    new DialogueUnit("果然已经走了吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("这留了张纸条。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("抱歉过来打扰你。", DialogueKind.Mono),
                    new DialogueUnit("我们听小C说你最近工作不顺利，人也变得阴沉沉的。", DialogueKind.Mono),
                    new DialogueUnit("我们都很担心你，所以想帮助你走出这段阴影。", DialogueKind.Mono),
                    new DialogueUnit("你那个玩偶也是我们送的，希望没有吓到你。", DialogueKind.Mono),
                    new DialogueUnit("这次很遗憾没有见到你，希望明年可以见到。", DialogueKind.Mono),
                    new DialogueUnit("祝你早日敞开心扉。", DialogueKind.Mono,()=>{
                        StartCoroutine(LevelEnd(5, GMManager.NORMAL_END, false));
                    }),                    
                };
            uiManager.SetDialogues(temp);
        }
    }

    #endregion
}

