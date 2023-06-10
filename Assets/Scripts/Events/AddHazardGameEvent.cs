using UnityEngine;

public class AddHazardGameEvent : GameEvent
{
    [SerializeField] private GameObject hazardPrefab;
    [SerializeField] private Transform hazardSpawnPos;
    
    public override void InvokeEvent()
    {
        throw new System.NotImplementedException();
    }
}