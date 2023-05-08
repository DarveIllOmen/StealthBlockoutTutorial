using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Fields

    [Space(5), Header("Movement"), Space(3)]
    [SerializeField] Rigidbody c_rb;
    [SerializeField] float _speed;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashTimer;
    [SerializeField] float _dashAcceleration;
    [SerializeField] string _dashButtonName;
    [SerializeField] GameObject g_stun;

    [Space(5), Header("Camera"), Space(3)]
    [SerializeField] GameObject g_cam;
    [SerializeField] GameObject g_camHolder;
    [SerializeField] LayerMask _camCollisionLayers;
    [SerializeField] float _camDistance;
    [SerializeField] float _sensitivity;
    [SerializeField] GameObject g_darkness;

    [Space(5), Header("Inputs"), Space(3)]
    [SerializeField] string _interactButtonName;
    [SerializeField] LayerMask _interactableLayers;
    [SerializeField] float _interactDistance;

    #endregion

    #region Variables

    [SerializeField] private bool _canInteract = true;
    private Vector3 _velocity;
    private Vector3 _rotation;
    private Vector3 _dashVelocity;
    private float _dTimer;
    private bool _isDashing = false;
    [HideInInspector] public bool _stunned = false;
    [HideInInspector] public bool _active = true;
    public bool _isDark = false;

    #endregion

    #region UnityFunctions

    private void Update()
    {
        if (_active)
        {
            Horizontal();
            Rotate();
            CameraDistance();
            Interact();
            Dash();

            Move();
        }

        g_stun.SetActive(_stunned);
        if(_active)g_darkness.SetActive(_isDark);
    }

    #endregion

    #region Methods

    private void Move()
    {
        c_rb.velocity = (_velocity * _speed + _dashVelocity) * Time.deltaTime * 100;
    }

    private void Horizontal()
    {
        if (!_stunned)
        {
            _velocity = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
            if (_velocity.magnitude > 1) _velocity.Normalize();
        }
        else
        {
            _velocity = Vector3.zero;
        }
        
    }

    private void Rotate()
    {
        _rotation.y += Input.GetAxis("Mouse X") * _sensitivity;
        _rotation.x += Input.GetAxis("Mouse Y") * _sensitivity;
        _rotation.x = Mathf.Clamp(_rotation.x, -90, 90);
        transform.eulerAngles = Vector3.up * _rotation.y;
        g_camHolder.transform.localRotation = Quaternion.Euler(Vector3.right * -_rotation.x);
    }

    private void CameraDistance()
    {
        RaycastHit hit;
        if(Physics.Raycast(g_camHolder.transform.position, -g_camHolder.transform.forward, out hit, _camDistance, _camCollisionLayers))
        {
            g_cam.transform.position = hit.point;
        }
        else
        {
            g_cam.transform.position = g_camHolder.transform.position - g_camHolder.transform.forward * _camDistance;
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,0.5f,0.45f), transform.forward, out hit, _interactDistance, _interactableLayers))
        {
            Debug.Log(hit.transform.name);
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if(interactable != null)
            {
                Debug.Log("Can Interact");
                interactable.ControlText(_interactButtonName);
                if (Input.GetButton(_interactButtonName) && !_stunned && _canInteract)
                {
                    Debug.Log("Interacting");
                    _canInteract = false;
                    interactable.Interact(this.gameObject);
                }
                else if (!Input.GetButton(_interactButtonName))
                {
                    _canInteract = true;
                }
            }

            Movement movement = hit.transform.GetComponent<Movement>();
            if(movement != null && movement._stunned && Input.GetButtonDown(_interactButtonName) && !_stunned && _canInteract)
            {
                Debug.Log("Get Up");
                movement._stunned = false;
                _canInteract = false;
            }
            else if (!Input.GetButton(_interactButtonName))
            {
                _canInteract = true;
            }
        }
    }

    public void Activate(bool isActive)
    {
        _active = isActive;
        g_cam.SetActive(isActive);
        if (isActive) c_rb.mass = 1;
        else
        {
            c_rb.mass = 50;
            c_rb.velocity = Vector3.zero;
        }
    }

    public void Dash()
    {
        _dTimer -= Time.deltaTime;
        if(!_stunned
           && Input.GetButton(_dashButtonName)
           && _dTimer <= 0)
        {
            _dTimer = _dashTimer;
            _isDashing = true;
            _dashVelocity = transform.forward * _dashSpeed * 100;
        }

        if (_isDashing)
        {
            _dashVelocity = Vector3.Lerp(_dashVelocity, Vector3.zero, _dashAcceleration);
            if(_dashVelocity.magnitude <= 0.1f)
            {
                _dashVelocity = Vector3.zero;
                _isDashing = false;
            }
        }
    }

    #endregion
}
