
using UnityEngine;

/// <summary>
/// An interactable responsible for toggling the power.
/// </summary>
public class PowerSwitchInteractable : MonoBehaviour, IInteractable
{
    private bool isFlippedOn;

    [SerializeField] private BoolChannel powerChannel;

    #region Event Subbing
    private void OnEnable()
    {
        powerChannel.OnEventRaised += HandlePowerChange;
    }

    private void OnDisable()
    {
        powerChannel.OnEventRaised += HandlePowerChange;
    }
    #endregion

    public void OnAssigned()
    {
    }
    
    public void OnUnassigned()
    {
    }

    private void HandlePowerChange(bool powerOn)
    {
        isFlippedOn = powerOn;

        if (isFlippedOn)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        
        // TODO: Play an animation.
    }

    public void PerformInteraction()
    {
        powerChannel.RaiseEvent(!isFlippedOn);
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