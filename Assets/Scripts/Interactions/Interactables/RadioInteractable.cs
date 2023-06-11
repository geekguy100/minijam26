using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private BoolChannel RadioChannel;

    AudioSource AudioSource;

    public List<AudioClip> Playlist;
    private int currentSong = 0;

    public AudioClip RadioStaticChange;
    //will handle music
    //interacting will play a radio static noise then play the next song in the list

    #region Event Subbing
    private void OnEnable()
    {
        RadioChannel.OnEventRaised += HandleRadioInteractChange;

    }

    private void OnDisable()
    {
        RadioChannel.OnEventRaised += HandleRadioInteractChange;

    }
    #endregion

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = Playlist[currentSong];
        if(Playlist[currentSong] != null)
            AudioSource.Play();
    }

    public void OnAssigned()
    {


    }

    public void OnUnassigned()
    {

    }

    private void HandleRadioInteractChange(bool powerOn)
    {
        //AudioSource.Stop();
        AudioSource.clip = RadioStaticChange;
        AudioSource.Play();

        // TODO: Play an sound byte to identify changing of the song

        PlayNextSong();
    }


    void PlayNextSong()
    {
        if(currentSong < Playlist.Count -1)
        {
            AudioSource.clip = Playlist[currentSong++];
            AudioSource.Play();
            Debug.Log("Now Playing " + GetDescription());
        }
        else
        {
            currentSong = 0;
            AudioSource.clip = Playlist[currentSong];
            Debug.Log("Now Playing Nothing...");
        }
        
    }

    public void PerformInteraction()
    {
        RadioChannel.RaiseEvent(false);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetName()
    {
        return "Radio Control Interactable";
    }

    public string GetDescription()
    {
        return "Current song: " + Playlist[currentSong].name;
    }
}
