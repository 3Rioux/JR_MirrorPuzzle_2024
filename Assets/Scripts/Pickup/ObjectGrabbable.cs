using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * =====Is placed on ANY game Object you want to be able to pick up=====
 * 
 * Talks with the Players PlayerPickUpDrop script:
 *      - Gets the players Hand position for that script and if the hand gameObject is NOT null 
 *        it will Lerp(move) the mirror to the position of the hand.
 *        
 * Talks with the Game Manager:
 *      - The game Manager makes it so that the Mirror can tell the playerPickUpDrop script if the game object gets placed in a socket so 
 *        this scripts drop method needs to be run.
 * 
 */
public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody rb;

    [Tooltip("Mirror follows this local EmptyObject in the player prefab")]
    public Transform PlayerHandPosition;

    [SerializeField]
    private MirrorSnap mirrorSnapObjectGrababble;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        /**
         * Get access to the mirror socket script 
         */
        mirrorSnapObjectGrababble = GetComponent<MirrorSnap>();
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        rb.drag = 10f;
        rb.angularDrag = 10f;

        /**
         * Set the socket code to be following the player and NOT connected 
         */
        //mirrorSnapObjectGrababble.follow = true;
        mirrorSnapObjectGrababble.isConnected = false;

        this.PlayerHandPosition = objectGrabPointTransform;
        rb.useGravity = false;
    }

    public void Drop()
    {
        rb.drag = 0f;
        rb.angularDrag = .5f;

       // mirrorSnapObjectGrababble.follow = false;

        this.PlayerHandPosition = null;
        rb.useGravity = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerHandPosition != null)
        {
            /**
             * Rotate the mirror game object if the object is in the hand of the player
             */
            if (Input.GetKeyDown(KeyCode.R))
            {
                //rotate the cube by 90 each press of the R button 
                gameObject.transform.rotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation
            }

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, PlayerHandPosition.position, 1f);
        }
    }
}
