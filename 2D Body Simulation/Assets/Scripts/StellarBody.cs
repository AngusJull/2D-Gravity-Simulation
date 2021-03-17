//Created by Angus Jull in August 2020. Makes the componenets nessecary to create a simulation of gravity work together. 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarBody : MonoBehaviour
{
    static public readonly float gravConstant = 6f;
    //This is used by the tracking script on the main camera
    static public StellarBody trackedBody = null;
    static public bool tracking = false;

    //Private Properties
    private SpriteRenderer sprite;
    //This object's velocityChanger
    private VelocityChanger velocityChanger;
    private DragAndDrop dragAndDrop;
    private Transform selectionCircle;
    private SpriteRenderer selectionCircleSprite;
    private List<StellarBody> BodyList;
    private float sizeScale = 0.5f;
    private bool paused = true;

    //Public properties
    public Vector2 velocity;
    public bool pinned = false;
    public VelocityChanger velChangerPrefab;
    public float mass = 1;
    public float size = 1;

    public Color Colour { get { return _colour; } }
    private Color _colour;
    void Awake()
    {
        //Setting componenets
        sprite = GetComponent<SpriteRenderer>();
        dragAndDrop = GetComponent<DragAndDrop>();
        selectionCircle = transform.GetChild(0);
        selectionCircleSprite = selectionCircle.GetComponent<SpriteRenderer>();
        velocityChanger = null;
        //Giving this object a random color
        SetRandomColour();
        //Setting up events
        GameEvents.eventManager.Pause += GamePaused;
        GameEvents.eventManager.Play += GameUnpaused;
    }
    private void Update()
    {
        if (paused)
        {
            //Checks if this object or its children are the last chosen objects
            if (FamilyLastChosen())
            {
                selectionCircleSprite.enabled = true;
                //Checks if this already has a velocityChanger child
                if (velocityChanger == null)
                {
                    velocityChanger = Instantiate(velChangerPrefab, transform);
                    velocityChanger.transform.rotation = transform.rotation;
                    Vector3 velocityOffset = velocity;
                    velocityOffset.z = velocityChanger.transform.localPosition.z;
                    velocityChanger.transform.position = transform.position + velocityOffset;
                }
                //Checks if the user has chosen to try and track or untrack an object
                if (Input.GetKeyDown("c"))
                {
                    //If c is pressed and this object was already being tracked it will deselect itself
                    if (trackedBody != this)
                    {
                        tracking = true;
                        trackedBody = this;
                    }
                    else
                    {
                        tracking = false;
                        trackedBody = null;
                    }
                }
            }
            else
            {
                selectionCircleSprite.enabled = false;
                if (velocityChanger)
                {
                    Destroy(velocityChanger.gameObject);
                    velocityChanger = null;
                }
            }
            //Updating the stellar body's velocity based on the velocityChanger
            if (velocityChanger)
            {
                velocity = velocityChanger.GetVelocityVec();
            }
        }
        else
        {
            
            //Checks if the user wants to untrack an object while the simulation is playing
            if (Input.GetKeyDown("c"))
            {
                tracking = false;
                trackedBody = null;
            }
        }
    }

    ///Class specific methods
    
    ///Private Methods

    //Checks if the object or it's velocitychanger were the last chosen objects
    private bool FamilyLastChosen()
    {
        if(dragAndDrop.lastChosen == true)
        {
            return true;
        }
        //If this object was not last chosen, then check if the velocitychanger was
        else if (velocityChanger ? velocityChanger.lastChosen : false)
        {
            return true;
        }
        return false;
    }

    //Called whenever a body is created
    private void GamePaused(object sender, EventArgs e)
    {
        //Making sure this object has a list of all other objects, since when the list is updated the game is automatically paused.
        BodyList = GetComponentInParent<BodyCreator>().GetBodies();
        paused = true;
    }
    private void GameUnpaused(object sender, EventArgs e)
    {
        selectionCircleSprite.enabled = false;
        paused = false;
        if (velocityChanger)
        {
            Destroy(velocityChanger.gameObject);
            velocityChanger = null;
        }
    }
    //Updates the sprite's transform's size based on it's size property and the sizeScale property
    private void UpdateSpriteSize()
    {
        transform.localScale = new Vector3(sizeScale * size, sizeScale * size, sizeScale * size);
    }
    //Updates the selection circle's sprite to match the stellar body's sprite size plus a small amount
    private void UpdateSelectionCircleSize()
    {
        float scale = 1.1f;
        selectionCircle.localScale = new Vector3(scale, scale, scale);
    }
    //Gives the spriterenderer a random hue, with a set saturation and value
    private void SetRandomColour()
    {
        sprite.color = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 0.8f, 1.0f);
        _colour = sprite.color;
    }

    ///Public Methods

    //Calculates the gravitational forces acting on this object, if nbody is true it will take into consideration all other stellar bodies,
    //Otherwise it will find the body which has the greatest effect and only return it's effect
    public Vector2 CalculateGravForces(bool nbody = true)
    {
        if (BodyList.Count == 1)
        {
            return Vector2.zero;
        }

        Vector2 forceVec;
        float force;
        if (nbody)
        {
            Vector2 totalForce = new Vector2();
            for (int i = 0; i < BodyList.Count; i++)
            {
                //Checks if the next body in the list is this one
                if (BodyList[i] == this) { continue; }
                forceVec = BodyList[i].transform.position - transform.position;
                //Since the stellar bodies can pass through one another, this limits how close their masses can get
                if (forceVec.sqrMagnitude < 0.01f) 
                { 
                    forceVec.Normalize(); 
                    forceVec *= 0.1f; 
                }
                //Newtons universal law of gravitation
                force = BodyList[i].mass * mass * gravConstant / forceVec.sqrMagnitude;
                forceVec.Normalize();
                forceVec *= force;
                totalForce += forceVec;
            }
            return totalForce;
        }
        else
        {
            float maxInfluence = 0;
            int index = 0;
            for (int i = 0; i < BodyList.Count; i++)
            {
                if (BodyList[i] == this) { continue; }
                //Calculates the influence of the current body on this object by dividing mass by square distance
                float influence = BodyList[i].mass / (BodyList[i].transform.position - transform.position).magnitude;
                if (influence > maxInfluence)
                {
                    maxInfluence = influence;
                    index = i;
                }
            }
            forceVec = BodyList[index].transform.position - transform.position;
            //Newtons universal law of gravitation
            force = BodyList[index].mass * mass * gravConstant / forceVec.sqrMagnitude;
            forceVec.Normalize();
            forceVec *= force;
            return forceVec;
        }
    }
    //Called by the BodyCreator on instancing this object, sets the mass and size properties as well as the rigidbody's mass property
    public void SetProperties(float _mass, float _size)
    {
        mass = _mass;
        size = _size;
        UpdateSpriteSize();
        UpdateSelectionCircleSize();
    }
}
