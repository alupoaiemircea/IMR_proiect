using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//preluat: https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
public class PlayerMovement : MonoBehaviour
{
    float moveForward;
    float moveSide;
    float moveUp;

    public float walkSpeed = 1f;
    public float sprintSpeed;
    float currentSpeed;
    Rigidbody rig;
    void Start()
    {
        rig= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (currentSpeed != sprintSpeed)
            {
                currentSpeed = sprintSpeed;
            }
        }
        else
            currentSpeed = walkSpeed;
        moveForward = Input.GetAxis("Vertical") * currentSpeed;
        moveSide = Input.GetAxis("Horizontal") * currentSpeed;
    }
    private void FixedUpdate()
    {
        rig.velocity=(transform.forward*moveForward)+(transform.right*moveSide);
    }
}
