using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript5 : LevelManager
{
    public AudioClip openDoorClip;

    public void OpenDoorSound() {
        SoundPlay(openDoorClip);
    }

    protected override void OnCommonStart() {
        usrState.gameObject.SetActive(true);
        uiManager.timeCounter.gameObject.SetActive(true);
        uiManager.usrStatePanel.gameObject.SetActive(true);
        GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
        tempT.GetComponent<TitleFlash>().Set("十月五日", "心之门", "我战与我");
    }

    protected override void OnDebugStart() {
        usrState.gameObject.SetActive(true);
        uiManager.timeCounter.gameObject.SetActive(true);
        uiManager.usrStatePanel.gameObject.SetActive(true);
        GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
        tempT.GetComponent<TitleFlash>().Set("十月五日", "心之门", "我战与我");
    }

    public override void SoundInvoke(string clipName) {
        Invoke(clipName + "Sound", 0);
    }
}
