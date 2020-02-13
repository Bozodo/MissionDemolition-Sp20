using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //static point of interest

    [Header("Set Dynamically")]
    public float camZ; //The desired Z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null) return; //return if there is no POI

        //Get the position of th POI
        Vector3 destination = POI.transform.position;

        //Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;

        //Set the camera to the destination
        transform.position = destination;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
