using System.Collections;
using UnityEngine;

public class ShipRotationBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsShip;

    private void OnTriggerEnter(Collider other)
    {
        int incomingLayer = 1 << other.gameObject.layer;
        if ((incomingLayer & whatIsShip) == 0)
        {
            return;
        }

        if (!other.TryGetComponent(out Ship ship))
        {
            return;
        }

        StartCoroutine(ChangeShipDirection(ship));
    }
    
    private IEnumerator ChangeShipDirection(Ship ship)
    {
        print("Starting change ship direction");
        Transform shipTransform = ship.transform;
        Vector2 tilePos = transform.position.ToVector2();
        if(shipTransform == null)
        {
            yield break;
        }
        // Wait until the ship is at the center of the tile.
        yield return new WaitUntil(() => Vector2.Distance(shipTransform.position.ToVector2(), tilePos) < 0.1f);

        print("Changing ship direction to " + transform.forward.ToVector2());
        ship.OnDirectionChange(transform.forward.ToVector2());
    }
}
