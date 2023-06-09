using UnityEngine;

public abstract class Channel<T> : Channel
{
    public new event System.Action<T> OnEventRaised;

    public void RaiseEvent(T arg)
    {
        base.RaiseEvent();
        this.OnEventRaised?.Invoke(arg);
    }
}

[CreateAssetMenu(menuName = "Channels/Void Channel")]
public class Channel : ScriptableObject
{
    public event System.Action OnEventRaised;
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}