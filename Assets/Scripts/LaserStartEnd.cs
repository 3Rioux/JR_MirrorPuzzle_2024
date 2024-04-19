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
    [SerializeField] private bool isEndPoint;// set to true if end point false if start point 

    [SerializeField] public bool isLaserOn;// set to true if switch is on else false  

    
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
            if(Physics.Raycast(transform.position, transform.forward, out hit))
            {
                lr.SetPosition(1, hit.point);// set the end point of the ray(laser) to be where it hits 
            }

            /**
             * Check if the laser is hitting a mirror 
             * if yes then activate le laser for that mirror if not do nothing !!!!!!!!!!!!!!!!!!!!!!!!!MAKe this happen in the OnCollisionEnter 
             */
            if (hit.transform.tag == "Mirror")
            {
                hit.transform.gameObject.GetComponent<Laser>().previousLaserTrue = true;
            }

            //add the END game check HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



            //add the END game check HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //check if THIS gameobject is the endpoint 
        if (isEndPoint)
        {
            //gamemanager end game 
            Debug.Log("Game Over!!!");
        }
    }

}
