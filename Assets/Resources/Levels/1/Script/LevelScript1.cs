using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript1 : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip keyboardClip;
    public AudioClip callingClip;
    public AudioClip hangClip;
    public AudioClip timerClip;
    public UsrState usrState;
    public UIManager uiManager;
    public GameObject timer;
    public GameObject titleFlash;
    public MapManager mapManager;

    public bool debugStart = true;

    void Start() {
        if (debugStart) {
            GMManager.Init();
            FindObjectOfType<CameraBlurShape>().brightness = 1;
            audioSource = GetComponent<AudioSource>();
            return;
        }
        timer.SetActive(false);
        usrState.gameObject.SetActive(false);
        List<DialogueUnit> temp = new List<DialogueUnit> {
                new DialogueUnit("放松", DialogueKind.Mono, null),
                new DialogueUnit("别忘记呼吸", DialogueKind.Mono,()=>{
                    audioSource = GetComponent<AudioSource>();
                    audioSource.clip = keyboardClip;
                    audioSource.Play();
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
                    uiManager.usrStatePanel.gameObject.SetActive(true);
                    usrState.gameObject.SetActive(true);
                    timer.SetActive(true);
                    GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
                    tempT.GetComponent<TitleFlash>().Set("十月一日", "神秘电话", "小试牛刀");
                    SoundPlay(timerClip);
                    mapManager.GetComponent<AudioSource>().Play();
                },uiManager.dialoguePanel.photoNAngry)
            };
        uiManager.SetDialogues(temp);
    }

    public void PlayCallingSound() {
        SoundPlay(callingClip);
    }
    public void PlayHangSound() {
        SoundPlay(hangClip);
    }

    private void SoundPlay(AudioClip ac, bool forceChange = false, float vol = 1.0f) {
        if (!audioSource.isPlaying || forceChange) { 
            audioSource.volume = vol;
            audioSource.clip = ac;
            audioSource.Play();
        }
    }
}
