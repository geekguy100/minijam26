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
    [SerializeField] private float translationSpeed = 2f;
    [SerializeField] private float rotateSpeed = 5f;
    private Vector2 targetDirection = new Vector2(1, 0);
    private Vector3 sinkDirection = new Vector3(0, -1, 0);

    private Team team = Team.Grey;

    Vector3 translationVector;
    Vector2 rotationVector;
    Vector2 currentDirection;

    //was true
    private bool spawning = false;
    public Action<Ship> onShipFailed;

    #region Crash
    [Header("Crash Stats")]
    public GridEnviornment currentTile;
    public GridEnviornment prevTile;
    GameObject ctile;
    public GameObject crashAsset;

   // public float sinkSpeed = 5;

    public float yAltCheck = -60.0f;
    public Vector3 rotEulerAngleCheck;
    bool sinkMovementActive = false;
    public float ShipRadius = 4;

    public float CrashAngleSpeed;
    public float SinkAltSpeed;

    #endregion

    [Header("Debug Gizmos")]
    public bool gizmosOn = true;


    void Start()
    {
        //setting -x rotation randomly
        //other 2 can be ok
        float xVal = transform.localEulerAngles.x + UnityEngine.Random.Range(0, -30f);
        float yVal = transform.localEulerAngles.y + UnityEngine.Random.Range(-10f, 10f);
        float zVal = transform.localEulerAngles.z + UnityEngine.Random.Range(-10.0f, 10.0f);
        rotEulerAngleCheck = new Vector3(xVal, yVal, zVal);
    }

    private void Update()
    {
        if (!spawning)
        {
            currentDirection = transform.forward.ToVector2();
            rotationVector = Vector2.Lerp(currentDirection, targetDirection, rotateSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, Vector2.SignedAngle(currentDirection, rotationVector));

            if (!sinkMovementActive)
            {
                translationVector = targetDirection.ToVector3() * translationSpeed * Time.deltaTime;
                transform.Translate(translationVector, Space.World);

                TESTFUNCTION();

                CheckCollision();
            }
            else
            {
                SinkMovement();
            }
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
                float fadeAmount = shipColor.a - (.75f * Time.deltaTime);

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
                float fadeAmount = shipColor.a + (1f * Time.deltaTime);

                shipColor = new Color(shipColor.r, shipColor.g, shipColor.b, fadeAmount);
                render.material.color = shipColor;
            }
            yield return null;
        }
        spawning = false;
    }

    public void OnOutOfBounds()
    {
        StartCoroutine(FadeOutSequence());
        StartCoroutine(OutOfBoundsSequence());
    }

    private IEnumerator OutOfBoundsSequence()
    {
        MeshRenderer[] renderGroup = GetComponentsInChildren<MeshRenderer>();
        while (renderGroup[0].material.color.a > 0)
        {
            yield return null;
        }
        onShipFailed?.Invoke(this);
    }


    /// <summary>
    /// Signals the ship to change to a specified direction
    /// </summary>
    /// <param name="dir"></param> limited range of -1,1
    public void OnDirectionChange(Vector2 dir)
    {
        if(targetDirection.magnitude != 1) { Debug.Log("Invalid entry for OnDirectionChange"); return;  }
        if(targetDirection == dir) { return;  } //Ship already heading in that direction
        if((targetDirection * -1) == dir) { return; } // Signalling ship to reverse to previous direction, does nothing so far

        targetDirection = dir;
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

        transform.rotation = Quaternion.Euler(new Vector3(0, rotVal, 0));
    }

    public void AssignTeam(Team color)
    {
        team = color;
    }

    public void StopShip()
    {
        translationSpeed = 0;
        FadeOut();
    }
    public void AssignMaterial(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }
    public Team Team { get { return team; } set { team = value; } }

    //could use a sphere cast
    void CheckCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, ShipRadius, transform.forward, out hit, ShipRadius))
        {
            //
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "TileInteractable" && 
                hit.transform.TryGetComponent<GridEnviornment>(out GridEnviornment tile))
            {
                //
                if (currentTile != tile && prevTile != tile)
                {
                   // Debug.Log(hit.transform.name);
                    //have a new tile
                    //check if it is an obstacle
                    TileCheck(tile);
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

    void TileCheck(GridEnviornment tile)
    {
        //is this new tile an obstacle? do we need to crash?
        if(tile.Hazard)
        {
            //start sink sequence
            sinkMovementActive = true;
            Debug.Log(transform.gameObject.name + " Has hit an obstacle on tile " + tile.gameObject.name + "and is now crashing. nice job");
            //crashing = true;

        } else {
            Debug.Log("Set Tile: " + tile.name);
            //tile is safe, set it as current tile. buisness as usual
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
        Debug.Log(transform.name + "Crashed.");

        currentTile.SetNewHazard(crashAsset);

        //run event for failure
        onShipFailed?.Invoke(this);

        //Destroy(this.gameObject);

        //for now disasble everything and lock it up
        this.gameObject.SetActive(false);

    }

    
    void SinkMovement()
    {
        bool bypass = false;


        bool alt = this.transform.position.y >= yAltCheck;
        bool rot = this.transform.localRotation.eulerAngles.x <= rotEulerAngleCheck.x;
        if (!bypass)
        {
            //check if we sunk far enough
            
            if (alt)
            {
                Vector3 sinkVec = sinkDirection * SinkAltSpeed * Time.deltaTime;
                transform.Translate(sinkVec, Space.World);
                Vector3 newRot = Vector3.Lerp(this.transform.localRotation.eulerAngles, rotEulerAngleCheck, CrashAngleSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(newRot);
            }
            //check if our small rotate is enough
            
            if (rot)
            {
                //currentDirection = transform.forward.ToVector2();
               // Vector3 newRot = Vector3.Lerp(this.transform.localRotation.eulerAngles, rotEulerAngleCheck, CrashAngleSpeed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(newRot);
                
            }
        } else {
            alt = true;
            rot = true;
        }
       

        //once both the rotate and the sinkage is enough we can run the event for ShipCrash
        if(!alt)
        {
            //sinkMovementActive = false;
            ShipCrash();

        }
    }
}
