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

    private Team team = Team.Grey;

    Vector3 translationVector;
    Vector2 rotationVector;
    Vector2 currentDirection;

    private bool spawning = true;
    public Action<Ship> onShipFailed;
    private void Update()
    {
        

        if (!spawning)
        {
            currentDirection = transform.forward.ToVector2();
            rotationVector = Vector2.Lerp(currentDirection, targetDirection, rotateSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up, Vector2.SignedAngle(currentDirection, rotationVector));

            translationVector = targetDirection.ToVector3() * translationSpeed * Time.deltaTime;
            transform.Translate(translationVector, Space.World);

            TESTFUNCTION();
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
}
