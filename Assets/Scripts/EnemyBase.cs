using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    #region Fields

    [Space(5), Header("Detection"), Space(3)]

    [SerializeField] float _detectionAngle;
    [SerializeField] float _detectionDistance;
    [SerializeField] float _detectionTime;
    [SerializeField] bool _checkThroughWalls;
    [SerializeField] int _visualHintDivider = 5;
    [SerializeField] LayerMask _visibleLayers;


    public enum Behavior
    {
        CHASE,
        ALERT
    }
    public enum PatrolMode
    {
        PINGPONG,
        CYCLE,
        RANDOM,
        STATIC,

    }

    [Space(5), Header("AI"), Space(3)]

    [SerializeField] Behavior _behavior;
    [SerializeField] float _alertDistance;
    [SerializeField] float _alertTimer;
    [SerializeField] float _speed;
    [SerializeField] float _attackRange;
    [SerializeField] float _searchTimer;
    [SerializeField] NavMeshAgent _navMA;
    [SerializeField] PatrolMode _patrolMode;
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] float _patrolTimer;

    #endregion

    #region Variables

    bool _patrolForward = true;
    int _patrolIndex;
    float _pTimer;
    float _dTimer;
    float _sTimer;
    [SerializeField] bool _hasDetected;
    [SerializeField] GameObject _playerDetected;
    [SerializeField] Vector3 _targetPosition;
    RaycastHit hit;
    float _aTimer;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        _navMA.speed = _speed;
        _playerDetected = null;
        _targetPosition = Vector3.zero;
        _sTimer = _searchTimer;
    }

    private void Update()
    {
        //Checks if it Sees the Players
        foreach(GameObject player in GameManager.Instance._players)
        {

            if(!player.GetComponent<Movement>()._stunned
               && Vector3.Distance(transform.position, player.transform.position) <= _detectionDistance
               && Vector3.Angle(transform.forward, player.transform.position - transform.position) <= _detectionAngle/2)
            {
                if(!_checkThroughWalls
                   && Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, _detectionDistance, _visibleLayers))
                {
                    if (hit.transform.gameObject == player)
                    {
                        _playerDetected = player;
                        _targetPosition = player.transform.position;

                        _dTimer = _detectionTime;
                        _hasDetected = true;
                    }
                }
                else if (_checkThroughWalls)
                {
                    _playerDetected = player;
                    _targetPosition = player.transform.position;

                    _dTimer = _detectionTime;
                    _hasDetected = true;
                }
                
            }
            else if(_playerDetected == player)
            {
                _playerDetected = null;
            }
        }

        //Has Seen a player
        if (_hasDetected)
        {
            _dTimer -= Time.deltaTime;
            if(_dTimer <= 0)
            {
                _dTimer = _detectionTime;
                _playerDetected = null;
                _hasDetected = false;
            }

            //How it acts when it sees the player
            switch (_behavior)
            {
                case Behavior.ALERT:

                    _aTimer -= Time.deltaTime;
                    if(_hasDetected
                        && _aTimer <= 0)
                    {
                        for (int i = 0; i < GameManager.Instance._enemyList.Length; i++)
                        {
                            if (Vector3.Distance(transform.position, GameManager.Instance._enemyList[i].transform.position) <= _alertDistance)
                            {
                                GameManager.Instance._enemyList[i].GetComponent<EnemyBase>().Alert(_targetPosition);
                            }
                        }
                        _aTimer = _alertTimer;
                    }

                    break;
                case Behavior.CHASE:

                    if(_hasDetected)
                    {
                        if (Vector3.Distance(transform.position, _targetPosition) > _attackRange)
                        {
                            _navMA.SetDestination(_targetPosition);
                            if(_navMA.isPathStale)
                            {
                                _playerDetected = null;
                                _hasDetected = false;

                                _navMA.ResetPath();
                            }
                        }
                        else if(_playerDetected != null)
                        {
                            _playerDetected.GetComponent<Movement>()._stunned = true;
                            _playerDetected = null;
                            _hasDetected = false;

                            _navMA.ResetPath();
                        }
                        else if(_sTimer > 0)
                        {
                            _sTimer -= Time.deltaTime;
                            if(_sTimer <= 0)
                            {
                                _sTimer = _searchTimer;
                                _hasDetected = false;
                                _navMA.ResetPath();
                            }
                        }
                    }

                    break;
            }
            
        }
        else
        {
            Debug.Log("Patrolling");
            switch (_patrolMode)
            {
                case PatrolMode.CYCLE:

                    _pTimer -= Time.deltaTime;
                    if (_pTimer <= 0)
                    {
                        _navMA.SetDestination(_patrolPoints[_patrolIndex].position);
                        Debug.Log(gameObject.name + _navMA.remainingDistance + _patrolPoints[_patrolIndex].name);
                        if (_navMA.remainingDistance <= 0.02f && !_navMA.pathPending)
                        {
                            _pTimer = _patrolTimer;
                            _patrolIndex++;
                            if (_patrolIndex >= _patrolPoints.Length) _patrolIndex = 0;
                        }
                    }

                    break;
                case PatrolMode.PINGPONG:

                    _pTimer -= Time.deltaTime;
                    if (_pTimer <= 0)
                    {
                        _navMA.SetDestination(_patrolPoints[_patrolIndex].position);

                        if (_navMA.remainingDistance <= 0.02f && !_navMA.pathPending)
                        {
                            _pTimer = _patrolTimer;

                            if (_patrolForward) _patrolIndex++;
                            else _patrolIndex--;

                            if (_patrolIndex == _patrolPoints.Length - 1
                               && _patrolForward)
                            {
                                _patrolForward = false;
                            }
                            if (_patrolIndex == 0
                               && !_patrolForward)
                            {
                                _patrolForward = true;
                            }
                        }

                    }

                    break;
                case PatrolMode.RANDOM:

                    _pTimer -= Time.deltaTime;
                    if (_pTimer <= 0)
                    {
                        _navMA.SetDestination(_patrolPoints[_patrolIndex].position);

                        if (_navMA.remainingDistance <= 0.02f && !_navMA.pathPending)
                        {
                            _pTimer = _patrolTimer;

                            _patrolIndex = Random.Range(0, _patrolPoints.Length);
                        }

                    }

                    break;
                case PatrolMode.STATIC:

                    _navMA.SetDestination(_patrolPoints[0].position);

                    break;
            }
        }
    }

    #endregion

    #region Methods

    public void Alert(Vector3 position)
    {
        if(_behavior != Behavior.ALERT && !_hasDetected)
        {
            _targetPosition = position;

            _hasDetected = true;
            _dTimer = _detectionTime;
        }
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < (int)_detectionAngle/_visualHintDivider + 1; i++)
        {
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(Vector3.up * (i * _visualHintDivider - _detectionAngle / 2)) * transform.forward * _detectionDistance);
        }
    }

    #endregion
}
