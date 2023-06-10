
using UnityEngine;

public class RadarBehaviour : MonoBehaviour
{
    private float timeSinceLastPulse;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float timeBetweenPulses;
    [SerializeField] private Channel triggerPulseChannel;
    
    private void PerformPulse()
    {
        audioSource.Play();
        triggerPulseChannel.RaiseEvent();
    }

    private void Update()
    {
        timeSinceLastPulse += Time.deltaTime;

        if (timeSinceLastPulse >= timeBetweenPulses)
        {
            timeSinceLastPulse = 0f;
            PerformPulse();
        }
    }
}