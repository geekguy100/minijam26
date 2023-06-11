using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    private List<int> goalIndexes; // list of potential goal locations
    private GridTile goalTile;
    public int GetGoalIndex()
    {
        return goalIndexes[Random.Range(0, goalIndexes.Count)];
    }

    public void ConfigureShip(Ship ship, Vector2 direction)
    {
        ship.transform.position = transform.position;
        ship.transform.Translate(0, 2, 0);
        ship.AssignDirection(direction);
    }

    public void AssignGoals(List<int> goals)
    {
        goalIndexes = goals;
    }
}
