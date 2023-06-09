
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Trackable : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform originalParent;

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
        originalParent = transform.parent;
    }

    private void Start()
    {
        SetAlpha(0f);
    }

    private void Pulse()
    {
        transform.SetParent(null);
        LeanTween.value(gameObject, SetAlpha, 1f, 0f, data.displayTime).
            setEase(data.easingType).
            setOnComplete(() =>
            {
                transform.SetParent(originalParent);
                transform.localPosition = Vector3.zero;
            });
    }

    private void SetAlpha(float alpha)
    {
        Color c = spriteRenderer.color;
        c.a = alpha;

        spriteRenderer.color = c;
    }
}