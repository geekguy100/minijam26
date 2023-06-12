
using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class PlayerInteractorController : MonoBehaviour
{
    private Interactor interactor;
    [SerializeField] private PlayerInteractorControls controls;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(controls.InteractionKey) || Input.GetKeyDown(controls.InteractionAltKey))
        {
            interactor.Interact();
        }
    }
}
