using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GridField))]
public class ShipScoringManager : MonoBehaviour
{
    [SerializeField]
    public List<Material> teamColors;
    GridField grid;

    public int TESTSCORE = 0;
    private void Awake()
    {
        grid = GetComponent<GridField>();
    }

    public void AddShipGoalPair(Ship ship, ShipGoal goal)
    {
        Team team = (Team)Random.Range(1, 4);
        ship.AssignTeam(team);
        goal.AssignTeam(team);
        goal.OnGoalSuccess+= ProcessGoalCompletion;


        ship.AssignMaterial(teamColors[(int)team]);
        goal.AssignMaterial(teamColors[(int)team]);
    }

    private void RemoveShipGoalPair(Ship ship, ShipGoal goal)
    {
        goal.AssignMaterial(teamColors[0]);
        goal.RemoveGoal();
    }

    private void ProcessGoalCompletion(Ship ship,ShipGoal goal)
    {
        IncrementScore(1);
        ship.DestroyShip();

        goal.AssignMaterial(teamColors[0]);
        goal.RemoveGoal();
    }


    private void IncrementScore(int value)
    {
        TESTSCORE += value;
    }

}
