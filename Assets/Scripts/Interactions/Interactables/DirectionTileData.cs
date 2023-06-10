using UnityEngine;

[CreateAssetMenu(menuName = "Interactables/Direction Tile Data")]
public class DirectionTileData : ScriptableObject
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Vector3 initialScale;
    [SerializeField] private float initialAngle;
    public Sprite ArrowSprite => arrowSprite;
    public Vector3 InitialScale => initialScale;
    public float InitialAngle => initialAngle;
}