using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Text loadingText;
    public Text progressText;

    private float dotNum = 0;
    private float progress = 0;

    public void SetAndSave(string levelName, int heartNum) {
        SaveToFile("game", levelName, heartNum);
        StartCoroutine(StartLoading(levelName));
    }
    public void Restart() {
        StartCoroutine(StartLoading(SceneManager.GetActiveScene().name));
    }
    public void ToTitle() {
        StartCoroutine(StartLoading("Start"));
    }

    private void Update() {
        loadingText.text = "Loading";
        for (int i = 0; i < (int)dotNum; i++)
            loadingText.text += ".";
        dotNum += Time.deltaTime * 5;
        if (dotNum > 7) {
            dotNum = 0;
        }
    }

    IEnumerator StartLoading(string str) {
        float currentProgress = 0;
        AsyncOperation loading = SceneManager.LoadSceneAsync(str);
        loading.allowSceneActivation = false;
        while (loading.progress < 0.9f) {
            progress = (int)loading.progress;
            while (currentProgress < progress) {
                ++currentProgress;
                progressText.text = currentProgress.ToString() + " %";
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        progress = 100;
        while (currentProgress < progress) {
            ++currentProgress;
            progressText.text = currentProgress.ToString() + " %";
            yield return new WaitForEndOfFrame();
        }
        loading.allowSceneActivation = true;
    }

    private void SaveToFile(string fileName, string levelName,int heartNum) {
        BinaryWriter bw = new BinaryWriter(new FileStream(fileName + ".save", FileMode.Create));
        bw.Write(levelName);
        bw.Write(heartNum);
        bw.Close();
    }
}
