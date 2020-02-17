using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    //attributes help organize components in the Unity Inspector pane

    //fields set in Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    //fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidBody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }


    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    private void OnMouseEnter()
    {
        print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    private void OnMouseExit()
    {
        print("Slingshot: OnMouseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //Mouse button is pressed down
        aimingMode = true;

        //Instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;

        //Start it at the launchPoint
        projectile.transform.position = launchPos;

        //Temporarily set projectile to isKinematic so we can move it around
        //projectile.GetComponent<Rigidbody>().isKinematic = true;

        projectileRigidBody = projectile.GetComponent<Rigidbody>();
        projectileRigidBody.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode)
        {
            return;
        }

        //Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;

        //This informs how far to push mousePos3D in the 3D space using the camera
        mousePos2D.z = -Camera.main.transform.position.z;

        //Sets the mouse's position in 3D space
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        //Limit mouseDealta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move the projectile to this new position
        //projPos is the result of the launchPos (which is the starting position) plus the difference of where the mouse is
        Vector3 projPos = launchPos + mouseDelta;

        //This is the part that actually moves it by setting the projectile's position
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            //The primary mouse button has been released
            aimingMode = false;
            projectileRigidBody.isKinematic = false;
            projectileRigidBody.velocity = -mouseDelta * velocityMult;

            //Sets projectile as the POI we want our camera to follow
            FollowCam.POI = projectile;

            //Empties the projectile field so we can fire another
            projectile = null;
            MissionDemolition.ShotsFired();
            ProjectileLine.S.poi = projectile;
        }

    }
}
