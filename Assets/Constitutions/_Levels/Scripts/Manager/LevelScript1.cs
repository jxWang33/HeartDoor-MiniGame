using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript1 : LevelManager
{
    public AudioClip keyboardClip;
    public AudioClip callingClip;
    public AudioClip hangClip;
    public AudioClip breakClip;

    protected override void OnDebugStart() {
        usrState.gameObject.SetActive(true);
        uiManager.timeCounter.gameObject.SetActive(true);
        uiManager.usrStatePanel.gameObject.SetActive(true);
        FindObjectOfType<CameraBlurShape>().brightness = 1;
    }
    protected override void OnCommonStart() {
        uiManager.timeCounter.gameObject.SetActive(false);
        usrState.gameObject.SetActive(false);
        uiManager.usrStatePanel.gameObject.SetActive(false);
        List<DialogueUnit> temp = new List<DialogueUnit> {
                new DialogueUnit("放松", DialogueKind.Mono, null),
                new DialogueUnit("别忘记呼吸", DialogueKind.Mono,()=>{
                    SoundPlay(keyboardClip);
                    StartCoroutine(Delayed(keyboardClip.length));
                })
            };
        uiManager.SetDialogues(temp);
    }

    private IEnumerator Delayed(float time) {
        float tempTime = 0;
        while (tempTime < time) {
            tempTime += Time.deltaTime;
            if (tempTime > time)
                tempTime = time;
            FindObjectOfType<CameraBlurShape>().brightness = tempTime / time;
            yield return new WaitForEndOfFrame();
        }

        audioSource.Stop();
        SoundPlay(callingClip);
        List<DialogueUnit> temp = new List<DialogueUnit> {
                new DialogueUnit("叮铃铃~ 叮铃铃~", DialogueKind.Sound),
                new DialogueUnit("......",DialogueKind.Talk,()=>{
                    SoundPlay(callingClip);
                },uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("叮铃铃~ 叮铃铃~",DialogueKind.Sound),

                new DialogueUnit("......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("好烦。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("为什么这么晚会有电话？", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("会是谁？", DialogueKind.Talk,()=>{
                    SoundPlay(callingClip);
                },uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("叮铃铃~ 叮铃铃~", DialogueKind.Sound),

                new DialogueUnit("好吵。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("要赶快去接才行。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("要是没接到的话，就白白浪费了路上的时间。", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("......", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("浪费时间？", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("我的时间能用来做什么？", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("发呆吗？", DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("睡觉吗？", DialogueKind.Talk,()=>{
                    SoundPlay(callingClip);
                },uiManager.dialoguePanel.photoNSad),
                new DialogueUnit("叮铃铃~ 叮铃铃~", DialogueKind.Sound),

                new DialogueUnit("好了别吵了，我知道了！", DialogueKind.Talk,() => {
                    usrState.gameObject.SetActive(true);
                    uiManager.timeCounter.gameObject.SetActive(true);
                    uiManager.usrStatePanel.gameObject.SetActive(true);
                    GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
                    tempT.GetComponent<TitleFlash>().Set("十月一日", "神秘电话", "小试牛刀");
                    mapManager.GetComponent<AudioSource>().Play();
                },uiManager.dialoguePanel.photoNAngry)
            };
        uiManager.SetDialogues(temp);
    }

    public void CallingSound() {
        SoundPlay(callingClip);
    }
    public void HangSound() {
        SoundPlay(hangClip);
    }
    public void BreakSound() {
        SoundPlay(breakClip);
    }

    public override void SoundInvoke(string clipName) {
        Invoke(clipName + "Sound", 0);
    }
}
