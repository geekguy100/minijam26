using System;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(MMScaleShaker), typeof(ShipRotationBehaviour))]
public class DirectionTile : MonoBehaviour, IInteractable
{
    private bool initialized;

    private ShipRotationBehaviour rotationBehaviour;
    private Transform directionIndication;
    private SpriteRenderer spriteRenderer;
    private MMScaleShaker shaker;

    [SerializeField] private MMF_Player feedbackPlayer;
    
    [SerializeField] private DirectionTileData data;

    #region Initialization
    private void Awake()
    {
        rotationBehaviour = GetComponent<ShipRotationBehaviour>();
        shaker = GetComponent<MMScaleShaker>();
    }
    
    private void Start()
    {
        CreateIndicationChild();
        
        spriteRenderer = directionIndication.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.ArrowSprite;

        spriteRenderer.enabled = false;
        
        shaker.Channel = -1;
    }

    private void CreateIndicationChild()
    {
        directionIndication = new GameObject("Direction Tile Indication").transform;
        directionIndication.gameObject.layer = 9;
        directionIndication.SetParent(transform);
        directionIndication.localPosition = Vector3.zero;
        directionIndication.localScale = data.InitialScale;
        directionIndication.localEulerAngles = data.InitialRotation;
    }
    #endregion

    #region Assignment
    public void OnAssigned()
    {
        shaker.Channel = 0;
        
        if (!ReferenceEquals(feedbackPlayer, null))
            feedbackPlayer.PlayFeedbacks();
    }
    
    public void OnUnassigned()
    {
        shaker.Channel = -1;
    }
    #endregion

    #region Misc. Methods
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetName()
    {
        return "Direction Tile";
    }

    public string GetDescription()
    {
        return "Rotate";
    }
    #endregion

    public void PerformInteraction()
    {
        spriteRenderer.enabled = true;
        rotationBehaviour.initialized = true;
        
        transform.Rotate(Vector3.up, 90f);
    }
}