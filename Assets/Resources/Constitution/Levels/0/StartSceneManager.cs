using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public Text startText;
    public Text loadText;
    public Text quitText;
    public Image cursor;
    public int currentChoosePoint = 0;
    
    public LoadingPanel loadingPanel;
    public CameraWaterWave cameraWaterWave;
    struct StartChoose {
        public Text text;
        public bool active;
        public StartChoose(Text text, bool active = true) {
            this.text = text;
            this.active = active;
            SetActive(active);
        }
        public void SetActive(bool ac) {
            active = ac;
            Color tempColor = text.color;
            if(!ac)
                tempColor.a = 0.1f;
            if(ac)
                tempColor.a = 1f;
            text.color = tempColor;
        }
    };
    List<StartChoose> startChooses;
    void Awake() {
        GMManager.Init();//全局初始化
        startChooses = new List<StartChoose>();

        startChooses.Add(new StartChoose(startText));
        FileStream fs = new FileStream("game.save", FileMode.OpenOrCreate);
        if (fs.Length == 0) 
            startChooses.Add(new StartChoose(loadText, false));
        else
            startChooses.Add(new StartChoose(loadText));
        startChooses.Add(new StartChoose(quitText));

        cursor.transform.position = startChooses[currentChoosePoint].text.transform.position - new Vector3(150, 0, 0);
        startChooses[currentChoosePoint].text.fontStyle = FontStyle.Bold;

        InvokeRepeating("RandomWaterWave", 3, 3);
    }

    private void Update() {
        if (loadingPanel.gameObject.activeSelf)
            return;
        if (Input.GetKeyDown(GMManager.UP_KEY)) {
            startChooses[currentChoosePoint].text.fontStyle = FontStyle.Normal;
            do {
                currentChoosePoint--;
                if (currentChoosePoint < 0)
                    currentChoosePoint = 0;
            } while (!startChooses[currentChoosePoint].active);
            cursor.transform.position = startChooses[currentChoosePoint].text.transform.position - new Vector3(150, 0, 0);
            startChooses[currentChoosePoint].text.fontStyle = FontStyle.Bold;
        }
        if (Input.GetKeyDown(GMManager.DOWN_KEY)) {
            startChooses[currentChoosePoint].text.fontStyle = FontStyle.Normal;
            do {
                currentChoosePoint++;
                if (currentChoosePoint > startChooses.Count - 1)
                    currentChoosePoint = startChooses.Count - 1;
            } while (!startChooses[currentChoosePoint].active);
            cursor.transform.position = startChooses[currentChoosePoint].text.transform.position - new Vector3(150, 0, 0);
            startChooses[currentChoosePoint].text.fontStyle = FontStyle.Bold;
        }
        if (Input.GetKeyDown(GMManager.FUNC_KEY)) {
            CancelInvoke("RandomWaterWave");
            switch (currentChoosePoint) {
                case 0:
                    PressStart();
                    break;
                case 1:
                    PressLoad();
                    break;
                case 2:
                    PressQuit();
                    break;
            }
        }
    }

    void RandomWaterWave() {
        Vector2 tempVec = new Vector2((float)GMManager.rd.NextDouble() * 1920, (float)GMManager.rd.NextDouble() * 1080);
        cameraWaterWave.Emit(tempVec);
    }

    private void PressStart() {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.SetAndSave(GMManager.LEVEL_1, 2);
    }
    private void PressLoad() {
        BinaryReader brr = new BinaryReader(new FileStream("game.save", FileMode.Open));
        if (brr == null)
            return;
        string levelName = brr.ReadString();
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.Set(levelName);
        brr.Close();
    }
    private void PressQuit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}