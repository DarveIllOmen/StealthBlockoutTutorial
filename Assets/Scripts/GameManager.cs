using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Fields

    public GameObject[] _players;
    //Ozzie 0
    //Matilda 1
    public GameObject[] _enemyList;
    public bool _win;
    public bool _lost;

    #endregion

    #region Variables

    public static GameManager Instance;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _lost = true;
        foreach(GameObject player in _players)
        {
            if(player.GetComponent<Movement>() != null
                && !player.GetComponent<Movement>()._stunned)
            {
                _lost = false;
            }
        }
        if (_lost || Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (_win)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Cursor.lockState = CursorLockMode.None;
        }
    }

    #endregion
}
