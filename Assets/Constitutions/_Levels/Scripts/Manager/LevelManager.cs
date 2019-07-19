using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public abstract class LevelManager : MonoBehaviour
{
    protected AudioSource audioSource;
    protected string levelName;
    public UsrState usrState;
    public UIManager uiManager;
    public MapManager mapManager;
    public GameObject titleFlash;
    public bool debugStart = false;
    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!GMManager.GAME_INITED)
            GMManager.Init();
        ReadHeartFromFile("game");
        if (debugStart)
            OnDebugStart();
        else
            OnCommonStart();
    }

    protected void SoundPlay(AudioClip ac, bool forceChange = false, float vol = 1.0f) {
        if (!audioSource.isPlaying || forceChange) {
            audioSource.volume = vol;
            audioSource.clip = ac;
            audioSource.Play();
        }
    }
    
    public void ReadHeartFromFile(string fileName) {//读取生命
        BinaryReader br = new BinaryReader(new FileStream(fileName + ".save", FileMode.Open));
        if (br == null) {
            usrState.SetHeart(GMManager.START_HEART);
            return;
        }
        levelName = br.ReadString();
        if (levelName == SceneManager.GetActiveScene().name)
            usrState.SetHeart(br.ReadInt32());
        else 
            usrState.SetHeart(GMManager.START_HEART);
        br.Close();
    }

    protected abstract void OnCommonStart();
    protected abstract void OnDebugStart();

    public abstract void SoundInvoke(string clipName);
}
