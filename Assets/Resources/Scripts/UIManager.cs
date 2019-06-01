using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public DialoguePanel dialoguePanel;

    void Awake()
    {
    //    List<DialogueUnit> a = new List<DialogueUnit>();
    //    a.Add(new DialogueUnit("dsadsadasdsadasdasdasdasdsadasdsadasa", "asd", null));
    //    a.Add(new DialogueUnit("qweqeqwewqewqeqwewqeqweqweqweqweqwe", "aaaaaavv", null));
    //    a.Add(new DialogueUnit("98765fbhjuhygtrfedevbtnyj7iu6y5t4e", "asd", null));
    //    a.Add(new DialogueUnit("1234567898765432345678765432sfgayuu", "asd", null));
    //    SetDialogues(a, () => {
    //        Debug.Log("DialogueEnd");
    //    });
    }

    void Update()
    {
        
    }

    public void SetDialogues(List<DialogueUnit> units, DialogueDelegate OnDialogueEnd = null) {
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.OnDialogueEnd = OnDialogueEnd;
        dialoguePanel.SetDialogues(units);
    }
}
