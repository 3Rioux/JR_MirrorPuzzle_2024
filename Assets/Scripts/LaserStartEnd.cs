using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStartEnd : MonoBehaviour
{
    //access the line renderer on the scripts gameobject 
    private LineRenderer lr;

    //access the start point (empty GameObject) for the laser 
    [SerializeField]
    private Transform startPoint;

    //start End bools =============================
    [SerializeField] public bool isEndPoint;// set to true if end point false if start point 

    [SerializeField] public bool isLaserOn;// set to true if switch is on else false  

    /**
     * access the laser script
     */
    private Laser thisLaserScript;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //set the default possition for the laser
        lr.SetPosition(0, startPoint.position);

        //set the raycastHit hit variable to detect if laser hits anything 
        RaycastHit hit;

        /**
         * Check if the laser is on or not 
         *              &&&
         * if the laser is the start so Not isEndPoint
         */
        if (isLaserOn && !isEndPoint)
        {
            if(Physics.Raycast(transform.position, -transform.right, out hit))
            {
                if (hit.collider)
                {
                    lr.SetPosition(1, hit.point);// set the end point of the ray(laser) to be where it hits 
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
            }
            //else lr.SetPosition(1, -transform.right * 1000);//default laser lenght == 1000 if does not hit anything

            

            //add the END game check HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



            //add the END game check HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }else
        {
            //if not start disable the lineRenderer
            lr.enabled = false;
        }
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    //check if THIS gameobject is the endpoint 
    //    if (collision.gameObject.tag == "Laser" && isEndPoint)
    //    {
    //        //gamemanager end game 
    //        Debug.Log("Game Over!!!");
    //    }
    //    else Debug.Log("Not ENd game" + collision.gameObject.tag);
    //}

}
