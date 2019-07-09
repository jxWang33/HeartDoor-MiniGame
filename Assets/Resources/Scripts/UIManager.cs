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

    public bool SetDialogues(List<DialogueUnit> units, DialogueDelegate OnDialogueEnd = null) {
        if (dialoguePanel.gameObject.activeSelf)
            return false;

        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.OnDialogueEnd = OnDialogueEnd;
        dialoguePanel.SetDialogues(units);
        return true;
    }

    public void SetLoading(string levelName) {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.Set(GMManager.BadEnd_2);
    }

    public void SetLoadingWithSave(string levelName,int hn) {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.SetAndSave(GMManager.BadEnd_2, hn);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!escPanel.gameObject.activeSelf) {
                escPanel.gameObject.SetActive(true);
            }
            else {
                escPanel.gameObject.SetActive(false);
            }
        }
    }

}
