using UnityEngine;

public class DirectionTile : MonoBehaviour, IInteractable
{
    private Transform directionIndication;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private DirectionTileData data;

    #region Initialization
    private void Start()
    {
        directionIndication = new GameObject("Direction Tile Indication").transform;
        directionIndication.localScale = data.InitialScale;
        directionIndication.localRotation = Quaternion.AngleAxis(data.InitialAngle, Vector3.up);
        
        spriteRenderer = directionIndication.gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.ArrowSprite;
        
        spriteRenderer.enabled = false;
    }
    #endregion

    #region Assignment
    public void OnAssigned()
    {
        spriteRenderer.enabled = true;
    }
    
    public void OnUnassigned()
    {
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
    
    public Vector3 GetDirection()
    {
        return directionIndication.forward;
    }
}