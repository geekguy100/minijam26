using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    /*
     * To do:
     * - Player can interact
     * - will inherit from interactable
     * - will need have info accessible for map to read from for proper orientation sync
     * - can either spin left or right
     * 
     * Questions/Things to consider:
     * - how do we want the player to interact?
     *      - player should be able to turn one way or another and must go below decks to see if they see anything new
     *      - should rotate via mouse input
     * 
     * - how should we animation / show the rotation
     */

    //
    public float xrotation;

    //whatever direction we are currently facing, that should be the only visible area outside
    //will have the fog of war be a seperate component
    //will read from this orientation to determine rotion of our cone for visibility
    public float currentOrientation;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RotateLeft()
    {

    }

    void RotateRight()
    {

    }

    void MakeRotation()
    {

    }

}
