﻿using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    public enum DirectionToMove { forward, back, still }; public DirectionToMove directionToMove; //Should player move forward or back
    public enum ColorOfPlayer { red, blue }; public ColorOfPlayer colorOfPlayer;
    public int lastWaypoint, nextWaypoint;
    public Transform[] waypoints; //Set Waypoints for player to move along
    public float speed, turningSpeed, thrust;
    public bool activePlayer, turnedAround, hasPuck;
    public GameObject puck; public Transform puckSpot;

    float lastRightTrigger, lastLeftTrigger;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {
            //other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //other.gameObject.transform.parent = gameObject.transform;
            //other.gameObject.transform.localPosition = puckSpot.localPosition;
            puck = other.gameObject;
            hasPuck = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Puck")
            hasPuck = true;
    }

    void Shoot()
    {
        puck.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
        puck.GetComponent<Rigidbody>().AddForce(transform.up * (thrust / 5f));
        puck.GetComponent<Rigidbody>().AddForce(transform.right * (thrust / 11));
        //puck.transform.parent = null;
        puck = null;
    }

	void Update () {
        Move();
        Turn();

        Vector3 forwardSpot = puckSpot.transform.TransformDirection(new Vector3((50 / 11), 0, 50));

        if (activePlayer)
        {
            Debug.DrawRay(new Vector3(puckSpot.transform.position.x, puckSpot.transform.position.y + .1f, puckSpot.transform.position.z), forwardSpot, Color.green);
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetAxis("RightTrigger") > .3f && lastRightTrigger < .3f))
            {
                if (hasPuck)
                {
                    hasPuck = false;
                    Shoot();
                }
            }
        }
        else
            Debug.DrawRay(new Vector3(puckSpot.transform.position.x, puckSpot.transform.position.y + .1f, puckSpot.transform.position.z), forwardSpot, Color.red);

        if (colorOfPlayer == ColorOfPlayer.blue) //Make sure rotation doesn't go -
        {
            if (transform.localEulerAngles.y > 360)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (transform.localEulerAngles.y - 360), transform.localEulerAngles.z);
            else if (transform.localEulerAngles.y < 0)
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (transform.localEulerAngles.y + 360), transform.localEulerAngles.z);
        }

        lastRightTrigger = Input.GetAxis("RightTrigger");
        lastLeftTrigger = Input.GetAxis("LeftTrigger");

	}

    void Turn()
    {
        if (activePlayer)
        {
            if (Input.GetButton("RightBumper") || Input.GetKeyDown(KeyCode.LeftShift))
                turningSpeed = 200;
            else
                turningSpeed = 800;

            if (Input.GetAxis("Horizontal") > .01f) //Rotate Around
            {
                transform.Rotate(Vector3.up * Time.deltaTime * turningSpeed * Input.GetAxis("Horizontal"));
            }
            else if (Input.GetAxis("Horizontal") < -.01f)
            {
                transform.Rotate(Vector3.down * Time.deltaTime * turningSpeed * Mathf.Abs(Input.GetAxis("Horizontal")));
            }

            if(colorOfPlayer == ColorOfPlayer.blue) //Set Turned Around if facing other direction
            {
                if (transform.localRotation.eulerAngles.y > 270 || transform.localRotation.eulerAngles.y < 90)
                    turnedAround = true;
                else
                    turnedAround = false;
            }else
            {
                if (transform.localRotation.eulerAngles.y < 270 && transform.localRotation.eulerAngles.y > 90)
                    turnedAround = true;
                else
                    turnedAround = false;
            }
        }
    }

    void Move()
    {
        if (activePlayer) //Only move when this player is active (Overwrite w/ AI later)
            GetInputForMovement();

        switch (directionToMove) //Call MoveToPoint depending on our direction to move
        {
            case DirectionToMove.forward:
                MoveToPoint(waypoints[nextWaypoint]);
                break;
            case DirectionToMove.back:
                MoveToPoint(waypoints[lastWaypoint]);
                break;
            case DirectionToMove.still:
                //Don't Do Anything
                break;
        }

        if(transform.position == waypoints[nextWaypoint].position) //Switch waypoints, allowing us to move on curves
        {
            if (nextWaypoint < waypoints.Length-1)
            {
                lastWaypoint++;
                nextWaypoint++;
            }
        }else if(transform.position == waypoints[lastWaypoint].position)
        {
            if (lastWaypoint > 0)
            {
                lastWaypoint--;
                nextWaypoint--;
            }
        }
    }

    void MoveToPoint(Transform point) //Move toward Waypoint
    {
        float step = speed * Time.deltaTime * Mathf.Abs(Input.GetAxis("Vertical"));
        transform.position = Vector3.MoveTowards(transform.position, point.position, step);
    }

    void GetInputForMovement() //Get input for moving
    {
        if (!turnedAround)
        {
            if (Input.GetAxis("Vertical") > .2f) //Set the direction to move based on input
                directionToMove = DirectionToMove.forward;
            else if (Input.GetAxis("Vertical") < -.2f)
                directionToMove = DirectionToMove.back;
            else
                directionToMove = DirectionToMove.still;
        }else
        {
            if (Input.GetAxis("Vertical") > .2f) //Move in the opposite direction if turned around
                directionToMove = DirectionToMove.back;
            else if (Input.GetAxis("Vertical") < -.2f)
                directionToMove = DirectionToMove.forward;
            else
                directionToMove = DirectionToMove.still;
        }
    }
}
