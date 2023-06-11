using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GridField))]
public class ShipScoringManager : MonoBehaviour
{
    [SerializeField]
    public List<Material> teamColors;

    private Dictionary<Team, List<ShipGoal>> teamGoals;
    GridField grid;

    public int TESTSCORE = 0;
    public int crashes = 0;
    public int outOfBounds = 0;

    private void Awake()
    {
        grid = GetComponent<GridField>();
        teamGoals = new Dictionary<Team, List<ShipGoal>>();
        foreach(Team team in System.Enum.GetValues(typeof(Team)))
        {
            teamGoals.Add(team, new List<ShipGoal>());
        }

    }

    public void AddShipGoalPair(Ship ship, ShipGoal goal)
    {
        Team team = (Team)Random.Range(1, 4);
        ship.AssignTeam(team);
        goal.AssignTeam(team);
        teamGoals[team].Add(goal);
        goal.OnGoalSuccess+= ProcessGoalCompletion;
        ship.onShipOutOfBounds += OnOutOfBounds;

        ship.AssignMaterial(teamColors[(int)team]);
        goal.AssignMaterial(teamColors[(int)team]);
        ship.FadeIn();
    }

    private void OnOutOfBounds(Ship ship)
    {
        outOfBounds++;
        RemoveGoal(ship);
    }

    private void OnCrash(Ship ship)
    {
        crashes++;
        RemoveGoal(ship);
    }
    private void RemoveGoal(Ship ship)
    {
        ShipGoal goal = teamGoals[ship.Team][Random.Range(0, teamGoals[ship.Team].Count)];

        teamGoals[goal.Team].Remove(goal);
        goal.AssignMaterial(teamColors[0]);
        goal.RemoveGoal();
        ship.FadeOut();
    }
    private void ProcessGoalCompletion(Ship ship,ShipGoal goal)
    {
        IncrementScore(1);

        ship.StopShip();

        teamGoals[goal.Team].Remove(goal);
        goal.AssignMaterial(teamColors[0]);
        goal.RemoveGoal();
    }


    private void IncrementScore(int value)
    {
        TESTSCORE += value;
    }

}
