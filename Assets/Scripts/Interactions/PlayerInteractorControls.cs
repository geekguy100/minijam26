
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction/Player Controls")]
public class PlayerInteractorControls : ScriptableObject
{
    public KeyCode InteractionKey => interactionKey;
    
    [SerializeField] private KeyCode interactionKey;
}