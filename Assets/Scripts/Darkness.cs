using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    #region Fields

    [SerializeField] Vector3 _size = Vector3.one;
    [SerializeField] GameObject _debugBox;
    [SerializeField] BoxCollider _collider;

    #endregion

    #region Variables

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        _debugBox.SetActive(false);
    }

    private void OnValidate()
    {
        _debugBox.transform.localScale = _size;
        _collider.size = _size;
    }

    #endregion

    #region Collisions && Triggers

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Matilda")
        {
            other.gameObject.GetComponent<Movement>()._isDark = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Matilda")
        {
            other.gameObject.GetComponent<Movement>()._isDark = false;
        }
    }

    #endregion
}
