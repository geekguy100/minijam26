
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Trackable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private TrackableData data;
    [SerializeField] private Channel triggerPulseChannel;

    #region Event Subbing
    private void OnEnable()
    {
        triggerPulseChannel.OnEventRaised += Pulse;
    }

    private void OnDisable()
    {
        triggerPulseChannel.OnEventRaised -= Pulse;
    }
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetAlpha(0f);
    }

    private void Pulse()
    {
        LeanTween.value(gameObject, SetAlpha, 1f, 0f, data.displayTime).setEase(data.easingType);
    }

    private void SetAlpha(float alpha)
    {
        Color c = spriteRenderer.color;
        c.a = alpha;

        spriteRenderer.color = c;
    }
}