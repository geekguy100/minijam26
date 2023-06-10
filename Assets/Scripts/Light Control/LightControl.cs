using Cinemachine;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    /*
     * To do:
     * - Player can interact
     * - will inherit from interactable
     * - will need have info accessible for map to read from for proper orientation sync
     * - will have x and y rotation
     *      - player should be able to turn one way or another and must go below decks to see if they see anything new
     *      - should rotate via mouse input
     * 
     * - how should we animation / show the rotation
     * 
     * 
     * 
     * - isotripic birds eye view
     * - will have the tile selector build in (no more telescope)
     * - basic behavior seems good, just need to hook it up and adjust when we get models to work with
     */

    #region Light Steering
    [Header("Control and Steering")]
    public GameObject _playerTemp;
    public bool InteractSimulate = false;
    [SerializeField] private PlayerInteractorControls controls;

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
   // public Transform LightTransform;

    public bool LockCamera = false;

    GameObject _player;
    CinemachineVirtualCamera _playerCam;
    PlayerMovementComponent _playerMovement;
    public LightControlInterable lightControlnteractable;
    #endregion

    #region Tile Interact
    [Header("Tile Interact")]
    [SerializeField] private Transform LightOrigin;
    [SerializeField] private LayerMask TileLayerMask;
    [SerializeField] private float TileDetectionRange;
    [SerializeField] private GameObject tileSelected;
    [SerializeField] private GridTile tileActive;
    public GameObject raycasthit;
    #endregion

    #region Light Env
    LineRenderer LightLR;
    Vector3 EndPos;
    public bool TurnOnLineR = false;
    //TileDetectoionRange to set where the light ends
    #endregion

    private void Start()
    {

        SetupLight();
    }

    // Update is called once per frame
    void Update()
    {
        //temp will remove (will be called else where via interaction)
        if(InteractSimulate)
        {
            
            SetUpLightControlSteering();
        }

        //only steer if we are currently using the LightControlCamera
        if (lightvcam.enabled)
        {
            currentOrientation = transform.rotation.y;
            ControlLightCamera();
        }

    }

    #region Light Steering/Control
    //runs when player interacts with the light control, switch from player cam to this cam
    public void SetUpLightControlSteering()//GameObject PlayerInstigator)
    {
        //_lightControlnteractable = LCI;
        Debug.Log("Setting up Lighthouse Camera...");
        _player = _playerTemp;//PlayerInstigator;
        _playerCam = _player.GetComponentInChildren<CinemachineVirtualCamera>();
        _playerMovement = _player.GetComponentInChildren<PlayerMovementComponent>();
        if(_playerCam == null || _playerMovement == null)
        {
            //test print (will remove)
            Debug.LogWarning("NO CAMERA OR MOVEMENT FOUND ON PLAYER!");
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
        //TO DO: may add enter or remove it entirely
        if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKeyDown(controls.ExitLightKey))
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

        if(TurnOnLineR)
            MoveLightBeam();
    }
    //when player exits the camera, we have to switch it back to the player camera
    public void ExitLightControlSteering()
    {
        lightControlnteractable.OnPlayerExit();
        _playerMovement.enabled = true;
        lightvcam.enabled = false;
        lightvcam.gameObject.SetActive(false);
        _playerCam.enabled = true;
        _playerCam.gameObject.SetActive(true);
        Debug.Log("Exit Light control steering, switch back to player!");
    }
    #endregion

    #region Tile Selection
    //while we are active, we are checking what tile we are raycasting to
    void AimLightRaycastCheck()
    {
        Ray ray = new Ray(LightOrigin.position, LightOrigin.forward);

        Debug.DrawRay(ray.origin, ray.direction * 60f, Color.yellow);

        // If we hit an interactable, assign it if it is not the one we already have assigned to us.
        if (Physics.Raycast(ray, out RaycastHit hitInfo, TileDetectionRange))
        {
            GameObject hitTransform = hitInfo.transform.gameObject;
            if (hitInfo.transform.gameObject != this.gameObject)
            {
                //check if its tile
                raycasthit = hitInfo.transform.gameObject;
                if (hitTransform.TryGetComponent(out GridTile Tile))
                {
                    //if raycast hits a tile (TO DO: overhaul this once we have more backend tile stuff ready)
                    if (tileSelected != hitTransform.transform.gameObject)
                    {
                        tileSelected = hitTransform.transform.gameObject;
                        tileActive = Tile;
                        //TO DO: run event here maybe for on tile select
                       // Debug.Log("Hit.");

                    }
                }
                else
                {
                    //Debug.LogWarning("[Raycast Light Control]: " + hitTransform.name + " has no GridTile component...");
                    return;
                }
            }
        } else if(tileSelected)
        {
            //TO DO: deselect tile (turn off highlight or whatever)


            //unassign the tile selected
            tileSelected = null;
        }
    }

    #endregion

    #region Light Component
    void SetupLight()
    {
        LightLR = GetComponent<LineRenderer>();
        LightLR.positionCount = 2;
        LightLR.startWidth = 0.0f;
        LightLR.SetPosition(0, LightOrigin.position);
        EndPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + TileDetectionRange);
        EndPos = transform.TransformPoint(EndPos);
        LightLR.SetPosition(1, EndPos);
    }

    void MoveLightBeam()
    {
        EndPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + TileDetectionRange);
        EndPos = transform.TransformPoint(EndPos);
        LightLR.SetPosition(1, EndPos);
    }

    #endregion
}
