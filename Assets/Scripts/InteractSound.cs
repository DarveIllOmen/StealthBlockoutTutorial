using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSound : Interactable
{
    [SerializeField] Transform _position;
    [SerializeField] float _alertDistance;

    public override void Interact(GameObject actuator)
    {
        foreach (GameObject player in _interactedBy)
        {
            if (player == actuator)
            {
                for (int i = 0; i < GameManager.Instance._enemyList.Length; i++)
                {
                    if (Vector3.Distance(_position.position, GameManager.Instance._enemyList[i].transform.position) <= _alertDistance)
                    {
                        GameManager.Instance._enemyList[i].GetComponent<EnemyBase>().Alert(_position.position);
                    }
                }
            }
        }

    }
}
