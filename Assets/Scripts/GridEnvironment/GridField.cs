using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridField : MonoBehaviour
{
    [SerializeField]
    private int gridWidth = 10;
    [SerializeField]
    private int gridHeight = 10;
    [SerializeField]
    private float widthSpacing = 5;
    [SerializeField]
    private float heightSpacing = 5;

    [SerializeField]
    private GameObject gridTileObject;

    private List<GridTile> gridLayout; // represents the entire [gridWidth, gridHeight] map

    private void Start()
    {
        GenerateField();
    }
    private void GenerateField()
    {
        gridLayout = new List<GridTile>();
        GridTile tile;
        Vector2 coordinate;
        //right now only generates blank tiles
        //map generation logic can potentially be here
        for(int idx = 0; idx < gridWidth * gridHeight; idx++)
        {
            coordinate = GetCoordinateFromIndex(idx);
            tile = Instantiate(gridTileObject, transform).GetComponent<GridTile>();
            tile.AssignPosition(coordinate);

            tile.transform.position = (new Vector2(coordinate.x * widthSpacing, coordinate.y * heightSpacing)).ToVector3();
        }

    }

    /// <summary>
    /// Returns the index of the respective tile in gridLayout from a 2d (x,y) coordiante
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GetIndexFromCoordinate(Vector2 coordinate)
    {
        return (int)(coordinate.x * gridHeight + coordinate.y);
    }

    /// <summary>
    /// Returns a Vector2(x,y) coordinate based on the index in gridLayout
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    private Vector2 GetCoordinateFromIndex(int idx)
    {
        int x = idx / gridHeight;
        int y = idx - x * gridHeight;
        return new Vector2(x, y);
    }
}