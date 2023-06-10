using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;

public class PlayerUIComponent : MonoBehaviour
{
    //in case we want to have it move slower
    public float TimeScale = 1.0f;

    public bool StartGameTimer;

    public string Clockprintout;

    //for now we can use a simple text object attached to the player but we might want to switch to either a clock
    //or some other object easily seen by the player in the enviornment
    public TMP_Text Gametime;
    public float StartTime;

    float _timeRemaining;

    bool _timeRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        _timeRemaining = StartTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(StartGameTimer)
            CountDown();
    }

    void CountDown()
    {
        // bool can be triggered once 'shift' has officially started
        if(_timeRunning)
        {
            if(_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime * TimeScale;
            }
            else
            {
                _timeRemaining = 0;
                _timeRunning = false;

                OnTimerFinish();
            }
        }

        DisplayTime();
    }

    void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(_timeRemaining / 60);
        float seconds = Mathf.FloorToInt(_timeRemaining % 60);

        
        Clockprintout = string.Format("{0:00}:{1:00}", minutes, seconds);
        Gametime.text = Clockprintout;
        //Debug.Log(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    void OnTimerFinish()
    {

        //trigger level completed

    }
}
