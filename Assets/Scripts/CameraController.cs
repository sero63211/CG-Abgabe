using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    public Transform target;

    private float startFOV, targetFOV;
    public float zoomSpeed=1f;
    public Camera thecam;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        startFOV = thecam.fieldOfView;
        targetFOV = startFOV;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //this is the code that makes the camera follow the player
        transform.position = target.position;
        transform.rotation = target.rotation;

        thecam.fieldOfView = Mathf.Lerp(thecam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
    // Zoom in
    public void ZoomIn(float newZoom)
    {
        targetFOV = newZoom;
    }
    //Zoom out
    public void ZoomOut()
    {
        targetFOV = startFOV;
    }
}
