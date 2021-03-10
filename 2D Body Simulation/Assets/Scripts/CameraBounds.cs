//Unity script by Angus Jull, Created on November 5th 2019
//Created for my boids project, original code should be in the boids project
//Creates two vector2s that store the coordinates at the edges of an orthographic camera. 

using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    #region Variables
    //These store the coordinates that the corners of the orthographic camera lies at.
    //mimimums is in the bottom left, maximums is the top right of the camera's view
    //Refer to the capitalized variables to get the information
    private Vector2 minimums = new Vector2();
    public Vector2 Minimums { get { return minimums; } }
    private Vector2 maximums = new Vector2();
    public Vector2 Maximums { get { return maximums; } }
    #endregion
    
    #region Unity Methods
    //Sets the two Vector2s
    void Awake()
    {
        float vertExtent = GetComponent<Camera>().orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        minimums.Set(-1 * horzExtent, -1 * vertExtent);
        maximums.Set(horzExtent, vertExtent);
    }
    void PrintExtents()
    {
        Debug.Log(minimums.ToString());
        Debug.Log(maximums.ToString());
    }
    #endregion
}
