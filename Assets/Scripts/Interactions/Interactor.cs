using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An object capable of interacting with IInteractables.
/// </summary>
public class Interactor : MonoBehaviour
{
    public bool HasInteractable => !ReferenceEquals(currentInteractable, null);
    private IInteractable currentInteractable;
    
    [SerializeField] private UnityEvent<IInteractable> onAssigned;
    [SerializeField] private UnityEvent onUnassigned;
    
    public void AssignInteractable(IInteractable interactable)
    {
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