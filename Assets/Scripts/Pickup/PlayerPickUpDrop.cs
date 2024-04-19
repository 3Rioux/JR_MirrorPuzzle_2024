using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    /**
     * Set the layer that can be picked up by the user 
     */
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;


    private ObjectGrabbable objectGrabbable;

    
    private MirrorSnap mirrorSnapPlayerPickup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /**
         * New pickup method 
         */
        if (Input.GetKeyDown(KeyCode.E) && !mirrorSnapPlayerPickup.youCan)
        {
            //check if already have something picked up
            if (objectGrabbable == null)//not carrying 
            {
                float pickUpDistance = 2f;
                Transform playerCamera = Camera.main.transform;

                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                    }
                }
            }else
            {
                objectGrabbable.Drop();
                objectGrabbable = null; // clear the object so we can pickup a new one 
            }
        }
    }
}
