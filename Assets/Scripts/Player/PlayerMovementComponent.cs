using UnityEngine;

public class PlayerMovementComponent : MonoBehaviour
{
    //[SerializeField] private PlayerInteractorControls controls;
    //movement
    public float movementSpeed;

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
    public Transform PlayerHead;

    private Vector3 vel;
    private Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }
    
    private void SetUp()
    {
        rb = GetComponent<Rigidbody>();
        //_head = GetComponentInChildren<Camera>().transform;

        _bodyStartOrientation = transform.localRotation;
        _headStartOrientation = PlayerHead.transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");

        _yaw += mouseX * turnSpeed;
        _pitch -= mouseY * turnSpeed;
        _pitch = Mathf.Clamp(_pitch, _headLowerAngleLimit, _headUpperAngleLimit);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        vel = new Vector3(horizontal, 0f, vertical).normalized * movementSpeed;
        vel.y = rb.velocity.y;
        
        MouseLook();
    }

    private void FixedUpdate()
    {
        DoMovement();
    }

    //getting and setting mouse look
    private void MouseLook()
    {
        var bodyRotation = Quaternion.AngleAxis(_yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(_pitch, Vector3.right);

        transform.localRotation = bodyRotation * _bodyStartOrientation;
        PlayerHead.localRotation = headRotation * _headStartOrientation;
    }

    private void DoMovement()
    {
        rb.velocity = transform.TransformDirection(vel);
    }
}
