using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMove : Interactable
{
    [SerializeField] Transform _position;
    [SerializeField] bool _shouldStun;

    public bool doorBlock;
    public bool chickenBlock;

    public override void Interact(GameObject actuator)
    {
        foreach(GameObject player in _interactedBy)
        {
            if(player == actuator)
            {
                if (chickenBlock)
                {
                    lights.SetLight(lights.mainBedroom, Lights.Mode.Flicker);
                    lights.SetLight(lights.secondFloor, Lights.Mode.On);
                }
                else if (doorBlock) lights.SetLight(lights.mainBedroom, Lights.Mode.On);

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
