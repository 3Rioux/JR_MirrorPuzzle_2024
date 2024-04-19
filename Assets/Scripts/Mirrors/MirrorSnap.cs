using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSnap : MonoBehaviour
{

    //[Tooltip("Mirror follows this local EmptyObject in the player prefab")]
    //public Transform PlayerHandPosition;

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

    /**
     * Access the Grabbable script to make the player drop the game object if the collision occurs 
     */
    private ObjectGrabbable thisMirrorGabbableScript;

    //public AN_DoorScript DoorObject;

    public bool follow = false;
    public bool isConnected = false, followFlag = false, youCan = true;

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

        /**
         * Access the Laser script and make sure the laser is off 
         */
        thisMirrorsLaserScript = GetComponentInChildren<Laser>();
        thisMirrorsLaserScript.laserActivated = false;

        /**
         * Access the Object Grabbable Script 
         */
        thisMirrorGabbableScript = GetComponent<ObjectGrabbable>();
    }

    void Update()
    {
        //if (youCan) Interaction();

        // frozen if it is connected to PowerOut
        if (isConnected)
        {
            /**
             * If is connected make the player drop the object then snap in place
             */
            thisMirrorGabbableScript.Drop();

            //snap in place to the socket 
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


            /**
             * set the size of the sphere collider back to 1 after the object is taken away AFTER 1 second delay
             */
            Invoke("ResetCollider", 1f);
        }
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
             * set the size of the sphere collider to the smallest possible after activating the trigger 
             */
            socketCollider.radius = 0.1f;


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

    private void ResetCollider()
    {
        socketCollider.enabled = !isConnected;
        socketCollider.radius = 0.8f;
    }
}
