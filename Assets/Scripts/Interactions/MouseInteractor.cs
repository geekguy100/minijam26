
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class MouseInteractor : MonoBehaviour
{
    private Interactor interactor;
    private Camera mainCam;

    [SerializeField] private float interactionDistance;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void FixedUpdate()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        
        // If we hit an interactable, assign it if it is not the one we already have assigned to us.
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance, interactor.WhatIsInteractable))
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
