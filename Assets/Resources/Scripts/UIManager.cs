using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public DialoguePanel dialoguePanel;

    public bool SetDialogues(List<DialogueUnit> units, DialogueDelegate OnDialogueEnd = null) {
        if (dialoguePanel.gameObject.activeSelf)
            return false;

        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.OnDialogueEnd = OnDialogueEnd;
        dialoguePanel.SetDialogues(units);
        return true;
    }
}
