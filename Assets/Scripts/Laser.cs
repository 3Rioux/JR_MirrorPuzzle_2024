using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    int maxBounces = 1;

    private LineRenderer lineRenderer;

    [SerializeField]
    private Transform laserStartPoint;

    [SerializeField]
    private bool reflectOnlyMirror;

    //bool to toggle the mirrors laser 
    public bool laserActivated = false; //off by default 


    //bool to toggle the mirrors laser until the previous laser makes contact with this mirror 
    public bool previousLaserTrue = false; //off by default 

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, laserStartPoint.position); // set the position of the line to start at laser out

        //Ignore the collisions between layer 0 (default) and layer 8 (custom layer you set in Inspector window)
        //Physics.IgnoreLayerCollision(7,8);
    }

    // Update is called once per frame
    void Update()
    {
        if (laserActivated && previousLaserTrue) // && previousLaserCollision == true
        {
            //Make sure the line renderer is Enabled 
            lineRenderer.enabled = true;
            CastLaser(transform.position, -transform.forward);
        }else
        {
            //Disable the laser when laser is NOT active
            lineRenderer.enabled = false;

            //reset the previous laser bool 
            previousLaserTrue = false;
        }
    }

    /**
     * 
     */
    private void CastLaser(Vector3 position, Vector3 reflectionDirection)
    {
        /*
         * reset every time its activated incase the laserOut point gets moved 
         */
        lineRenderer.SetPosition(0, laserStartPoint.position);

        for(int i = 0; i < maxBounces; i++)
        {
            Ray ray = new Ray(position, reflectionDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                position = hit.point;

                // calculate the angle 
                //reflectionDirection = Vector3.Reflect(reflectionDirection, hit.normal); 

                //set the angle 
                lineRenderer.SetPosition(i + 1, hit.point);

                //test the reflection is on a mirror && is the reflectOnlyMirror bool toggled from the serialised feield
                if (hit.transform.tag != "Mirror" && reflectOnlyMirror)
                {
                    for (int j = (i + 1); j <= maxBounces; j++)
                    {
                        lineRenderer.SetPosition(j, hit.point);
                    }
                }

                /**
                 * Check if the laser is hitting a mirror 
                 * if yes then activate le laser for that mirror if not do nothing !!!!!!!!!!!!!!!!!!!!!!!!!MAKe this happen in the OnCollisionEnter 
                 */
                if (hit.transform.tag == "Mirror")
                {
                    //Debug.Log("Is hitting mirror!!!");
                    //thisLaserScript = hit.transform.gameObject.GetComponentInParent<Laser>();
                    //thisLaserScript.previousLaserTrue = true;
                    // Check if the hit object has a parent with a Laser script attached
                    Laser laserScript = hit.transform.gameObject.GetComponentInChildren<Laser>();

                    if (laserScript != null)
                    {
                        // You can now access methods or properties of the Laser script
                        laserScript.previousLaserTrue = true;
                    }
                    else
                    {
                        Debug.Log("No Laser script found on the hit object's parent." + hit.transform.name);
                    }
                }

                /**
                 * END GAME HERE OR IN THE LASER START END SCRIPT*******************************************
                 */
                //if (hit.transform.tag == "LaserStartEnd")
                //{

                //}

                    //set the angle 
                    lineRenderer.SetPosition(i + 1, hit.point);
            }

        }
    }
}
