using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; //static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; //The desired Z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        //if there is no POI return P:[0, 0, 0]
        if (POI == null) {
            destination = Vector3.zero;
        }
        else
        {
            //Get the position of the POI
            destination = POI.transform.position;

            //If POI is a projectile, check to see if it's at rest
            if(POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        //Get the position of the POI
        

        //Limit the X & Y to minimum vales
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        //Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;

        //Set the camera to the destination
        transform.position = destination;

        //Set the orthographicSize of the Camera to keep Ground in view
        //10 is the minimum size regardless of destination's y position
        Camera.main.orthographicSize = destination.y + 10;
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
