using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript3 : LevelManager
{
    public NumbFish numbFish;
    public AudioClip floorClip;
    public AudioClip lightingClip;
    public AudioClip openDoorClip;

    private IEnumerator Brightness(float time) {
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
        numbFish.gameObject.SetActive(false);
        uiManager.timeCounter.gameObject.SetActive(false);
        uiManager.usrStatePanel.gameObject.SetActive(false);
        StartCoroutine(Brightness(3));
        FloorSound();
        List<DialogueUnit> temp = new List<DialogueUnit> {
            new DialogueUnit("吱——",DialogueKind.Sound),
            new DialogueUnit("......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("唔......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("阁楼好像传来了奇怪的声音。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("诶？",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("这不是昨天收到的玩偶吗？",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("他怎么在动？",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("好像拥有生命一样。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("感觉他像是在呼唤我。",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("......",DialogueKind.Talk,null,uiManager.dialoguePanel.photoNSad),
            new DialogueUnit("跟上去看看吧。",DialogueKind.Talk,() => {
                usrState.gameObject.SetActive(true);
                numbFish.gameObject.SetActive(true);
                uiManager.usrStatePanel.gameObject.SetActive(true);
                uiManager.timeCounter.gameObject.SetActive(true);
                GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
                tempT.GetComponent<TitleFlash>().Set("十月三日", "莫逆之交", "互利共生");
                mapManager.GetComponent<AudioSource>().Play();
            },uiManager.dialoguePanel.photoNSad),
        };
        uiManager.SetDialogues(temp);
    }

    protected override void OnDebugStart() {
        usrState.gameObject.SetActive(true);
        uiManager.timeCounter.gameObject.SetActive(true);
        uiManager.usrStatePanel.gameObject.SetActive(true);
        FindObjectOfType<CameraBlurShape>().brightness = 1;
    }

    public void FloorSound() {
        SoundPlay(floorClip, false, 0.5f);
    }
    public void LightingSound() {
        SoundPlay(lightingClip);
    }
    public void OpenDoorSound() {
        SoundPlay(openDoorClip);
    }

    public override void SoundInvoke(string clipName) {
        Invoke(clipName + "Sound", 0);
    }
}
