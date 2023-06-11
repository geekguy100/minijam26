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
    public CinemachineVirtualCamera lightvcam;

    //movement
    public float movementSpeed;
    //similar to character controller
    Quaternion _startOrientation;
    [SerializeField]
    public float turnSpeed;// = 300.0f;

    public bool LockCamera = false;

    GameObject _player;
    CinemachineVirtualCamera _playerCam;
    PlayerMovementComponent _playerMovement;
    public LightControlInterable lightControlnteractable;

    Vector3 transformToSee;
    #endregion

    #region Tile Interact
    [Header("Tile Interact")]
    [SerializeField] private Transform LightOrigin;
    [SerializeField] private LayerMask TileLayerMask;
    [SerializeField] private float TileDetectionRange;
    [SerializeField] private GameObject tileSelected;
    [SerializeField] private GridTile tileActive;
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
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ControlLightCamera()
    {
        //TO DO: may add enter or remove it entirely
        if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(controls.ExitLightKey))
        {
            //for now if they see somewhere they want to keep looking at, press enter
            // will definately need to change it
            LockCamera = true;
            ExitLightControlSteering();
        }

        MouseLook();
    }

    float speed = 0.01f;
    float timeCount = 0.0f;
    //getting and setting mouse look
    private void MouseLook()
    {
        if (!LockCamera)
        {
            //look at transform from mouse to screen point
            Vector3 mouse = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mouse);
            if (Physics.Raycast(castPoint, out RaycastHit hitInfo, Mathf.Infinity))
            {
                GameObject hitTransform = hitInfo.transform.gameObject;
                //objectToMove.transform.position = hit.point;
                transformToSee = hitInfo.point;

                //tile check (might not need anymore TBD)
                if (hitTransform.TryGetComponent(out GridTile Tile))
                {
                    //if raycast hits a tile (TO DO: overhaul this once we have more backend tile stuff ready)
                    if (tileSelected != hitTransform.transform.gameObject)
                    {
                        tileSelected = hitTransform.transform.gameObject;
                        tileActive = Tile;
                        //TO DO: run event here maybe for on tile select
                         //Debug.Log("Hit.");
                    }
                }

                //make rotation
                Vector3 relativePos = transformToSee - LightOrigin.position;

                // the second argument, upwards, defaults to Vector3.up
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                LightOrigin.transform.rotation = Quaternion.Lerp(LightOrigin.rotation, rotation, timeCount * speed);
                timeCount = timeCount + Time.deltaTime;

            }
        }

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
        _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Exit Light control steering, switch back to player!");
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
