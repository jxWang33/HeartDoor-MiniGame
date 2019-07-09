using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript4 : MonoBehaviour
{
    public UsrState usrState;
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
        timer.SetActive(false);
        List<DialogueUnit> temp = new List<DialogueUnit> {
                new DialogueUnit("............", "小N", null),
                new DialogueUnit("啊呼——", "小N", null),
                new DialogueUnit("............", "小N", null),
                new DialogueUnit("果然是梦吗。", "小N", null),
                new DialogueUnit("............", "小N", null),
                new DialogueUnit("咚咚咚", "敲门声", null),
                new DialogueUnit("啊，有人敲门。", "小N", null),
                new DialogueUnit("等我！", "小N", null)
            };
        uiManager.SetDialogues(temp, () => {
            usrState.gameObject.SetActive(true);
            timer.SetActive(true);
            GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
            tempT.GetComponent<TitleFlash>().Set("十月四日", "勇往直前", "不要停顿");
            SoundPlay(timerClip);
            mapManager.GetComponent<AudioSource>().Play();
        });
    }

    public void ReadFromFile(string fileName) {
        BinaryReader br = new BinaryReader(new FileStream(fileName + ".save", FileMode.Open));
        if (br == null)
            return;

        string levelName = br.ReadString();
        if (levelName != GMManager.LEVEL_4) {
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
