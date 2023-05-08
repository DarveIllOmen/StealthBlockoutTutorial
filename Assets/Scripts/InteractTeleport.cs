using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTeleport : Interactable
{
    [SerializeField] Transform _position;

    public override void Interact(GameObject actuator)
    {
        foreach (GameObject player in _interactedBy)
        {
            if (player == actuator)
            {
                actuator.transform.position = _position.position;
            }
        }

    }
}
