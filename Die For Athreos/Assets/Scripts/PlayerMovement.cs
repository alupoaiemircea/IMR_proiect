using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//adaptat: https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
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
    private Vector3 moveDirection;

    [Header("Dashing")]
    public Transform orientation;
    public Transform player_camera;
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    private Vector3 delayedForceToApply;
    public float dashCD;
    private float dashCDtimer;
    public bool dashing = false;
    private Vector2 moveInputDash;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;
    public float groundDrag;

    [Header("Slope Hndling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;


    public bool fatigue=false;
    void Start()
    {
        rig= GetComponent<Rigidbody>();
        rig.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<PlayerStats>().penalty==false)
        {
            fatigue=false;
        }
        if (OnSlope())
        {
            rig.AddForce(GetSlopeMoveDirection() * currentSpeed, ForceMode.Force);
            
        }
        grounded =Physics.Raycast(transform.position, Vector3.down,playerHeight*0.5f+0.2f,whatIsGround);

        if (dashing)
        {
            rig.useGravity = false;
            currentSpeed = dashForce;
            rig.drag = 0; 
            Dash();
        }
        if (fatigue && Input.GetKeyUp(KeyCode.Space))
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

        moveForward = Input.GetAxisRaw("Vertical") * currentSpeed;
        moveSide = Input.GetAxisRaw("Horizontal") * currentSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !fatigue && dashCDtimer==0)
        {       
            dashing = true;
            Invoke(nameof(resetDash), dashDuration);
            dashCDtimer = dashCD;
        }
        
        if (dashCDtimer > 0)
        { dashCDtimer -= Time.deltaTime; }
        else { dashCDtimer = 0; }
        SpeedControl();
        CheckGrounded();
        rig.useGravity = !OnSlope();
    }
    private void CheckGrounded()
    {
        if (grounded)
            rig.drag = groundDrag;
        else
            rig.drag = 0;
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
        if(flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized *currentSpeed;
            rig.velocity= new Vector3(limitedVel.x, rig.velocity.y, limitedVel.z);
        }
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight*0.5f+0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection,slopeHit.normal).normalized;
    }
    private void FixedUpdate()
    {
        moveDirection=orientation.forward*moveForward+ orientation.right*moveSide;
        //rig.velocity=(transform.forward*moveForward)+(transform.right*moveSide)+new Vector3(0,_gravity,0);
        rig.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }
    private void Dash()
    {
        moveInputDash.x = Input.GetAxisRaw("Vertical");
        moveInputDash.y = Input.GetAxisRaw("Horizontal");
        moveInputDash.Normalize();

        //dashing = true;
        
        //Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;
        //delayedForceToApply = forceToApply;
        //Invoke(nameof(DelayedDashForceToApply), 0.025f);
        //rig.AddForce(forceToApply, ForceMode.Impulse);

        gameObject.transform.Translate(new Vector3(moveInputDash.y,0,moveInputDash.x) * dashForce * Time.deltaTime);
        //rig.MovePosition(transform.position+ transform.forward * dashForce * Time.deltaTime);
        
        
    }

    private void DelayedDashForceToApply()
    {
        rig.velocity = moveInputDash*dashForce;
        rig.AddForce(delayedForceToApply, ForceMode.Impulse);
    }
    private void resetDash()
    {
        dashing=false;
        rig.useGravity = true;
       
    }
    public void SetCurrentSpeed(float value)
    { currentSpeed = value; }

    public float GetWalkSpeed()
    { return walkSpeed; }
}
