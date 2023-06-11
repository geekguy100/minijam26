using UnityEngine;

[RequireComponent(typeof(Interactor))]
public class TriggerInteractor : MonoBehaviour
{
    private Interactor interactor;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitGameObject = other.gameObject;

        if (!hitGameObject.TryGetComponent(out IInteractable interactable))
            return;

        interactor.AssignInteractable(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject hitGameObject = other.gameObject;

        if (interactor.MatchesInteractable(hitGameObject))
        {
            interactor.UnassignInteractable();
        }
    }
}