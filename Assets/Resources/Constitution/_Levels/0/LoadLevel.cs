using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button quitButton;

    public LoadingPanel loadingPanel;

    // Use this for initialization
    void Awake() {
        FileStream fs = new FileStream("game.save", FileMode.OpenOrCreate);
        if (fs.Length == 0) {
            loadButton.interactable = false;
        }

        startButton.onClick.AddListener(() => {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.SetAndSave(GMManager.LEVEL_1, 2);
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
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.Set(levelName);
            brr.Close();
        });
    }
}