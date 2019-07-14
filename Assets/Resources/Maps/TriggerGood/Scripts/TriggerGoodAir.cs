using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGoodAir : TriggerGood
{
    public string nameTag;

    public LevelScript1 level1Manager;
    public AudioClip audioBreak;

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
                            level1Manager.PlayCallingSound();
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
        GetComponent<AudioSource>().clip = audioBreak;
        GetComponent<AudioSource>().Play();
        List<DialogueUnit> temp = new List<DialogueUnit> {
                    new DialogueUnit("砰——",DialogueKind.Sound),
                    new DialogueUnit("......",DialogueKind.Talk, null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("我好像把什么东西弄碎了。",DialogueKind.Talk,null, uiManager.dialoguePanel.photoNSad),
                    new DialogueUnit("是 门 造成的吗？",DialogueKind.Talk,()=>{
                            level1Manager.PlayCallingSound();
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
}
