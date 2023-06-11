using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField]
    private int minGoalDistance;
    [SerializeField]
    private GameObject shipObject;
    public Material greenTestMat;
    public Material redTestMat;

    GridField grid;
    private List<SpawnLocation> shipSpawners;
    private List<int> validGoalIndexes;

    private void Awake()
    {
        grid = GetComponent<GridField>();
        grid.onPopulationComplete.AddListener(ProcessScoreGridElements);
        shipSpawners = new List<SpawnLocation>();
    }

    private void Update()
    {
        TESTspawnShipGoalPair();
    }

    private void TESTspawnShipGoalPair()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateShipSpawn();
            //shipSpawners[Random.Range(0, shipSpawners.Count)].SpawnShip();
        }
    }
    /// <summary>
    /// Receives grid layout from GridField.cs and creates Ship Spawn/Goal pairings
    /// </summary>
    private void ProcessScoreGridElements()
    {
        validGoalIndexes = new List<int>();
        List<GridTile> layout = grid.GridLayout;

        SpawnLocation spawner;
        //Find valid Goal locations ||Currently set to edges of the Grid
        for (int x = 0; x < grid.GridWidth; x++)
        {
            validGoalIndexes.Add(grid.GetIndexFromCoordinate(new Vector2(x, 0)));
            validGoalIndexes.Add(grid.GetIndexFromCoordinate(new Vector2(x, grid.GridHeight - 1)));
        }
        for (int y = 1; y < grid.GridHeight - 1; y++)//Smaller Index range to prevent repeating corners
        {
            validGoalIndexes.Add(grid.GetIndexFromCoordinate(new Vector2(0, y)));
            validGoalIndexes.Add(grid.GetIndexFromCoordinate(new Vector2(grid.GridWidth - 1, y)));
        }

        // Find/Create valid Spawn locations || Currently set to edges of the grid
        for (int x = 0; x < grid.GridWidth; x++)
        {
            spawner = grid.GetTile(new Vector2(x, 0)).AddComponent<SpawnLocation>();
            AssignShipGoals(spawner);
            shipSpawners.Add(spawner);

            spawner = grid.GetTile(new Vector2(x, grid.GridHeight - 1)).AddComponent<SpawnLocation>();
            AssignShipGoals(spawner);
            shipSpawners.Add(spawner);
        }
        for (int y = 1; y < grid.GridHeight - 1; y++)//Smaller Index range to prevent repeating corners
        {
            spawner = grid.GetTile(new Vector2(0, y)).AddComponent<SpawnLocation>();
            AssignShipGoals(spawner);
            shipSpawners.Add(spawner);

            spawner = grid.GetTile(new Vector2(grid.GridWidth - 1, y)).AddComponent<SpawnLocation>();
            AssignShipGoals(spawner);
            shipSpawners.Add(spawner);
        }

    }

    /// <summary>
    /// Assigns a list of possible Goal locations for a spawner
    /// </summary>
    /// <param name="spawner"></param>
    private void AssignShipGoals(SpawnLocation spawner)
    {
        Vector2 gridPosition = spawner.GetComponent<GridTile>().GridPosition;
        Vector2 minCoordinate = new Vector2(Mathf.Clamp(gridPosition.x - minGoalDistance, 0, grid.GridWidth - 1), 
                                            Mathf.Clamp(gridPosition.y - minGoalDistance, 0, grid.GridHeight - 1));
        Vector3 maxCoordinate = new Vector2(Mathf.Clamp(gridPosition.x + minGoalDistance, 0, grid.GridWidth - 1),
                                            Mathf.Clamp(gridPosition.y + minGoalDistance, 0, grid.GridHeight - 1));
        spawner.AssignGoals(FindGoalsOutsideConstraints(minCoordinate, maxCoordinate));
    }

    /////////////////could potentially move constraint seaches to gridField for general use
    /// <summary>
    /// returns a list of grid indexes containing all the goals that fit INSIDE the min/max coordinate constraint
    /// </summary>
    /// <param name="min"></param> min (x,y) coordinate
    /// <param name="max"></param> max (x,y) coordinate
    /// <returns></returns>
    private List<int> FindGoalsWithinConstraints(Vector2 min, Vector2 max)
    {
        if (min.x > max.x || min.y > max.y) { Debug.Log("Invalid Constraint Field, Min: " + min + " --- Max: " + max); return new List<int>(); }
        List<int> goalIndexes = new List<int>();
        Vector2 coordinate;
        foreach (int goalIndex in validGoalIndexes)
        {
            coordinate = grid.GetCoordinateFromIndex(goalIndex);

            if (min.x <= coordinate.x && max.x >= coordinate.x && min.y <= coordinate.y && max.y >= coordinate.y)
            {
                goalIndexes.Add(grid.GetIndexFromCoordinate(coordinate));
            }
        }

        return goalIndexes;
    }

    /// <summary>
    /// returns a list of GridIndexes containting all goals that are OUTSIDE the min/max coordinate constraints
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private List<int> FindGoalsOutsideConstraints(Vector2 min, Vector3 max)
    {
        List<int> goalIndexes = new List<int>();

        //Splits search constraints into 4 separate rectangles
        goalIndexes.AddRange(FindGoalsWithinConstraints(new Vector2(0, 0), new Vector2(min.x - 1, grid.GridHeight - 1)));
        goalIndexes.AddRange(FindGoalsWithinConstraints(new Vector2(max.x + 1, 0), new Vector2(grid.GridWidth - 1, grid.GridHeight - 1)));

        goalIndexes.AddRange(FindGoalsWithinConstraints(new Vector2(min.x, 0), new Vector2(max.x, min.y - 1)));
        goalIndexes.AddRange(FindGoalsWithinConstraints(new Vector2(min.x, max.y + 1), new Vector2(max.x, grid.GridHeight - 1)));
        return goalIndexes;
    }


    /// <summary>
    /// Instantiates a ship and Assigns a goal for it
    /// </summary>
    private void GenerateShipSpawn()
    {
        Ship ship = Instantiate(shipObject).GetComponent<Ship>();
        SpawnLocation location = shipSpawners[Random.Range(0, shipSpawners.Count)];
        //SpawnLocation location = shipSpawners[5];
        GridTile goalTile = grid.GetTile(location.GetGoalIndex());
        GridTile startTile = location.GetComponent<GridTile>();

        //TODO:update initial direction
        Vector2 startPosition = startTile.GridPosition;
        Vector2 direction;

        if(startPosition.x == 0) { direction = new Vector2(1, 0); }
        else if(startPosition.x == (grid.GridWidth - 1)) { direction = new Vector2(-1, 0); }
        else if(startPosition.y == 0) { direction = new Vector2(0, 1); }
        else { direction = new Vector2(0, -1); }

        location.ConfigureShip(ship.GetComponent<Ship>(), direction);


        //location.GetGoalIndex();
        //Colors for testing
        goalTile.GetComponent<MeshRenderer>().material = redTestMat;
        location.GetComponent<MeshRenderer>().material = greenTestMat;
        ship.GetComponent<MeshRenderer>().material = redTestMat;
    }
}
