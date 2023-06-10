
using UnityEngine;

/// <summary>
/// An interactable responsible for toggling the power.
/// </summary>
public class PowerSwitchInteractable : MonoBehaviour, IInteractable
{
    private bool isFlippedOn;

    [SerializeField] private BoolChannel powerChannel;
    
    public void OnAssigned()
    {
    }

    public void PerformInteraction()
    {
        isFlippedOn = !isFlippedOn;
        // TODO: Play an animation.
        
        powerChannel.RaiseEvent(isFlippedOn);
    }

    public void OnUnassigned()
    {
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetName()
    {
        return "Breaker Switch";
    }

    public string GetDescription()
    {
        return "turn " + (isFlippedOn ? "off " : "on ");
    }
}