using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Interactables/Direction Tile Data")]
public class DirectionTileData : ScriptableObject
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Vector3 initialScale;
    [SerializeField] private Vector3 initialRotation;
    public Sprite ArrowSprite => arrowSprite;
    public Vector3 InitialScale => initialScale;
    public Vector3 InitialRotation => initialRotation;
}