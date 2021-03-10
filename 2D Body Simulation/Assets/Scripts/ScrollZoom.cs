using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{

    float scrollSpeed = -1000f;
    float cameraDst = 9f;
    public float maxCameraDst = 20f;
    public float minCameraDst = 3f;

    void Update()
    {
        cameraDst += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        cameraDst = Mathf.Clamp(cameraDst, minCameraDst, maxCameraDst);
        Camera.main.orthographicSize = cameraDst;
    }
}
