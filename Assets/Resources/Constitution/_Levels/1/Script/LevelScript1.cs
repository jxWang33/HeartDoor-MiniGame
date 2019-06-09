using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript1 : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip keyboardClip;
    public AudioClip callingClip;
    public AudioClip timerClip;
    public UsrState usrState;
    public UIManager uiManager;
    public GameObject timer;
    public GameObject titleFlash;
    public MapManager mapManager;

    int scriptTag = 0;//剧情推进标签

    void Start()
    {
        GMManager.Init();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = keyboardClip;
        audioSource.Play();
        StartCoroutine(Delayed(keyboardClip.length));
        usrState.gameObject.SetActive(false);
        timer.SetActive(false);
    }
    
    void Update()
    {

    }

    private IEnumerator Delayed(float time) {
        yield return new WaitForSeconds(time);
        if (scriptTag == 0) {
            scriptTag++;
            audioSource.Stop();
            SoundPlay(callingClip);
            List<DialogueUnit> temp = new List<DialogueUnit> {
                new DialogueUnit("叮铃铃~ 叮铃铃~", "电话", null),
                new DialogueUnit("嗯？电话？", "小N", null),
                new DialogueUnit("叮铃铃~ 叮铃铃~", "电话", null),
                new DialogueUnit("好烦。。", "小N", null),
                new DialogueUnit("我记得电话是在客厅", "小N", null),
                new DialogueUnit("要赶快去接才行", "小N", null),
            };
            uiManager.SetDialogues(temp, () => {
                usrState.gameObject.SetActive(true);
                timer.SetActive(true);
                GameObject tempT = Instantiate(titleFlash,GameObject.Find("Canvas").transform);
                tempT.GetComponent<TitleFlash>().Set("十月一日","神秘电话","小试牛刀");
                SoundPlay(timerClip);
                mapManager.GetComponent<AudioSource>().Play();
            });
        }

    }
    private void SoundPlay(AudioClip ac, bool forceChange = false, float vol = 1.0f) {
        if (!audioSource.isPlaying || forceChange) { 
            audioSource.volume = vol;
            audioSource.clip = ac;
            audioSource.Play();
        }
    }
}
