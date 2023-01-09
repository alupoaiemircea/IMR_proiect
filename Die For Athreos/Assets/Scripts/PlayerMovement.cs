using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//preluat: https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
public class PlayerMovement : MonoBehaviour
{
    float moveForward;
    float moveSide;
    float moveUp;

    public float walkSpeed = 3f;
    public float sprintSpeed;
    public float currentSpeed;
    Rigidbody rig;
    private float _gravity = -2f;

    public Transform orientation;
    public Transform player_camera;
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;

    public float dashCD;
    private float dashCDtimer;

    public bool fatigue=false;
    void Start()
    {
        rig= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fatigue && Input.GetKeyUp(KeyCode.Space))
        {
            fatigue=false;
        }
        if (Input.GetKey(KeyCode.Space) && gameObject.GetComponent<PlayerStats>().GetStamina()>0 && !fatigue)
        {
            if (currentSpeed != sprintSpeed && !fatigue)
            {
                currentSpeed = sprintSpeed;
            }
            gameObject.GetComponent<PlayerStats>().SetSprinting(true);
        }
        else
        { currentSpeed = walkSpeed;
            gameObject.GetComponent<PlayerStats>().SetSprinting(false);
        }

        moveForward = Input.GetAxis("Vertical") * currentSpeed;
        moveSide = Input.GetAxis("Horizontal") * currentSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        { Dash(); }
        if(dashCDtimer > 0)
        { dashCDtimer -= Time.deltaTime; }
    }
    private void FixedUpdate()
    {
        rig.velocity=(transform.forward*moveForward)+(transform.right*moveSide)+new Vector3(0,_gravity,0);
    }
    private void Dash()
    {
        if (dashCDtimer > 0) { return; }
        else dashCDtimer = dashCD;
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;
        rig.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(resetDash), dashDuration);
    }
    private void resetDash()
    {

    }
    public void SetCurrentSpeed(float value)
    { currentSpeed = value; }

    public float GetWalkSpeed()
    { return walkSpeed; }
}
