using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuComponent : MonoBehaviour
{
    [SerializeField] private PlayerCoreControls controls;

    bool menuToggle = false;

    public GameObject StopwatchUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(controls.TabMenuKey))
        {
            menuToggle = !menuToggle;


        }

        StopwatchUI.SetActive(menuToggle);
        
    }
}
