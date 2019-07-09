using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript3 : MonoBehaviour
{
    public UsrState usrState;
    public NumbFish numbFish;
    public GameObject timer;
    public UIManager uiManager;
    public GameObject titleFlash;
    AudioSource audioSource;
    public AudioClip timerClip;
    public MapManager mapManager;
    void Start() {
        ReadFromFile("game");
        audioSource = GetComponent<AudioSource>();
        usrState.gameObject.SetActive(false);
        numbFish.gameObject.SetActive(false);
        timer.SetActive(false);
        List<DialogueUnit> temp = new List<DialogueUnit> {
                new DialogueUnit("吱——", "鳗鱼", null),
                new DialogueUnit("............", "小N", null),
                new DialogueUnit("唔......", "小N", null),
                new DialogueUnit("阁楼好像传来了奇怪的声音。", "小N", null),
                new DialogueUnit("欸?", "小N", null),
                new DialogueUnit("那不是昨天的玩偶吗。", "小N", null),
                new DialogueUnit("一定是在做梦吧。", "小N", null),
                new DialogueUnit("............", "小N", null),
                new DialogueUnit("总之先跟上去看看。", "小N", null)
            };
        uiManager.SetDialogues(temp, () => {
            usrState.gameObject.SetActive(true);
            numbFish.gameObject.SetActive(true);
            timer.SetActive(true);
            GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
            tempT.GetComponent<TitleFlash>().Set("十月三日", "莫逆之交", "互利共生");
            SoundPlay(timerClip);
            mapManager.GetComponent<AudioSource>().Play();
        });
    }

    public void ReadFromFile(string fileName) {
        BinaryReader br = new BinaryReader(new FileStream(fileName + ".save", FileMode.Open));
        if (br == null)
            return;

        string levelName = br.ReadString();
        if (levelName != GMManager.LEVEL_3) {
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
