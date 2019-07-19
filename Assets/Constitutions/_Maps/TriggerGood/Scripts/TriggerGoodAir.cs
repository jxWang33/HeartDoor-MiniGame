using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGoodAir : TriggerGood
{
    public string nameTag;

    public LevelManager levelManager;

    protected override void Start() {
        base.Start();
    }

    protected override void TriggerCallBack() {
        Invoke(nameTag, 0);
    }

    public void Level1Chaos() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("这里东西怎么这么乱。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("都是我弄的吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("真不敢相信我住在这种地方。",DialogueKind.Talk,()=>{
                            levelManager.SoundInvoke("Calling");
                        }, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("叮铃铃，叮铃铃",DialogueKind.Sound),
                    new DialogueUnit("我怎么还在这里乱想。", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("难道想让之前的路都白走吗。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoNSad)
                };
        uiManager.SetDialogues(temp);
    }
    public void Level1Attack() {
        usrState.GetComponent<UsrControl>().Stand();
        levelManager.SoundInvoke("Break");
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("砰——",DialogueKind.Sound),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我好像把什么东西弄碎了。",DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("是 门 造成的吗？",DialogueKind.Talk,()=>{
                            levelManager.SoundInvoke("Calling");
                        }, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("叮铃铃，叮铃铃",DialogueKind.Sound),
                    new DialogueUnit("打电话的人真有毅力呢。", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("前面那个扫地机器人在做什么？", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("它坏掉了吗。", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("去拿电话的路好像被他挡住了。", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......", DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("要是也能用 门 把他弄碎就好了。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoNSad)
                };
        uiManager.SetDialogues(temp);
    }

    public void Level2WanderA() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("地下室也变得好乱。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("梦游来的路上弄乱的吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我为什么要去拿一个不属于我的物品？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我还是好困，要不然从前面回去好了...",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("等一会没人开门的话他就会自己离开吧。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoNSad)
                };
        uiManager.SetDialogues(temp);
    }
    public void Level2WanderB() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("不是说要回去吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过都走到这里了，还是去门口看一看吧？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("或者....",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("还是放弃算了，本来就不属于我。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("不过....",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我怎么连这种事都拿不定主意。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("不过看来好像不只是我的 门 可以触发开关，扫地机器人也可以。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("想要打败他的话要看准时机呢。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("如果要过去开门的话......", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoNSad)
                };
        uiManager.SetDialogues(temp);
    }

    public void Level3DreamA() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("快来，小N。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("我需要你的帮助，帮我打开这堵墙。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你居然还会说话。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你不是一个玩偶吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("你是谁，要做什么？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我是一直玩偶，不过我被寄托着一些其他东西，我现在可能是一个，惊喜。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("惊喜吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("快，小N，打开这堵墙。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("我带你去面对外面的世界。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoToy)
                };
        uiManager.SetDialogues(temp);
    }
    public void Level3DreamB() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("你到底在做什么？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你忘记外面的世界给你带来的痛苦了吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你承受的折磨还不够吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你真的想要面对这一切吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我不知道。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoNPanic)
                };
        uiManager.SetDialogues(temp);
    }
    public void Level3DreamC() {
        usrState.GetComponent<UsrControl>().Stand();
        StartCoroutine(Lighting(1));
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("啊！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("就是这样，你想起了曾经的伤痛吧。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("社会所带来的伤痛，可远远没有这么简单。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("放弃吧！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("你还在犹豫什么！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("可是...",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("可是...",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNPanic),
                    new DialogueUnit("快点，小N。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("就快要到门口了。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoToy)
                };
        uiManager.SetDialogues(temp);
    }
    public void Level3DreamD() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("一会就能迎接崭新的生活了。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("小N，是时候走出阴影了。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("你不必如此紧张，一切都没有你想象的那么糟。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("真的吗？",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("可是，我曾那么的痛苦。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("谁都会有失败的时候，小N，不要太过在意，逃避永远不是解决的方法。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("来，让我过去帮你打开最后一堵墙，我们一起来面对。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoToy)
                };
        uiManager.SetDialogues(temp);
    }
    public void Level3DreamVs() {
        usrState.GetComponent<UsrControl>().Stand();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("小N，打开前面那扇门，勇敢的面对曾经的一切。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("别忘了你的痛苦！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("看看你都被折磨成什么样子了！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoX),
                    new DialogueUnit("还有那么多人都在支持你，你不能选择逃避！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("想想我，我是一个惊喜，世界也总是充满惊喜！",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("快，打开那扇门。",DialogueKind.Talk, null, uiManager.dialoguePanel.photoToy),
                    new DialogueUnit("不，你不能。", DialogueKind.Talk,() => {
                        usrState.isControlEnable = true;
                    },uiManager.dialoguePanel.photoX)
                };
        uiManager.SetDialogues(temp);
    }

    IEnumerator Lighting(float time) {
        levelManager.SoundInvoke("Lighting");
        float tempTime = 0;
        while (tempTime < time) {
            tempTime += Time.deltaTime;
            FindObjectOfType<CameraBlurShape>().brightness = (float)GMManager.rd.NextDouble() / 2 + 1.5f;
            yield return new WaitForEndOfFrame();
        }
        FindObjectOfType<CameraBlurShape>().brightness = 1;
    }
}
