//Created by Angus Jull on 10/24/2020, checks if there is a tracked stellar body and then moves the camera to it using slerp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracking : MonoBehaviour
{
    public float trackingSpeed = 30f;
    private bool paused = true;
    void Start()
    {
        GameEvents.eventManager.Pause += GamePaused;
        GameEvents.eventManager.Play += GameUnpaused;
    }
    void Update()
    {
        if (StellarBody.tracking)
        {
            if (StellarBody.trackedBody != null)
            {
                //This is nessecary to stop the camera from moving inside the plane of the tracked object, and moving the orthographic view end up behind it
                Vector3 trackedObjectPos = StellarBody.trackedBody.transform.position;
                trackedObjectPos.z = transform.position.z;
                if (paused)
                {
                    transform.position = Vector3.Slerp(transform.position, trackedObjectPos, trackingSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = trackedObjectPos;
                }
            }
        }
    }
    private void GamePaused(object sender, EventArgs e)
    {
        paused = true;
    }
    private void GameUnpaused(object sender, EventArgs e)
    {
        paused = false;
    }
}
