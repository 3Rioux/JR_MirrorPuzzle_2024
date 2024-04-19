using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        rb.drag = 10f;
        rb.angularDrag = 10f;

        mirrorSnapObjectGrababble.follow = true;

        this.PlayerHandPosition = objectGrabPointTransform;
        rb.useGravity = false;
    }

    public void Drop()
    {
        rb.drag = 0f;
        rb.angularDrag = .5f;

        mirrorSnapObjectGrababble.follow = false;

        this.PlayerHandPosition = null;
        rb.useGravity = true;
    }

    // Update is called once per frame
    private void Update()
    {


        if (PlayerHandPosition != null)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, PlayerHandPosition.position, 1f);
        }
    }
}
