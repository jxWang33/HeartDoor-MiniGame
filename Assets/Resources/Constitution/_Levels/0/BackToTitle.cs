using UnityEngine;
using UnityEngine.UI;

public class BackToTitle : MonoBehaviour
{
    public Button backButton;
    public LoadingPanel loadingPanel;

    // Use this for initialization
    void Awake() {
        backButton.onClick.AddListener(() => {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.ToTitle();
        });
    }
}
