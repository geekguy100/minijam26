using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    private Vector2 gridPosition;
    private void Start()
    {
        
    }


    public void AssignPosition(Vector2 coordinates)
    {
        gridPosition = coordinates;

    }
}
