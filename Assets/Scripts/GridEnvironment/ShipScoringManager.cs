using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(GridField))]
public class ShipScoringManager : MonoBehaviour
{

    GridField grid;
    
    private void Awake()
    {
        grid = GetComponent<GridField>();
        //grid.onPopulationComplete.AddListener(ProcessScoreGridElements);
    }

}
