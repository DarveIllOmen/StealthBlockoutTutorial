using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDelete : Interactable
{
    [SerializeField] GameObject[] _destroyObjects;

    public override void Interact(GameObject actuator)
    {
        foreach(GameObject player in _interactedBy)
        {
            if(player == actuator)
            {
                foreach(GameObject destroy in _destroyObjects)
                {
                    destroy.SetActive(false);
                }
            }
        }
        this.enabled = false;
    }
}
