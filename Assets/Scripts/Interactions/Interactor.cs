﻿using System;
using UnityEngine;

/// <summary>
/// An object capable of interacting with IInteractables.
/// </summary>
public class Interactor : MonoBehaviour
{
    private IInteractable currentInteractable;
    public bool HasInteractable => !ReferenceEquals(currentInteractable, null);
    
    public void AssignInteractable(IInteractable interactable)
    {
        UnassignInteractable();

        currentInteractable = interactable;
        currentInteractable.OnAssigned();
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