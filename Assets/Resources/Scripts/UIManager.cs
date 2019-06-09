using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public DialoguePanel dialoguePanel;
    public EscPanel escPanel;

    public bool SetDialogues(List<DialogueUnit> units, DialogueDelegate OnDialogueEnd = null) {
        if (dialoguePanel.gameObject.activeSelf)
            return false;

        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.OnDialogueEnd = OnDialogueEnd;
        dialoguePanel.SetDialogues(units);
        return true;
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
