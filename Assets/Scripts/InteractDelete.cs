using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDelete : Interactable
{
    [SerializeField] GameObject[] _destroyObjects;

    public bool firstKey;
    public bool secondKey;
    public bool thirdKey;

    public override void Interact(GameObject actuator)
    {
        foreach(GameObject player in _interactedBy)
        {
            if(player == actuator)
            {
                if (firstKey)
                {
                    lights.SetLight(lights.restroomLivingroom, Lights.Mode.On);
                    lights.SetLight(lights.bathroom, Lights.Mode.Flicker);
                }
                else if (secondKey)
                {
                    lights.SetLight(lights.dinnerKitchen, Lights.Mode.Flicker);
                    lights.SetLight(lights.bathroom, Lights.Mode.On);
                }
                else if (thirdKey)
                {
                    lights.SetLight(lights.dinnerKitchen, Lights.Mode.On);
                    lights.SetLight(lights.secondFloor, Lights.Mode.Flicker);
                }

                            foreach (GameObject destroy in _destroyObjects)
                            {
                                destroy.SetActive(false);
                            }
            }
        }
        this.enabled = false;
    }
}
