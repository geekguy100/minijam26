using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles invoking in-game events depending on certain conditions.
/// </summary>
public class EventManager : MonoBehaviour
{
    [SerializeField] private Channel tickChannel;
    [SerializeField] private float secondsPerTick;
    
    private void Start()
    {
        StartCoroutine(HandleTick());
    }

    private IEnumerator HandleTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsPerTick);
            tickChannel.RaiseEvent();
        }
    }
}