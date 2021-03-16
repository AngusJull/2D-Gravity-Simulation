using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    private BodyCreator creator;
    private List<StellarBody> BodyList;
    private bool paused = false;
    public int timescale = 1;

    void Start()
    {
        creator = GetComponent<BodyCreator>();
        BodyList = creator.GetBodies();

        GameEvents.eventManager.Pause += GamePaused;
        GameEvents.eventManager.Play += GameUnpaused;
    } 
    void Update()
    {
        if (!paused)
        {
            updateVelocities();
            updatePositions();
        }
    }
    private void GamePaused(object sender, EventArgs e)
    {
        paused = true;
        BodyList = creator.GetBodies();
    }
    private void GameUnpaused(object sender, EventArgs e)
    {
        paused = false;
        BodyList = creator.GetBodies();
    }
    private void updateVelocities()
    {
        foreach (StellarBody current in BodyList)
        {
            //Timescale changes how quickly the velocity will change.
            current.velocity += (current.CalculateGravForces(true) / current.mass) * Time.deltaTime * timescale;
        }
        return;
    }
    private void updatePositions()
    {
        foreach (StellarBody current in BodyList)
        {
            Vector3 v = current.velocity;
            current.transform.position += v * Time.deltaTime * timescale;
        }
        return;
    }
    public void SetTimescale(int _timescale)
    {
        timescale = _timescale;
    }
}
