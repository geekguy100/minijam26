using UnityEngine;

public class GameTickInfluencer : GameEventInfluencer
{
    [Header("Read Only")]
    [SerializeField] private float likeliness;
    
    [Header("Fields")]
    [SerializeField][Min(0)] private float likelinessPerTick;
    [SerializeField] private float requiredLikeliness;
    [SerializeField] private Channel tickChannel;

    #region Event Subbing
    private void OnEnable()
    {
        tickChannel.OnEventRaised += Influence;
    }

    private void OnDisable()
    {
        tickChannel.OnEventRaised -= Influence;
    }
    #endregion

    protected override void Influence()
    {
        likeliness += likelinessPerTick;

        if (likeliness >= requiredLikeliness)
        {
            InvokeEvent();
            likeliness = 0f;
        }
    }
}