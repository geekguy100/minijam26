using System;
using System.Collections;
using UnityEngine;

public class ShipRotationBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer SpriteRend
    {
        get
        {
            if (ReferenceEquals(spriteRenderer, null))
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            return spriteRenderer;
        }
    }
    
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

    private void OnTriggerExit(Collider other)
    {
        int incomingLayer = 1 << other.gameObject.layer;

        if ((incomingLayer & whatIsShip) == 0)
            return;

        initialized = false;
        SpriteRend.enabled = false;
    }

    private IEnumerator ChangeShipDirection(Ship ship)
    {
        Transform shipTransform = ship.transform;
        Vector2 tilePos = transform.position.ToVector2();

        // Wait until the ship is at the center of the tile.
        yield return new WaitUntil(() => shipTransform == null || Vector2.Distance(shipTransform.position.ToVector2(), tilePos) < 0.5f);

        if (shipTransform == null)
            yield break;

        Vector3 destination = tilePos.ToVector3();
        destination.z = shipTransform.position.z;
        shipTransform.position = destination;

        ship.OnDirectionChange(transform.forward.ToVector2());

        initialized = false;
    }
}
