//Created by Angus Jull on 8/6/2020, creates new stellar bodies when requested by the player (done using a button)
using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyCreator : MonoBehaviour
{
    private float size = 1;
    public StellarBody BodyPrefab;
    //A list of the stellar bodies this script has initialized
    private List<StellarBody> BodyList = new List<StellarBody>();
    public List<StellarBody> GetBodies() { return BodyList; }

    public void AdjustSize(float _size)
    {
        size = _size;
    }
    //This is called by a button, will pause the game
    public void BodyRequested()
    { 
        StellarBody current = Instantiate(BodyPrefab, transform);
        current.SetProperties(size * size,  (size / 2));
        //Stops objects from being spawned perfectly inside eachother
        Vector3 randomOffset = UnityEngine.Random.insideUnitCircle;
        //When the camera is tracking an object, this ensures the object is created in the center of the screen
        randomOffset += Camera.main.transform.position;
        //Since the camera is not in the plane of the game, this moves the object back into the plane of the other objects.
        randomOffset.z = 0f;
        current.transform.position = randomOffset;
        //Adds this object to the list of objects so that they can update their lists of objects 
        BodyList.Add(current);

        GameEvents.eventManager.OnPause(new EventArgs());
    }
    
    
}
