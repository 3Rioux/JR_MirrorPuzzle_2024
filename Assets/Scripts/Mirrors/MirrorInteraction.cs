using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{

    [Tooltip("Mirror follows this local EmptyObject in the player prefab")]
    public Transform PlayerHandPosition;

    [Tooltip("SocketObject with collider(shpere, box etc.) (is trigger = true)")]
    //public Collider Socket; // need Trigger
    private Collider Socket; // need Trigger

    /**
     * access the line renderer in child of mirrorBody
     * On after collision with socket 
     * OFF by default 
     */
    private LineRenderer thisLineRenderer;

    //public AN_DoorScript DoorObject;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    bool follow = false, isConnected = false, followFlag = false, youCan = true;
    
    /**
     * Mirror Game object Components 
     */
    Rigidbody rb;
    BoxCollider boxCol;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        thisLineRenderer = GetComponentInChildren<LineRenderer>();
        LineRenderer.enable
    }

    void Update()
    {
        if (youCan) Interaction();

        // frozen if it is connected to PowerOut
        if (isConnected)
        {
            gameObject.transform.position = new Vector3(Socket.transform.position.x, Socket.transform.position.y + 0.25f, Socket.transform.position.z);
            
            /**
             * Keep the game object original rotation 
             */
            //gameObject.transform.rotation = Socket.transform.rotation;
            //DoorObject.isOpened = true;
        }
        else
        {
            //DoorObject.isOpened = false;
        }
    }

    void Interaction()
    {

        /**
         * Pick up the mirror game object 
         */
        if (NearView() && Input.GetKeyDown(KeyCode.E) && !follow)
        {
            isConnected = false; // unfrozen
            
            follow = true;
            followFlag = false;
        }

        /**
         * Rotate the mirror game object WHEN FOLLOWING!!!
         */
        if (NearView() && Input.GetKeyDown(KeyCode.R) && follow)
        {
            //rotate the cube by 90 each press of the R button 
            gameObject.transform.rotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation
        }

        if (follow)
        {
            boxCol.enabled = false;
            rb.drag = 10f;
            rb.angularDrag = 10f;
            if (followFlag)
            {
                distance = Vector3.Distance(transform.position, Camera.main.transform.position);
                if (distance > 5f || Input.GetKeyDown(KeyCode.E))
                {
                    follow = false;
                }
            }

            followFlag = true;
            //rb.AddExplosionForce(-1000f, PlayerHandPosition.position, 10f);
            // second variant of following
            //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, objectLerp.position, 1f);
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, PlayerHandPosition.position, 1f);
        }
        else
        {
            boxCol.enabled = true;
            rb.drag = 0f;
            rb.angularDrag = .5f;
        }
    }

    bool NearView() // it is true if you near interactive object
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        direction = transform.position - Camera.main.transform.position;
        angleView = Vector3.Angle(Camera.main.transform.forward, direction);
        //if (distance < 5f && angleView < 35f) return true;
        if (distance < 6f) return true;
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Socket")
        {
            //set the socket on collision 
            Socket = other.gameObject.GetComponent<Collider>();



            isConnected = true;
            follow = false;
            //DoorObject.rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));


            //******************************************************************************************************ADD Lazer Activation Code HERE

            //******************************************************************************************************ADD Lazer Activation Code HERE

        }
        //if (OneTime) youCan = false;
    }
}
