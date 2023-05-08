using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWin : Interactable
{
    public override void Interact(GameObject actuator)
    {
        GameManager.Instance._win = true;
    }
}
