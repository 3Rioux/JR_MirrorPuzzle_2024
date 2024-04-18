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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /**
         * New pickup method 
         */
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    float pickUpDistance = 2f;
        //    Transform playerCamera = Camera.main.transform;
        //    if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
        //    {
        //        if (raycastHit.transform.TryGetComponent(out MirrorInteraction mirrorInteraction))
        //        {
        //            mirrorInteraction.Interaction();
        //            mirrorInteraction.Interaction();
        //        }
        //    }
        //}
    }
}
