using UnityEngine;

[RequireComponent(typeof(GameEvent))]
public abstract class GameEventInfluencer : MonoBehaviour
{
    private GameEvent gameEvent;

    protected virtual void Awake()
    {
        gameEvent = GetComponent<GameEvent>();
    }

    protected void InvokeEvent()
    {
        gameEvent.InvokeEvent();
    }
    
    protected abstract void Influence();
}