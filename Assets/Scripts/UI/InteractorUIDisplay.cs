
using TMPro;
using UnityEngine;

public class InteractorUIDisplay : MonoBehaviour
{
    [SerializeField] private PlayerInteractorControls interactorControls;
    [SerializeField] private TextMeshProUGUI displayField;

    private void Start()
    {
        displayField.gameObject.SetActive(false);
    }

    public void Display(IInteractable interactable)
    {
        string description = interactable.GetDescription();
        
        displayField.gameObject.SetActive(true);
        displayField.text = "Press '" + interactorControls.InteractionKey +
                            "' to " +
                            (string.IsNullOrEmpty(description) ? "interact with " : description) + 
                            "the " + interactable.GetName();
    }

    public void Hide()
    {
        displayField.gameObject.SetActive(false);
    }
}
