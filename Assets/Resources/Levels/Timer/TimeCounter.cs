using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public int minute;
    public int second;
    float timeSpend = 0.0f;

    public void Awake() {
    }
    private void Update() {
        UpdateTimer();
    }

    public void UpdateTimer() {
        timeSpend += Time.deltaTime;
        int hour = (int)timeSpend / 3600;
        minute = ((int)timeSpend - hour * 3600) / 60;
        second = (int)timeSpend - hour * 3600 - minute * 60;
        GetComponentInChildren<Text>().text = string.Format("{0:D2}:{1:D2}", minute, second);
    }
}