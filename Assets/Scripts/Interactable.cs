using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _hintText;
    [SerializeField] protected float _timer = 0.2f;
    [SerializeField] protected GameObject[] _interactedBy;

    protected float _t;

    public void Update()
    {
        if(_hintText != null)
        {
            if (_t <= 0)
            {
                _hintText.gameObject.SetActive(false);
            }
            else
            {
                _t -= Time.deltaTime;
            }
        }
        
    }

    public virtual void Interact(GameObject actuator) { Debug.Log("Say Something"); }
    public void ControlText(string buttonName) 
    {
        if(_hintText != null)
        {
            _hintText.text = "Press " + buttonName;
            _hintText.gameObject.SetActive(true);
            _t = _timer;
        }
    }
}
