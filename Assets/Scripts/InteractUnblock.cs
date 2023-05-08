using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUnblock : InteractMove
{
    public GameObject door;
    public override void Interact(GameObject actuator)
    {
        Destroy(door);
        base.Interact(actuator);
    }
}
