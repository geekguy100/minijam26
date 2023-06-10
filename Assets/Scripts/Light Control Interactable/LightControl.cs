using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    /*
     * To do:
     * - Player can interact
     * - will inherit from interactable
     * - will need have info accessible for map to read from for proper orientation sync
     * - can either spin left or right
     * 
     * Questions/Things to consider:
     * - how do we want the player to interact?
     *      - player should be able to turn one way or another and must go below decks to see if they see anything new
     *      - should rotate via mouse input
     * 
     * - how should we animation / show the rotation
     * 
     * 
     * 
     * - isotripic birds eye view
     * - will have the tile selector build in (no more telescope)
     * - 
     */

    public GameObject _playerTemp;
    public bool InteractSimulate = false;

    //whatever direction we are currently facing, that should be the only visible area outside
    //will have the fog of war be a seperate component?
    //will read from this orientation to determine rotion of our cone for visibility
    //may need more detail, could have the entire transform or rotation objs
    public float currentOrientation;

    //Camera _lightControlCamera;
    public CinemachineVirtualCamera lightvcam;

    //movement
    public float movementSpeed;
    //similar to character controller
    Quaternion _startOrientation;
    private Vector3 _vel;
    private Rigidbody _rb;
    [SerializeField]
    public float turnSpeed;// = 300.0f;
    [SerializeField]
    float _headUpperAngleLimit = 85f;
    [SerializeField]
    float _headLowerAngleLimit = -80.0f;
    float _yaw = 0, _pitch = 0;
    public Transform LightTransform;

    public bool LockCamera = false;

    GameObject _player;
    CinemachineVirtualCamera _playerCam;
    PlayerMovementComponent _playerMovement;


    //snappiness
    float xAccumulator, yAccumulator;
    const float Snappiness = 10.0f;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //temp will remove (will be called else where via interaction)
        if(InteractSimulate)
        {
            
            SetUpLightControlSteering(_playerTemp);
        }

        //only steer if we are currently using the LightControlCamera
        if (lightvcam.enabled)
        {
            currentOrientation = transform.rotation.y;
            ControlLightCamera();
        }

    }
    
    //runs when player interacts with the light control, switch from player cam to this cam
    public void SetUpLightControlSteering(GameObject PlayerInstigator)
    {
        Debug.Log("Setting up Lighthouse Camera...");
        _player = PlayerInstigator;
        _playerCam = PlayerInstigator.GetComponentInChildren<CinemachineVirtualCamera>();
        _playerMovement = PlayerInstigator.GetComponentInChildren<PlayerMovementComponent>();
        if(_playerCam == null || _playerMovement == null)
        {
            //test print (will remove)
            Debug.Log("NO CAMERA OR MOVEMENT FOUND ON PLAYER!" + _playerCam == null + " " +  _playerMovement == null);
            return;
        }

        //temp will remove
        InteractSimulate = false;

        //if we know the player caused interaction, we can disable movement on player and use the LightControl movement
        _playerMovement.enabled = false;
        lightvcam.gameObject.SetActive(true);
        lightvcam.enabled = true;
        _playerCam.enabled = false;
        _playerCam.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _startOrientation = transform.localRotation;
        LockCamera = false;
    }

    void ControlLightCamera()
    {
        
        if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Escape))
        {
            //for now if they see somewhere they want to keep looking at, press enter
            // will definately need to change it
            LockCamera = true;
            ExitLightControlSteering();
        }

        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");

        _yaw += mouseX * turnSpeed;
        _pitch -= mouseY * turnSpeed;
        _pitch = Mathf.Clamp(_pitch, _headLowerAngleLimit, _headUpperAngleLimit);

        MouseLook();
    }

    
    //getting and setting mouse look
    private void MouseLook()
    {
        if (!LockCamera)
        {
            var bodyRotation = Quaternion.AngleAxis(_yaw, Vector3.up);
            var headRotation = Quaternion.AngleAxis(_pitch, Vector3.right);
            Quaternion rot = headRotation * bodyRotation * _startOrientation;
            transform.localRotation = new Quaternion(rot.x, rot.y, 0, rot.w);
        }

        AimLightRaycastCheck();
    }

    public GameObject raycasthit;
    //while we are active, we are checking what tile we are raycasting to
    void AimLightRaycastCheck()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position;
        Debug.DrawRay(transform.position, -Vector3.up, Color.red, 5);
        if (Physics.Raycast(rayStart, transform.TransformDirection(-transform.up), out hit, 15))
        {
            if (hit.transform.gameObject != this.gameObject)
            {
                //check if its tile
                Debug.Log(hit.transform.name);
                raycasthit = hit.transform.gameObject;
                //if raycast hits a tile
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Tile")
                {


                }
            }
        }
    }

    //when player exits the camera, we have to switch it back to the player camera
    public void ExitLightControlSteering()
    {
        
        _playerMovement.enabled = true;
        lightvcam.enabled = false;
        lightvcam.gameObject.SetActive(false);
        _playerCam.enabled = true;
        _playerCam.gameObject.SetActive(true);
        Debug.Log("Exit Light control steering, switch back to player!");
    }
}
