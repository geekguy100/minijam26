using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIComponent : MonoBehaviour
{
    public bool StartGameTimer;

    //for now we can use a simple text object attached to the player but we might want to switch to either a clock
    //or some other object easily seen by the player in the enviornment
    public Text Gametime;
    public float StartTime;

    public float timeRemaining;

    bool _timeRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StartGameTimer)
            CountDown();
    }

    void CountDown()
    {
        //
        if(_timeRunning)
        {
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

            }
            else
            {
                timeRemaining = 0;
                _timeRunning = false;

                OnTimerFinish();
            }
        }

        DisplayTime();
    }

    void DisplayTime()
    {

        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        Gametime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerFinish()
    {

        //trigger level completed

    }
}
