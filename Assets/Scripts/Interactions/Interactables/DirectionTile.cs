using System;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(MMScaleShaker))]
public class DirectionTile : MonoBehaviour, IInteractable
{
    private Transform directionIndication;
    private SpriteRenderer spriteRenderer;
    private MMScaleShaker shaker;

    [SerializeField] private MMF_Player feedbackPlayer;
    
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private DirectionTileData data;

    #region Initialization
    private void Awake()
    {
        shaker = GetComponent<MMScaleShaker>();
    }
    
    private void Start()
    {
        CreateIndicationChild();
        
        spriteRenderer = directionIndication.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.ArrowSprite;

        meshRenderer.enabled = true;
        spriteRenderer.enabled = false;
        
        shaker.Channel = -1;
    }

    private void CreateIndicationChild()
    {
        directionIndication = new GameObject("Direction Tile Indication").transform;
        directionIndication.SetParent(transform);
        directionIndication.localPosition = Vector3.zero;
        directionIndication.localScale = data.InitialScale;
        directionIndication.localEulerAngles = data.InitialRotation;
    }
    #endregion

    #region Assignment
    public void OnAssigned()
    {
        // meshRenderer.enabled = true;
        spriteRenderer.enabled = true;

        shaker.Channel = 0;
        
        if (!ReferenceEquals(feedbackPlayer, null))
            feedbackPlayer.PlayFeedbacks();
    }
    
    public void OnUnassigned()
    {
        shaker.Channel = -1;

        // meshRenderer.enabled = false;
        spriteRenderer.enabled = false;
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
        transform.Rotate(Vector3.up, 90f);
    }
}