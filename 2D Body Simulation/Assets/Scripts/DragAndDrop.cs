//Created by Angus Jull on 8/8/2020, allows sprites with a COLLIDER2D (REQUIRED!!) to be dragged and dropped around the screen
//An event system is implemented with pausing, but can be easily removed. Just get rid of everything relating to GameEvents.
//Should be relatively loosely coupled.
using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool dragging = false;
    public bool lastChosen = false;
    private bool paused = true;
    //Stores the offset the object has from the mouse when clicked so it keeps that offset.
    private Vector3 dragOffset = new Vector3();
    private Collider2D _collider;


    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        GameEvents.eventManager.Pause += GamePaused;
        GameEvents.eventManager.Play += GameUnpaused;
    }
    void Update()
    {
        if (paused)
        {
            //Getting the mouse position in world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Setting the mouse position's z component to 0 prevents the mouse from pulling the sprites behindt the camera. 
            mousePos.z = 0;
            //Checks if the mouse has just been pressed (LMB)
            if (Input.GetMouseButtonDown(0))
            {
                //Checks if this object's collider is under the mouse
                if (_collider == Physics2D.OverlapPoint(mousePos))
                {
                    dragging = true;
                    lastChosen = true;
                    dragOffset = transform.position - mousePos;
                }
            }
            if (dragging)
            {
                Vector3 pos = mousePos + dragOffset;
                pos.z = transform.position.z;
                transform.position = pos;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!dragging)
                {
                    lastChosen = false;
                }
                else
                {
                    dragging = false;
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
        dragging = false;
        lastChosen = false;
    }
}
