using UnityEngine;

/// <summary>
/// Toggles a group of GameObjects.
/// </summary>
public class GameObjectToggler : MonoBehaviour
{
    [SerializeField] private BoolChannel toggleChannel;
    [SerializeField] private GameObject[] objects;

    #region Event Subbing
    private void OnEnable()
    {
        if (!ReferenceEquals(toggleChannel, null))
            toggleChannel.OnEventRaised += Toggle;
    }

    private void OnDisable()
    {
        if (!ReferenceEquals(toggleChannel, null))
            toggleChannel.OnEventRaised -= Toggle;
    }
    #endregion

    public void Toggle(bool enabled)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(enabled);
        }
    }
}
