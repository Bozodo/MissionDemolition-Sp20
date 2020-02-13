using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    //attributes help organize components in the Unity Inspector pane

    //fields set in Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;

    //fields set dynamically
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;


    private void Awake()
    {
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
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
