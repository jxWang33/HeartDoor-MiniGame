using UnityEngine;
using UnityEngine.UI;

public class EscPanel : MonoBehaviour
{
    public Slider slider;
    public Button backButton;
    public Button restartButton;

    public LoadingPanel loadingPanel;
    void Start()
    {
        slider.value = AudioListener.volume;
        backButton.onClick.AddListener(() => {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.ToTitle();
        });
        restartButton.onClick.AddListener(() => {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.Restart();
        });
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = slider.value;
    }
    
}
