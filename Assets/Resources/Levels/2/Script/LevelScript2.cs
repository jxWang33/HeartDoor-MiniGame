using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript2 : MonoBehaviour
{
    public UsrState usrState;
    public GameObject timer;
    public UIManager uiManager;
    public GameObject titleFlash;
    AudioSource audioSource;
    public AudioClip timerClip;
    public MapManager mapManager;
    public AudioClip callingClip;
    void Start() {
        ReadFromFile("game");

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = callingClip;
        audioSource.Play();

        usrState.gameObject.SetActive(false);
        //timer.SetActive(false);
        //List<DialogueUnit> temp = new List<DialogueUnit> {
        //        new DialogueUnit("叮铃铃~ 叮铃铃~", "电话", null),
        //        new DialogueUnit("............", "小N", null),
        //        new DialogueUnit("叮铃铃~ 叮铃铃~", "电话", null),
        //        new DialogueUnit("喂，您好，您的快递，请签收", "小N", null),
        //        new DialogueUnit("额?", "小N", null),
        //        new DialogueUnit("挂断——", "电话", null),
        //        new DialogueUnit("我不记得有在网上买东西...", "小N", null),
        //        new DialogueUnit("............", "小N", null),
        //        new DialogueUnit("又梦游到地下室来了吗。", "小N", null),
        //        new DialogueUnit("最近总是这样呢。", "小N", null),
        //        new DialogueUnit("总之，先去门口看看。", "小N", null)
        //    };
        //uiManager.SetDialogues(temp, () => {
        //    usrState.gameObject.SetActive(true);
        //    timer.SetActive(true);
        //    GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
        //    tempT.GetComponent<TitleFlash>().Set("十月二日", "不速之客", "来来回回");
        //    SoundPlay(timerClip);
        //    mapManager.GetComponent<AudioSource>().Play();
        //});
    }
    
    void Update()
    {
        
    }

    public void ReadFromFile(string fileName) {
        BinaryReader br = new BinaryReader(new FileStream(fileName + ".save", FileMode.Open));
        if (br == null)
            return;

        string levelName = br.ReadString();
        if (levelName != GMManager.LEVEL_2) {
            br.Close();
            return;
        }

        usrState.SetHeart(br.ReadInt32());
        br.Close();
    }

    private void SoundPlay(AudioClip ac, bool forceChange = false, float vol = 1.0f) {
        if (!audioSource.isPlaying || forceChange) {
            audioSource.volume = vol;
            audioSource.clip = ac;
            audioSource.Play();
        }
    }
}
