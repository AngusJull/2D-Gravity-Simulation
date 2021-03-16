//Created by Angus Jull on 10/31/2020, manages the custom trail of an object.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrail : MonoBehaviour
{
    private TrailRenderer trail;
    private StellarBody body;
    private Gradient gradient = new Gradient();
    private GradientColorKey[] colourKey = new GradientColorKey[2];
    private GradientAlphaKey[] alphaKey =  new GradientAlphaKey[2];

    void Awake()
    {
        GameEvents.eventManager.Pause += GamePaused;
        GameEvents.eventManager.Play += GameUnpaused;

        body = GetComponent<StellarBody>();
        trail = GetComponent<TrailRenderer>();
    }
    private void GamePaused(object sender, EventArgs e)
    {
        trail.emitting = false;
    }
    private void GameUnpaused(object sender, EventArgs e)
    {
        UpdateColourKeys();
        UpdateAlphaKeys();
        gradient.colorKeys = colourKey;
        gradient.alphaKeys = alphaKey;
        trail.colorGradient = gradient;
        trail.emitting = true;
    }
    private void UpdateColourKeys()
    {
        colourKey[0].color = body.Colour;
        colourKey[0].time = 0;
        colourKey[1].color = body.Colour;
        colourKey[1].time = 1;
    }
    private void UpdateAlphaKeys()
    {
        alphaKey[0].alpha = 0.6f;
        alphaKey[0].time = 0;
        alphaKey[1].alpha = 0.3f;
        alphaKey[1].time = 0.77f;
    }
}
