using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    //movement
    public float movementSpeed;
    Vector3 _moveVal;
    Transform _mainTransform;

    //mouse look
    [SerializeField]
    public float turnSpeed;// = 300.0f;
    [SerializeField]
    float _headUpperAngleLimit = 85f;
    [SerializeField]
    float _headLowerAngleLimit = -80.0f;
    float _yaw = 0, _pitch = 0;
    Quaternion _bodyStartOrientation;
    Quaternion _headStartOrientation;
    Transform _head;


    // Start is called before the first frame update
    void Start()
    {

        SetUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        MouseLook();
        DoMovement();
    }

    void SetUp()
    {
        _mainTransform = transform;
        _head = GetComponentInChildren<Camera>().transform;


        _bodyStartOrientation = transform.localRotation;
        _headStartOrientation = _head.transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    //getting and setting mouse look
    void MouseLook()
    {
        //mousePosition = Mouse.current.position.ReadValue();
        var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;

        var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;

        _yaw += horizontal;
        _pitch -= vertical;

        _pitch = Mathf.Clamp(_pitch, _headLowerAngleLimit, _headUpperAngleLimit);

        var bodyRotation = Quaternion.AngleAxis(_yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(_pitch, Vector3.right);

        transform.localRotation = bodyRotation * _bodyStartOrientation;
        _head.localRotation = headRotation * _headStartOrientation;



    }

    void DoMovement()
    {

        _moveVal = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (_moveVal != Vector3.zero)
        {
            //Debug.Log("Input: " + moveVal);
            _moveVal.Normalize();
            _moveVal = _mainTransform.TransformDirection(_moveVal);
            _moveVal *= Time.deltaTime * movementSpeed;

            //rb.MovePosition(transform.position + moveDir);

            _mainTransform.Translate(_moveVal, Space.World);
        }

    }


}
