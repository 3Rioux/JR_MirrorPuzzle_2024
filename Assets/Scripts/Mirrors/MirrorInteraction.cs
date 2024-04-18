using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{

    [Tooltip("Mirror follows this local EmptyObject in the player prefab")]
    public Transform PlayerHandPosition;

    [Tooltip("socketColliderObject with collider(shpere, box etc.) (is trigger = true)")]
    //public Collider socketCollider; // need Trigger
    private SphereCollider socketCollider; // need Trigger
    private GameObject socketGameObject; // need Trigger

    /**
     * access the line renderer in child of mirrorBody
     * On after collision with socketCollider 
     * OFF by default 
     */
    private Laser thisMirrorsLaserScript;

    //public AN_DoorScript DoorObject;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    public bool follow = false;
    bool isConnected = false, followFlag = false, youCan = true;

    /**
     * avariable to determine if the socket has a mirror in place or not
     */
    bool isMirrorEquipped = false;
    
    /**
     * Mirror Game object Components 
     */
    Rigidbody rb;
    BoxCollider boxCol;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();

        /**
         * Access the Laser script and make sure the laser is off 
         */
        thisMirrorsLaserScript = GetComponentInChildren<Laser>();
        thisMirrorsLaserScript.laserActivated = false;
    }

    void Update()
    {
        if (youCan) Interaction();

        // frozen if it is connected to PowerOut
        if (isConnected)
        {
            gameObject.transform.position = new Vector3(socketCollider.transform.position.x, socketCollider.transform.position.y + 0.25f, socketCollider.transform.position.z);

            /**
             * After placing the mirror in the socketCollider activate the laser
             */
            thisMirrorsLaserScript.laserActivated = true;
            /**
             * Keep the game object original rotation 
             */
            //gameObject.transform.rotation = socketCollider.transform.rotation;
            //DoorObject.isOpened = true;
        }
        else
        {
            /**
             * if the user picks the object up again then turn laser OFF again 
             */
            thisMirrorsLaserScript.laserActivated = false;
            //DoorObject.isOpened = false;
        }
    }

    public void Interaction()
    {
        /**
         * Pick up the mirror game object 
         */
        if (NearView() && Input.GetKeyDown(KeyCode.E) && !follow)
        {
            isConnected = false; // unfrozen
            
            follow = true;
            followFlag = false;

            /**
             * set the size of the sphere collider back to 1 after the object is taken away 
             */
            Invoke("ResetCollider", 1.25f);
        }

        /**
         * Rotate the mirror game object WHEN FOLLOWING!!!
         */
        if (Input.GetKeyDown(KeyCode.R) && follow)
        {
            //rotate the cube by 90 each press of the R button 
            gameObject.transform.rotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation
        }

        if (follow)
        {
            //boxCol.enabled = false;
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
            //boxCol.enabled = true;
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
        if (distance < 3f) return true;
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Socket")
        {
            //get the socket game object 
            socketGameObject = other.gameObject;

            //set the socketCollider on collision 
            socketCollider = socketGameObject.GetComponent<SphereCollider>();

            /**
             * set the size of the sphere colider to the smallest possible after activating the trigger 
             */
            //socketCollider.radius = 0.1f;


            /**
             * Disable the collider after collision  
             */
            socketCollider.enabled = false;

            isConnected = true;
            follow = false;
            //DoorObject.rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));


            //******************************************************************************************************ADD Lazer Activation Code HERE

            //******************************************************************************************************ADD Lazer Activation Code HERE

        }

        //if (OneTime) youCan = false;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Socket")
    //    {
    //        //set the socketCollider on collision 
    //        socketCollider = other.gameObject.GetComponent<SphereCollider>();

    //        /**
    //        * set the size of the sphere collider back to 1 after the object is taken away 
    //        */
    //        Invoke("ResetCollider", 1.25f); 
    //    }
    //}

    private void ResetCollider()
    {
        socketCollider.enabled = !isConnected;
        socketCollider.radius = 0.8f;
    }
}
