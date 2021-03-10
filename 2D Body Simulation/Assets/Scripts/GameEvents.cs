//Created by Angus Jull on 8/5/2020
//This script sends game events, triggered by other parts of the game. Currently only implements game pausing
using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //Singleton reference
    [HideInInspector]
    public static GameEvents eventManager;
    //The Events which will be subscribed to. You could replace eventhandler with Action instead, however I kept eventhandler just in case
    public event EventHandler Pause;
    public event EventHandler Play;

    void Awake()
    {
        eventManager = this;
    }
    public void OnPause(EventArgs e)
    {
        EventHandler handler = Pause;
        handler?.Invoke(this, e);
    }
    public void OnPlay(EventArgs e)
    {
        EventHandler handler = Play;
        handler?.Invoke(this, e);
    }
}
