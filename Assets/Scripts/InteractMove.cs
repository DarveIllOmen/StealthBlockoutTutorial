using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMove : Interactable
{
    [SerializeField] Transform _position;
    [SerializeField] bool _shouldStun;

    public override void Interact(GameObject actuator)
    {
        foreach(GameObject player in _interactedBy)
        {
            if(player == actuator)
            {
                transform.position = _position.position;
                if (_shouldStun)
                {
                    actuator.GetComponent<Movement>()._stunned = true;
                }
                this.enabled = false;
            }
        }
        
    }
}
