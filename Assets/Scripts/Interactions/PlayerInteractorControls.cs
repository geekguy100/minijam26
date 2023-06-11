
using UnityEngine;

[CreateAssetMenu(menuName = "Interaction/Player Controls")]
public class PlayerInteractorControls : ScriptableObject
{
    public KeyCode InteractionKey => interactionKey;
    public KeyCode ExitLightKey => exitLightKey;


    [SerializeField] private KeyCode interactionKey;
    [SerializeField] private KeyCode exitLightKey;
}