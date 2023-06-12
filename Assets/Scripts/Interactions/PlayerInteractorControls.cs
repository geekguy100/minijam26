
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction/Player Controls")]
public class PlayerInteractorControls : ScriptableObject
{
    public KeyCode InteractionKey => interactionKey;
    public KeyCode InteractionAltKey => altInteractionKey;
    public KeyCode ExitLightKey => exitLightKey;


    [SerializeField] private KeyCode interactionKey = KeyCode.E;
    [SerializeField] private KeyCode altInteractionKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode exitLightKey;
}