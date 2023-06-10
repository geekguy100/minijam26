using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControlInterable : MonoBehaviour, IInteractable
{
    private bool isFlippedOn;
    [SerializeField] private BoolChannel lightChannel;

    public LightControl LightControl;

    #region Event Subbing
    private void OnEnable()
    {
        lightChannel.OnEventRaised += HandleLightInteractChange;

    }

    private void OnDisable()
    {
        lightChannel.OnEventRaised += HandleLightInteractChange;

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void HandleLightInteractChange(bool powerOn)
    {
        Debug.Log("HandleLightInteractChange");
        isFlippedOn = powerOn;

        if (isFlippedOn)
        {
            //GetComponent<MeshRenderer>().material.color = Color.green;
            //run SetUpLightControlSteering
            LightControl.SetUpLightControlSteering(this);
        }
        else
        {
            //shouldnt need an exit since it is handled in the LightControl when keypres

        }

        // TODO: Play an animation.
    }

    public void OnPlayerExit()
    {
        Debug.Log("Exit Light");
        HandleLightInteractChange(false);
    }

    public void OnAssigned()
    {
        //Debug.Log("here");

    }

    public void OnUnassigned()
    {
    }

    public void PerformInteraction()
    {
        lightChannel.RaiseEvent(!isFlippedOn);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetName()
    {
        return "Light Control Interactable";
    }

    public string GetDescription()
    {
        return "turn " + (isFlippedOn ? "off " : "on ");
    }
}
