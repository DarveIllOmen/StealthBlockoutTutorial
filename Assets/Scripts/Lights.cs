using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    public Light[] basement;
    public Light[] restroomLivingroom;
    public Light[] bathroom;
    public Light[] dinnerKitchen;
    public Light[] secondFloor;
    public Light[] mainBedroom;

    public enum Mode
    {
        On,
        Off,
        Flicker
    }
    public Mode mode;

    Dictionary<Light[], Mode> lightLibrary;

    float timer;
    bool timerOn;

    public void Start()
    {
        //Set the Library
        lightLibrary = new Dictionary<Light[], Mode>();
        lightLibrary.Add(basement,Mode.Off);
        lightLibrary.Add(restroomLivingroom, Mode.Off);
        lightLibrary.Add(bathroom, Mode.Off);
        lightLibrary.Add(dinnerKitchen, Mode.Off);
        lightLibrary.Add(secondFloor, Mode.Off);
        lightLibrary.Add(mainBedroom, Mode.Off);

        //Light the basement
        SetLight(basement, Mode.On);
    }

    public void SetLight(Light[] key, Mode toMode)
    {
        lightLibrary[key] = toMode;
    }

    public void Update()
    {
        foreach(Light[] key in lightLibrary.Keys)
        {
            mode = lightLibrary[key];

            foreach(Light light in key)
            {
                switch (mode)
                {
                    case Mode.Off:
                        light.enabled = false;
                        break;
                    case Mode.On:
                        light.enabled = true;
                        break;
                    case Mode.Flicker:

                        if(timer < 0 && timerOn)
                        {
                            light.enabled = transform;
                        }
                        else if(timer < 0 && !timerOn)
                        {
                            light.enabled = false;
                        }

                        break;
                }
            }
        }

        if (timer < 0 && timerOn)
        {
            timer = Random.Range(0.1f,1.6f);
            timerOn = !timerOn;
        }
        else if (timer < 0 && !timerOn)
        {
            timer = Random.Range(0.05f, 0.15f);
            timerOn = !timerOn;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
