using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button quitButton;

    public UIManager uiManager;
    
    void Awake() {
        GMManager.Init();//全局初始化

        FileStream fs = new FileStream("game.save", FileMode.OpenOrCreate);
        if (fs.Length == 0) {
            loadButton.interactable = false;
        }

        startButton.onClick.AddListener(() => {
            uiManager.SetLoadingWithSave(GMManager.LEVEL_1, 2);
        });

        quitButton.onClick.AddListener(() => {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        });
        loadButton.onClick.AddListener(() => {
            BinaryReader brr = new BinaryReader(new FileStream("game.save", FileMode.Open));
            if (brr == null)
                return;
            string levelName = brr.ReadString();
            uiManager.SetLoading(levelName);
            brr.Close();
        });
    }
}