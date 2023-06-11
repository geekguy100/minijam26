using System;
using System.Collections;
using UnityEngine;

public class ShipGoal : MonoBehaviour
{
    [SerializeField] private LayerMask shipLayer;

    private Team colorTeam;
    public Team Team => colorTeam;
    public Action<Ship,ShipGoal> OnGoalSuccess;
    //public UnityEvent OnGoalFailed;

    private void Start()
    {
        shipLayer = LayerMask.GetMask("Ship");
    }

    public void AssignTeam(Team color)
    {
        colorTeam = color;
    }
    public void AssignMaterial(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }
    private void OnTriggerEnter(Collider other)
    {
        int incomingLayer = 1 << other.gameObject.layer;
        if ((incomingLayer & shipLayer) == 0)
        {
            return;
        }

        if (!other.TryGetComponent(out Ship ship))
        {
            return;
        }

        if (ship.Team == colorTeam)
        {
            OnGoalSuccess?.Invoke(ship,this);
        }
        //StartCoroutine(CheckShip(ship));
    }

    public void RemoveGoal()
    {

        Destroy(this);
    }

    private IEnumerator CheckShip(Ship ship)
    {
        Transform shipTransform = ship.transform;
        Vector2 tilePos = transform.position.ToVector2();

        // Wait until the ship is at the center of the tile.
        yield return new WaitUntil(() => Vector2.Distance(shipTransform.position.ToVector2(), tilePos) < 0.01f);

        if (ship.Team == colorTeam)
        {
            OnGoalSuccess?.Invoke(ship,this);
        }
    }

}
