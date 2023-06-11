using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEnviornment : MonoBehaviour
{

    //manages what goes on each tile
    public bool Hazard;

    //hazard will be stored here (passed from ship?)
    public GameObject HazEnv;

    public Transform HazSpawnOrigin;

    public void SetNewHazard(GameObject newHaz)
    {
        if (!newHaz)
        {
            Debug.LogError("Ship script does not contain its crashed ship env asset. Please update value crashAsset");
            return;
        }

        Debug.Log("Set hazard");

        //spawn in asset
        HazEnv = Instantiate(newHaz, HazSpawnOrigin.position, HazSpawnOrigin.rotation);



        //mark this tile as hazard
        Hazard = true;
        //note: ship fail event is going to trigger in Ship script after
    }
}
