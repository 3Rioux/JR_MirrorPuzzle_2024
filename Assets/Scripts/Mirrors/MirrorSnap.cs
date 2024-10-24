using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        this.thisMirrorsLaserScript = GetComponentInChildren<Laser>();
        this.thisMirrorsLaserScript.laserActivated = false;

        /**
         * Access the Object Grabbable Script 
         */
        this.thisMirrorGabbableScript = GetComponent<ObjectGrabbable>();
    }

    void Update()
    {
        //if (youCan) Interaction();

        // frozen if it is connected to PowerOut
        if (this.isConnected)
        {
            /**
             * If is connected make the player drop the object then snap in place
             */
            this.thisMirrorGabbableScript.Drop();

            //snap in place to the socket 
            this.gameObject.transform.position = new Vector3(socketCollider.transform.position.x, socketCollider.transform.position.y + 0.25f, socketCollider.transform.position.z);

            /**
             * After placing the mirror in the socketCollider activate the laser
             */
            //if (this.thisMirrorsLaserScript.previousLaserTrue)
            //{
                this.thisMirrorsLaserScript.laserActivated = true;
            //}else
            //{
            //    Debug.Log("Not Active");
            //}
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
            this.thisMirrorsLaserScript.laserActivated = false;
            
            //DoorObject.isOpened = false;


            /**
             * set the size of the sphere collider back to 1 after the object is taken away AFTER 1 second delay
             */
            this.Invoke("ResetCollider", 1f);
            //ResetCollider();
        }
    }

   


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Socket")
        {
            //get the socket game object 
            this.socketGameObject = other.gameObject;

            //set the socketCollider on collision 
            this.socketCollider = this.socketGameObject.GetComponent<SphereCollider>();

            /**
             * set the size of the sphere collider to the smallest possible after activating the trigger 
             */
            this.socketCollider.radius = 0.1f;


            /**
             * Disable the collider after collision  
             */
            this.socketCollider.enabled = false;

            this.isConnected = true;
            follow = false;
            //DoorObject.rbDoor.AddRelativeTorque(new Vector3(0, 0, 20f));


            //******************************************************************************************************ADD Lazer Activation Code HERE

            //******************************************************************************************************ADD Lazer Activation Code HERE

        }

        //if (OneTime) youCan = false;
    }

    private void ResetCollider()
    {
        if (socketCollider != null)
        {
            this.socketCollider.enabled = !this.isConnected;
            socketCollider.radius = 0.8f;
            CancelInvoke("ResetCollider");
        }
    }
}
