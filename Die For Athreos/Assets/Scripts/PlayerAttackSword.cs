using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSword : MonoBehaviour
{
    GameObject rightWeapon;
    GameObject leftWeapon;

    public GameObject righthand;
    public GameObject lefthand;

    public Animator left_hand_Animator;
    public Animator right_hand_Animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Transform child in righthand.transform)
        {
            if (child.tag == "InHand")
            {
                rightWeapon = child.gameObject;
            }
        }
        foreach (Transform child in lefthand.transform)
        {
            if (child.tag == "InHand")
            {
                leftWeapon = child.gameObject;
            }
        }

        //if(rightWeapon!=null){
            if (Input.GetMouseButtonDown(1))
            {
                right_hand_Animator.SetTrigger("IsAttacking");
               
                
            }
            if (Input.GetMouseButtonDown(0))
            {
                left_hand_Animator.SetTrigger("IsAttacking");


            }
        // }

    }

   
}
