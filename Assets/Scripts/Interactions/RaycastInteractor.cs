using UnityEngine;

/// <summary>
/// An interactor that uses raycasts to check for interactables.
/// </summary>
[RequireComponent(typeof(Interactor))]
public class RaycastInteractor : MonoBehaviour
{
    private Interactor interactor;

    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private LayerMask whatIsInteractable;
    [SerializeField] private float interactionDistance;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    /// <summary>
    /// Perform a raycast from the origin to check for interactables.
    /// </summary>
    private void FixedUpdate()
    {
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
        
        // If we hit an interactable, assign it if it is not the one we already have assigned to us.
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance, whatIsInteractable))
        {
            GameObject hitGameObject = hitInfo.transform.gameObject;
            
            if (!hitGameObject.TryGetComponent(out IInteractable interactable))
                return;
            
            interactor.AssignInteractable(interactable);
        }
        // If we did not hit anything and have an interactable assigned, unassign it.
        else if (interactor.HasInteractable)
        {
            interactor.UnassignInteractable();
        }
    }
}