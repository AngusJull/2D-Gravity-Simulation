//Created by Angus Jull on 8/9/2020, used to change the velocity of a stellar body. 
//Points at it's parent and stores the distance and vector between them
using System.Collections.Generic;
using UnityEngine;

public class VelocityChanger : MonoBehaviour
{
    //Used by other classes
    public bool lastChosen = false;
    //This is the vector between the parent and the velocity changer
    private Vector3 velocityVec = new Vector3();
    private LineRenderer line;
    private DragAndDrop dandd;
    private Vector3[] linePoints;
    //The square magnitude of the velocityVec
    private float sqrDistance = 0;

    void Awake()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        transform.localScale = new Vector3(0.3f, 0.3f, 0f);
        transform.parent = parent;
        transform.localPosition = new Vector3(0f, 0f, -0.5f);

        dandd = GetComponent<DragAndDrop>();
        line = GetComponent<LineRenderer>();
        linePoints = new Vector3[2];
    }
    void Update()
    {
        //Sets the start and end position of the line
        linePoints[0] = transform.position;
        //The position minus the local position should give the world coordinates of the parent
        linePoints[1] = transform.parent.position;
        line.SetPositions(linePoints);
        //Updating the last chosen variable
        lastChosen = dandd.lastChosen;
    }
    void FixedUpdate()
    {
        //Vector points from this obj to parent
        velocityVec = transform.position - transform.parent.position;
        velocityVec.z = 0;
        //Sets the rotation of the changer to be in the same direction as the vector
        transform.right = velocityVec.normalized;
        sqrDistance = velocityVec.sqrMagnitude;
    }
    //Gets the square distance between this obj and the parent
    public float GetSqrDistance()
    {
        return sqrDistance;
    }
    public float GetDistance()
    {
        return velocityVec.magnitude;
    }
    public Vector3 GetVelocityVec()
    {
        return velocityVec;
    }
}
