using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public DialoguePanel dialoguePanel;
    public EscPanel escPanel;
    public LoadingPanel loadingPanel;
    public UsrStatePanel usrStatePanel;
    public TimeCounter timeCounter;

    public bool SetDialogues(List<DialogueUnit> units) {
        if (dialoguePanel.gameObject.activeSelf)
            return false;

        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.SetDialogues(units);
        return true;
    }

    public void SetLoading(string levelName) {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.Set(levelName);
    }

    public void SetLoadingWithSave(string levelName,int hn) {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.SetAndSave(levelName, hn);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!escPanel.gameObject.activeSelf) {
                escPanel.gameObject.SetActive(true);
                escPanel.GetComponent<AudioSource>().Play();
                GMManager.GamePause();
            }
            else {
                escPanel.gameObject.SetActive(false);
                GMManager.GameResume();
            }
        }
    }

}
