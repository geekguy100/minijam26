using System.Collections;
using UnityEngine;

public class ShipRotationBehaviour : MonoBehaviour
{
    public bool initialized;
    [SerializeField] private LayerMask whatIsShip;

    private void OnTriggerEnter(Collider other)
    {
        if (!initialized)
            return;
        
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

        // Wait until the ship is at the center of the tile.
        yield return new WaitUntil(() => shipTransform == null || Vector2.Distance(shipTransform.position.ToVector2(), tilePos) < 0.5f);

        if (shipTransform == null)
            yield break;

        Vector3 destination = tilePos.ToVector3();
        destination.z = shipTransform.position.z;
        shipTransform.position = destination;

        print("Changing ship direction to " + transform.forward.ToVector2());
        ship.OnDirectionChange(transform.forward.ToVector2());
    }
}
