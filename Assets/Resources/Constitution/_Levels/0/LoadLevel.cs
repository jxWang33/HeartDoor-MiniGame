using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public Button startButton;

    public LoadingPanel loadingPanel;

    // Use this for initialization
    void Awake() {
        startButton.onClick.AddListener(() => {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.Set(GMManager.LEVEL_1);
        });
    }
}