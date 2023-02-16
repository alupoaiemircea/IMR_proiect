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

    private int inHandLayer;
    private bool foundR;
    private bool foundL;

    public bool fatigue=false;

    void Start()
    {
        inHandLayer = LayerMask.NameToLayer("InHand");
    }

    // Update is called once per frame
    void Update()
    {
        foundR = false;
        foundL = false;
        foreach (Transform child in righthand.transform)
        {
            if (child.gameObject.layer == inHandLayer)
            {
                rightWeapon = child.gameObject;
                foundR = true;
            }
        }
        if(!foundR)
        {
            rightWeapon = null;
        }
        foreach (Transform child in lefthand.transform)
        {
            if (child.gameObject.layer == inHandLayer)
            {
                leftWeapon = child.gameObject;
                foundL |= true;
            }
        }
        if (!foundL)
        {
            leftWeapon = null;
        }
        //if(rightWeapon!=null){
        if (Input.GetMouseButtonDown(1) && rightWeapon!=null &&!fatigue)
            {
              if(righthand.tag=="sword")
             { right_hand_Animator.SetTrigger("IsAttacking"); }
                
                gameObject.GetComponent<PlayerStats>().SetAttacking(true);
                
            }
            if (Input.GetMouseButtonDown(0) && leftWeapon!=null && !fatigue)
            {
              if (righthand.tag == "sword")
              { left_hand_Animator.SetTrigger("IsAttacking"); }
               
                gameObject.GetComponent<PlayerStats>().SetAttacking(true);

        }
        // }

    }

   
}
