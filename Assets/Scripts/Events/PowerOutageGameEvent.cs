using UnityEngine;

public class PowerOutageGameEvent : GameEvent
{
    [SerializeField] private BoolChannel powerChannel;
    
    public override void InvokeEvent()
    {
        powerChannel.RaiseEvent(false);
    }
}