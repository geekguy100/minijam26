using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Ship : MonoBehaviour
{
    [SerializeField] private float translationSpeed = 2f;
    [SerializeField] private float rotateSpeed = 5f;
    private Vector2 targetDirection = new Vector2(1, 0);


    Vector3 translationVector;
    Vector2 rotationVector;
    Vector2 currentDirection;
    private void Update()
    {
        translationVector =  targetDirection.ToVector3() * translationSpeed * Time.deltaTime;
        transform.Translate(translationVector, Space.World);

        currentDirection = transform.forward.ToVector2();
        rotationVector = Vector2.Lerp(currentDirection, targetDirection, rotateSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, Vector2.SignedAngle(currentDirection, rotationVector));

        TESTFUNCTION();
        
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
}
