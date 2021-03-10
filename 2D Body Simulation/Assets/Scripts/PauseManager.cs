//Created by Angus Jull, Creates a system for pausing and unpausing a game. Mostly relies on the event system in C#.
//Scripts should individually subscribe to the event.

using System;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    //Private properties
    private bool gamePaused = true;
    private float elapsedTime;
    private Image image;
    private Color currentColor;

    //Public Properties
    public float fadeWaitTime;
    public Sprite pauseSprite;
    public Sprite playSprite;

    //Event system
    void Awake()
    {
        //Getting the image component
        image = gameObject.GetComponent<Image>();
        //Sets the image to start transparent
        currentColor = new Color(1.0f, 1.0f, 1.0f, 0f);
        image.color = currentColor;

        GameEvents.eventManager.Pause += GamePaused;
        GameEvents.eventManager.Play += GameUnpaused;
    }

    void Update()
    {
        //Checking if space was pressed
        if (Input.GetKeyDown("space"))
        {
            if (gamePaused)
            {
                GameEvents.eventManager.OnPlay(new EventArgs());
            }
            else
            {
                GameEvents.eventManager.OnPause(new EventArgs());
            }
        }
        //If the image is already transparent no changes are needed
        if (currentColor.a != 0f)
        {
            //Checking if the wait period has expired
            if (elapsedTime > fadeWaitTime)
            {
                currentColor.a -= 0.5f * Time.deltaTime;
                if (currentColor.a < 0)
                {
                    currentColor.a = 0;
                }
            }
            else
            {
                elapsedTime += Time.deltaTime;
            }
        }
        image.color = currentColor;
    }
    private void GameUnpaused(object sender, EventArgs e)
    {
        gamePaused = false;
        image.sprite = playSprite;
        currentColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        elapsedTime = 0;
    }
    private void GamePaused(object sender, EventArgs e)
    {
        gamePaused = true;
        image.sprite = pauseSprite;
        currentColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        elapsedTime = 0;
    }
}
