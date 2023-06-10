
using UnityEngine;

/// <summary>
/// A contract all interactable objects must implement.
/// </summary>
public interface IInteractable
{
    void OnAssigned();
    void PerformInteraction();
    void OnUnassigned();
    GameObject GetGameObject();
    string GetName();
    string GetDescription();
}