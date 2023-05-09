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

    private void OnTriggerStay(Collider other)
    {
        Movement player = other.transform.GetComponent<Movement>();
        if (player != null)
        {
            player._isDark = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Movement player = other.transform.GetComponent<Movement>();
        if (player != null)
        {
            player._isDark = false;
        }
    }

    #endregion
}
