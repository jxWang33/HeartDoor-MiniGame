using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseForPrompt : Doorable
{
    public override void OnDoorHurt(Vector2 dir) {
        GetComponent<Animator>().SetBool("isBreak", true);
    }
}
