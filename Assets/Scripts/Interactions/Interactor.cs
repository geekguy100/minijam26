using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An object capable of interacting with IInteractables.
/// </summary>
public class Interactor : MonoBehaviour
{
    public bool HasInteractable => !ReferenceEquals(currentInteractable, null);
    public LayerMask WhatIsInteractable => whatIsInteractable;

    private IInteractable currentInteractable;

    [SerializeField] private LayerMask whatIsInteractable;
    [SerializeField] private UnityEvent<IInteractable> onAssigned;
    [SerializeField] private UnityEvent onUnassigned;
    
    public void AssignInteractable(IInteractable interactable)
    {
        // If we try to assign the same interactable, return.
        if (HasInteractable && MatchesInteractable(interactable.GetGameObject()))
            return;

        // If the GameObject is not on the appropriate layer, return.
        int incomingLayer = 1<<interactable.GetGameObject().layer;
        if ((incomingLayer & whatIsInteractable) == 0)
            return;

        UnassignInteractable();

        currentInteractable = interactable;
        currentInteractable.OnAssigned();
        
        print("Assigned interactable");
        
        onAssigned?.Invoke(interactable);
    }

    public void Interact()
    {
        if (!HasInteractable)
            return;
        
        currentInteractable.PerformInteraction();
    }
    
    public void UnassignInteractable()
    {
        if (!HasInteractable)
            return;
        
        currentInteractable.OnUnassigned();
        currentInteractable = null;
        
        print("Unassigned interactable");

        onUnassigned?.Invoke();
    }

    /// <summary>
    /// Returns true if the current interactable's GameObject matches the provided GameObject.
    /// </summary>
    /// <param name="interactable">The incoming interactable's GameObject.</param>
    /// <returns>True if the current interactable matches the provided one.</returns>
    public bool MatchesInteractable(GameObject interactable)
    {
        if (!HasInteractable)
            return false;

        return interactable == currentInteractable.GetGameObject();
    }
}