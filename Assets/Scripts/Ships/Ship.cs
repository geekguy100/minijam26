using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System;
using System.Runtime.CompilerServices;

public enum Team
{
    Grey,
    Red,
    Blue,
    Green
}
public class Ship : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float currentSpeed = 2f;
    [SerializeField] private float acceleration = .5f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float difficultyValue = 1f;
    [SerializeField] private List<MeshRenderer> colorChangeElements;
    public Vector3 offset;
   // public Vector3 rotationAxis;
    private Vector2 targetDirection = new Vector2(1, 0);

    public float DifficultyValue => difficultyValue;
    private Team team = Team.Grey;

    Vector3 translationVector;
    Vector2 rotationVector;
    Vector2 currentDirection;
    public Action<Ship> onShipOutOfBounds;
    public Action<Ship> onShipDestroy;
    //was true
    private bool spawning = false;
    public Action<Ship> onShipFailed;
    
    #region Crash
    [Header("Crash Stats")]
    public GameObject crashAsset;
    public GridEnviornment currentTile;
    public GridEnviornment prevTile;
    GameObject ctile;

    public float yAltCheck = -3.5f;
    public Vector3 rotEulerAngleCheck;
    bool sinkMovementActive = false;
    public float ShipRadius = 4;
    private Vector3 sinkDirection = new Vector3(0, -1, 0);
    public float CrashAngleSpeed;
    public float SinkAltSpeed;
    public Action<Ship> onShipCrash;
    #endregion

    [Header("Debug Gizmos")]
    public bool gizmosOn = true;

    private void Start()
    {
        currentSpeed = startSpeed;
    }
    private void Update()
    {
        if (!spawning)
        {
            currentSpeed += acceleration * Time.deltaTime;

            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            //transform.rotation = transform.rotation * Quaternion.Euler(offset);

            currentDirection = (transform.forward).ToVector2();
            rotationVector = Vector2.Lerp(currentDirection, targetDirection, rotateSpeed * Time.deltaTime);

            //transform.Rotate(Vector3.up, Vector2.SignedAngle(currentDirection, rotationVector));
            //transform.rotation = Quaternion.Euler(offset) * transform.rotation;


            //targetDirection = targetDirection;

            
            float rotVal = -90 * targetDirection.x;
            if (targetDirection.y == 1) { rotVal = 180; }
            

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, rotVal, 0)) * Quaternion.Euler(offset), Time.deltaTime * rotateSpeed);
            
            if (!sinkMovementActive)
            {
                translationVector = targetDirection.ToVector3() * currentSpeed * Time.deltaTime;
                transform.Translate(translationVector, Space.World);

                //TESTFUNCTION();
            }

        }
    }

    void FixedUpdate()
    {
        if (!sinkMovementActive)
        {
            CheckCollision();

        }
        else
        {
            SinkMovement();
        }
    }

    private void TESTFUNCTION()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnDirectionChange(new Vector2(0, 1));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            OnDirectionChange(new Vector2(0, -1));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnDirectionChange(new Vector2(-1, 0));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            OnDirectionChange(new Vector2(1,0));
        }

    }
    public void DestroyShip()
    {
        onShipDestroy?.Invoke(this);
        Destroy(this.gameObject);
    }

    public void FadeIn()
    {
        spawning = true;
        MeshRenderer[] renderGroup = GetComponentsInChildren<MeshRenderer>();
        if (renderGroup.Length <= 0) { return; }
        foreach (MeshRenderer render in renderGroup)
        {
            Color shipColor = render.material.color;
            shipColor = new Color(shipColor.r, shipColor.g, shipColor.b, 0);
            render.material.color = shipColor;
        }
        StartCoroutine(FadeInSequence());
    }
    public void FadeOut()
    {
        MeshRenderer[] renderGroup = GetComponentsInChildren<MeshRenderer>();
        if(renderGroup.Length <= 0) { return; }
        StartCoroutine(FadeOutSequence());
    }

    private IEnumerator FadeOutSequence()
    {
        MeshRenderer[] renderGroup = GetComponentsInChildren<MeshRenderer>();
        while (renderGroup[0].material.color.a > 0)
        {
            foreach(MeshRenderer render in renderGroup)
            {
                Color shipColor = render.material.color;
                float fadeAmount = shipColor.a - (.5f * Time.deltaTime);

                shipColor = new Color(shipColor.r, shipColor.g, shipColor.b, fadeAmount);
                render.material.color = shipColor;
            }
            yield return null;
        }

        DestroyShip();
    }

    private IEnumerator FadeInSequence()
    {
        MeshRenderer[] renderGroup = GetComponentsInChildren<MeshRenderer>();
        while (renderGroup[0].material.color.a < 1)
        {
            foreach (MeshRenderer render in renderGroup)
            {
                Color shipColor = render.material.color;
                float fadeAmount = shipColor.a + (1.5f * Time.deltaTime);

                shipColor = new Color(shipColor.r, shipColor.g, shipColor.b, fadeAmount);
                render.material.color = shipColor;
            }
            yield return null;
        }
        spawning = false;
    }

    public void OnOutOfBounds()
    {
        //StartCoroutine(FadeOutSequence());
        //StartCoroutine(OutOfBoundsSequence());
        onShipOutOfBounds?.Invoke(this);
    }

    private IEnumerator OutOfBoundsSequence()
    {

        MeshRenderer[] renderGroup = GetComponentsInChildren<MeshRenderer>();
        while (renderGroup[0].material.color.a > 0)
        {
            yield return null;
        }
        
    }


    /// <summary>
    /// Signals the ship to change to a specified direction
    /// </summary>
    /// <param name="dir"></param> limited range of -1,1
    public void OnDirectionChange(Vector2 dir)
    {
        if(targetDirection == dir) { return;  } //Ship already heading in that direction
        if((targetDirection * -1) == dir) { return; } // Signalling ship to reverse to previous direction, does nothing so far

        targetDirection = dir.normalized;
    }

    /// <summary>
    /// Immediately changes the ship's direction
    /// </summary>
    /// <param name="dir"></param>
    public void AssignDirection(Vector2 dir)
    {
        targetDirection = dir;
        
        float rotVal = -90 * dir.x;
        if(dir.y == 1) { rotVal = 180; }
        // 90  =  1,  0 
        // 180 =  0,  1
        // 270 = -1,  0
        // 360 =  0, -1

        //transform.rotation = Quaternion.Euler(new Vector3(0, rotVal, 0) + offset);
        transform.rotation = Quaternion.Euler(new Vector3(0, rotVal, 0));
        transform.rotation = transform.rotation * Quaternion.Euler(offset); 

    }

    public void AssignTeam(Team color)
    {
        team = color;
    }

    public void StopShip()
    {
        currentSpeed = 0;
        FadeOut();
    }
    public void AssignMaterial(Material mat)
    {
        if(colorChangeElements.Count <= 0)
        {
            return;
        }
        foreach(MeshRenderer render in colorChangeElements)
        {
            render.material = mat;
        }
        
    }
    public Team Team { get { return team; } set { team = value; } }

    //could use a sphere cast
    void CheckCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ShipRadius);
        foreach (var hitCollider in hitColliders)
        {
            GameObject go = hitCollider.gameObject;

            if (LayerMask.LayerToName(go.transform.gameObject.layer) == "TileInteractable" &&
                go.transform.TryGetComponent<GridEnviornment>(out GridEnviornment tile))
            {
                if (currentTile != tile && prevTile != tile)
                {
                    //have a new tile
                    //check if it is an obstacle
                    TileCheck(tile);
                    //return;
                }
            }


            //collision with another ship
            if (LayerMask.LayerToName(go.transform.gameObject.layer) == "Ship" &&
                go.transform.TryGetComponent<Ship>(out Ship ship))
            {
                if(ship.gameObject != this.gameObject)
                {
                    //we have hit another ship
                    Debug.Log("Collided with Ship: " + ship.gameObject.name);
                    sinkMovementActive = true;
                    //TileCheck(currentTile);
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (gizmosOn)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, ShipRadius);

        }
    }

    Vector3 RandomAngle()
    {
        CrashAngleSpeed = UnityEngine.Random.Range(0.2f, 0.45f);
        float xVal = transform.localEulerAngles.x + UnityEngine.Random.Range(10, -30f);
        float yVal = transform.localEulerAngles.y + UnityEngine.Random.Range(-10f, 10f);
        float zVal = transform.localEulerAngles.z + UnityEngine.Random.Range(-10.0f, 10.0f);
        return new Vector3(xVal, yVal, zVal);
    }

    void TileCheck(GridEnviornment tile)
    {
        //is this new tile an obstacle? do we need to crash?
        if(tile.Hazard)
        {
            //start sink sequence
            sinkMovementActive = true;
            Debug.Log(transform.gameObject.name + " Has hit an obstacle on tile " + tile.gameObject.name + " and is now crashing. nice job");
            rotEulerAngleCheck = RandomAngle();
            //crashing = true;
            prevTile = currentTile;
            //currentTile = currentTile;

        } else {
            //Debug.Log("Set Tile: " + tile.name);
            //tile is safe, set it as current tile. buisness as usual
            if (currentTile == null)
                prevTile = tile;
            else
                prevTile = currentTile;

            currentTile = tile;

            ctile = currentTile.gameObject;
        }

    }

    //when the ship crashes
    void ShipCrash()
    {
        //check which tile we are currently on or the previous one since technically we will check if the incoming tile is safe 
        //shouldnt need to flat out delte tile, justs instead mark that tile as a hazard and add the asset to the tile
        //when this ship crashes it will stop then sink down
        Debug.Log(transform.name + "Crashed at " + prevTile.name);

        prevTile.SetNewHazard(crashAsset);

        //run event for failure
        onShipCrash?.Invoke(this);

        //for now disasble everything and lock it up
        this.gameObject.SetActive(false);
        this.GetComponent<Ship>().enabled = false;
    }

    void SinkMovement()
    {
        bool alt = this.transform.position.y > yAltCheck;

        //check if we sunk far enough
        if (alt)
        {
            Vector3 sinkVec = sinkDirection * SinkAltSpeed * Time.deltaTime;
            transform.Translate(sinkVec, Space.World);
            Vector3 newRot = Vector3.Lerp(this.transform.localRotation.eulerAngles, rotEulerAngleCheck, CrashAngleSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(newRot);
        }
        else
        {
            //sinkMovementActive = false;
            ShipCrash();
        }
    }
}
