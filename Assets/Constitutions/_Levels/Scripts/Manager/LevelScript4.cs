using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript4 : LevelManager
{
    public AudioClip knockClip;

    IEnumerator UsrAwake(float time) {
        float tempTime = 0;
        while (tempTime < time) {
            tempTime += Time.deltaTime;
            if (tempTime > time)
                tempTime = time;
            FindObjectOfType<CameraBlurShape>().brightness = tempTime / time;
            yield return new WaitForEndOfFrame();
        }
    }

    protected override void OnCommonStart() {
        usrState.gameObject.SetActive(false);
        uiManager.timeCounter.gameObject.SetActive(false);
        uiManager.usrStatePanel.gameObject.SetActive(false);
        StartCoroutine(UsrAwake(1));
        List<DialogueUnit> temp = new List<DialogueUnit> {
            new DialogueUnit("————",DialogueKind.Sound),
            new DialogueUnit("......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNPanic),
            new DialogueUnit("果然是梦吗。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("那为什么我的感觉这么真实。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNPanic),
            new DialogueUnit("啊，我的玩偶。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNPanic),
            new DialogueUnit("怎么被撕开了。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNPanic),
            new DialogueUnit("不能再这样下去了，我已经受够了这样的生活。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNPanic),
            new DialogueUnit("受够了?",DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
            new DialogueUnit("那你受够了嘲笑和讥讽，受够了失败和伤痛吗？",DialogueKind.Talk,null,uiManager.dialoguePanel.photoX),
            new DialogueUnit("可是......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("可是我也不能就这么一直逃避下去。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("那还有什么意义！",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNAngry),
            new DialogueUnit("我已经决定了！",DialogueKind.Talk,()=>{
                SoundInvoke("Knock");
            },uiManager.dialoguePanel.photoNAngry),
            new DialogueUnit("咚咚咚",DialogueKind.Sound),
            new DialogueUnit("小N，你在家里吗？",DialogueKind.Talk,null,uiManager.dialoguePanel.photoBHappy),
            new DialogueUnit("小N，我们来看你了。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoASad),
            new DialogueUnit("出来和我们一起去散散心吧。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoAHappy),
            new DialogueUnit("对啊，总在屋子里人会变傻的。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoCHappy),
            new DialogueUnit("是他们。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNHappy),
            new DialogueUnit("对，我要做出改变，我要去勇敢的面对。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNHappy),
            new DialogueUnit("还有那么多人关心我。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNHappy),
            new DialogueUnit("我怎么可以放弃！",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNHappy),
            new DialogueUnit("等我！！！",DialogueKind.Talk,()=>{
                    usrState.gameObject.SetActive(true);
                    uiManager.timeCounter.gameObject.SetActive(true);
                    uiManager.usrStatePanel.gameObject.SetActive(true);
                    GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
                    tempT.GetComponent<TitleFlash>().Set("十月四日", "勇往直前", "不顾一切");
                    mapManager.GetComponent<AudioSource>().Play();
            },uiManager.dialoguePanel.photoNHappy)
        };
        uiManager.SetDialogues(temp);
    }

    protected override void OnDebugStart() {
        usrState.gameObject.SetActive(true);
        uiManager.timeCounter.gameObject.SetActive(true);
        uiManager.usrStatePanel.gameObject.SetActive(true);
        FindObjectOfType<CameraBlurShape>().brightness = 1;
    }

    public void KnockSound() {
        SoundPlay(knockClip);
    }

    public override void SoundInvoke(string clipName) {
        Invoke(clipName + "Sound", 0);
    }
}
