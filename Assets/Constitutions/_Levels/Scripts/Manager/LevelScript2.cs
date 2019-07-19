using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript2 : LevelManager
{
    public AudioClip callingClip;
    public AudioClip hangClip;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;

    public void OpenDoorSound() {
        SoundPlay(openDoorClip);
    }
    public void CloseDoorSound() {
        SoundPlay(closeDoorClip);
    }

    protected override void OnCommonStart() {
        usrState.gameObject.SetActive(false);
        uiManager.timeCounter.gameObject.SetActive(false);
        uiManager.usrStatePanel.gameObject.SetActive(false);
        SoundPlay(callingClip);
        List<DialogueUnit> temp = new List<DialogueUnit> {
                  new DialogueUnit("叮铃铃~ 叮铃铃~", DialogueKind.Sound),
                  new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("怎么这么早也有人打电话。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("吵死了。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("接起", DialogueKind.Sound),
                  new DialogueUnit("喂？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("你好，有你的包裹，请出来取一下。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                  new DialogueUnit("诶？快递吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("我不记得我有在网上买东西。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("嗯，这边的地址确实是这里没错。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoCo),
                  new DialogueUnit("总之先来开一下门吧。", DialogueKind.Talk, ()=>{
                            SoundPlay(hangClip);
                        },uiManager.dialoguePanel.photoCo),
                  new DialogueUnit("挂断", DialogueKind.Sound),
                  new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("我现在这是在哪里？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("这不是地下室吗。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("我记得昨天晚上睡在了客厅沙发上。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("又梦游到这里来了吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("看来症状一点也没有减轻。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                  new DialogueUnit("他会伴随我一生吗？", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNPanic),
                  new DialogueUnit("......", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("他说是在门口等我吧。", DialogueKind.Talk, null,uiManager.dialoguePanel.photoNSad),
                  new DialogueUnit("还是抓紧去开门吧，让人等太久可不好。", DialogueKind.Talk, ()=>{
                        usrState.gameObject.SetActive(true);
                        uiManager.timeCounter.gameObject.SetActive(true);
                        uiManager.usrStatePanel.gameObject.SetActive(true);
                        GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
                        tempT.GetComponent<TitleFlash>().Set("十月二日", "不速之客", "来来回回");
                        mapManager.GetComponent<AudioSource>().Play();
                  },uiManager.dialoguePanel.photoNSad)
            };
        uiManager.SetDialogues(temp);
    }
    protected override void OnDebugStart() {
        usrState.gameObject.SetActive(true);
        uiManager.timeCounter.gameObject.SetActive(true);
        uiManager.usrStatePanel.gameObject.SetActive(true);
    }

    public override void SoundInvoke(string clipName) {
        Invoke(clipName + "Sound", 0);
    }
}
