using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelScript5 : MonoBehaviour
{
    public UsrState usrState;
    public GameObject timer;
    public UIManager uiManager;
    public GameObject titleFlash;
    AudioSource audioSource;
    public AudioClip timerClip;
    public MapManager mapManager;
    void Start() {
        GMManager.Init();
        ReadFromFile("game");

        audioSource = GetComponent<AudioSource>();
        usrState.gameObject.SetActive(true);
        timer.SetActive(true);
        GameObject tempT = Instantiate(titleFlash, GameObject.Find("Canvas").transform);
        tempT.GetComponent<TitleFlash>().Set("十月五日", "心之门", "最后一步");
        SoundPlay(timerClip);
    }

    public void ReadFromFile(string fileName) {
        BinaryReader br = new BinaryReader(new FileStream(fileName + ".save", FileMode.Open));
        if (br == null)
            return;

        string levelName = br.ReadString();
        if (levelName != GMManager.LEVEL_5) {
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
