using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] string _switchButtonName;

    private int _index = 0;

    private void Start()
    {
        GameManager.Instance._players[_index].GetComponent<Movement>().Activate(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown(_switchButtonName))
        {
            GameManager.Instance._players[_index].GetComponent<Movement>().Activate(false);
            _index++;
            if (_index >= GameManager.Instance._players.Length) _index = 0;
            GameManager.Instance._players[_index].GetComponent<Movement>().Activate(true);
        }
    }
}
