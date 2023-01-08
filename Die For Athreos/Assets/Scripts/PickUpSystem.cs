using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//adaptat: https://www.youtube.com/watch?v=8kKLUsn7tcg&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=6
public class PickUpSystem : MonoBehaviour
{
    public Transform left_equipPosition;
    public Transform right_equipPosition;
    public float distance = 10f;
    GameObject right_currentWeapon;
    GameObject left_currentWeapon;
    GameObject wp;
    public GameObject tangram_puzzle;
   
    bool canGrab;
    bool sword=false;
    bool potion = false;
    
    string right_tem_tag = "";
    string left_tem_tag = "";

    public Animator left_hand_Animator;
    public Animator right_hand_Animator;
 
    
    private void Update()
    {
        CheckWeapons();
        if(Input.GetKeyUp(KeyCode.R) && left_tem_tag=="HealthPotion")
        {
            Destroy(left_currentWeapon);
            left_hand_Animator.SetTrigger("isDropping");
            gameObject.GetComponent<PlayerStats>().Heal(5);
        }
        if(canGrab)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //if(right_currentWeapon != null)
                //{
                //    Drop();
                //}
                    PickUp();
                    
               
            }
           
        }
        if(right_currentWeapon != null)
        { if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
                if(right_tem_tag=="sword")
                { sword = false; }
                if(right_tem_tag=="HealthPotion")
                { potion = false;

                 }
                
            } 
        }
        if((left_currentWeapon != null || right_currentWeapon != null) && Input.GetKeyDown(KeyCode.F)) { Switch(); }
        
    }
    private void CheckWeapons()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit,distance))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance, Color.red);
            if (hit.transform.tag=="CanGrab")
            {
                Debug.Log("i can grab it");
                canGrab = true;
                wp = hit.transform.gameObject;
            }
            if (hit.transform.tag == "sword")
            {
                //Debug.Log("sword time");
                canGrab = true;
                wp = hit.transform.gameObject;
                sword = true;
            }
            if (hit.transform.tag == "HealthPotion")
            {
                Debug.Log("health time");
                canGrab = true;
                wp = hit.transform.gameObject;
                potion = true;
            }

        }
        else
            canGrab = false;
    }
    private void PickUp()
    {
        if(right_currentWeapon == null)
        {
            right_hand_Animator.SetTrigger("isGrabbing");
            right_tem_tag = wp.tag;
            wp.tag = "InHand";
            right_currentWeapon = wp;
            right_currentWeapon.tag = "InHand";
            right_currentWeapon.transform.position = right_equipPosition.position;
            right_currentWeapon.transform.parent = right_equipPosition;
            if (sword)
            {
                right_currentWeapon.transform.localEulerAngles = new Vector3(0f, 120f, 90f);
                right_currentWeapon.transform.position = new Vector3(right_currentWeapon.transform.position.x, right_currentWeapon.transform.position.y, right_currentWeapon.transform.position.z);
            }
            else
            if (potion)
            {
                right_currentWeapon.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                right_currentWeapon.transform.localPosition = new Vector3(0f, 0.015f, 0f);
            }
            else
            {
                right_currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
            right_currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        if(wp!=right_currentWeapon)
        {
            left_hand_Animator.SetTrigger("isGrabbing");
            left_tem_tag = wp.tag;
            wp.tag = "InHand";
            left_currentWeapon = wp;
            left_currentWeapon.tag = "InHand";
            left_currentWeapon.transform.position = left_equipPosition.position;
            left_currentWeapon.transform.parent = left_equipPosition;
            if (sword)
            {
                left_currentWeapon.transform.localEulerAngles = new Vector3(344.398956f, 304.720123f, 275.388397f);
                left_currentWeapon.transform.position = new Vector3(left_currentWeapon.transform.position.x, left_currentWeapon.transform.position.y, left_currentWeapon.transform.position.z);
            }
            else
            if (potion)
            {
                left_currentWeapon.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                left_currentWeapon.transform.localPosition = new Vector3(0f,0.015f,0f);
            }
            else
            {
                left_currentWeapon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
            left_currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }

    private void Drop()
    {
        right_hand_Animator.SetTrigger("isDropping");
        right_currentWeapon.tag = right_tem_tag;
        right_currentWeapon.transform.parent = null;
        right_currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        right_currentWeapon.GetComponent<Rigidbody>().useGravity = true;
        right_currentWeapon = null;
        right_tem_tag=null;
    }

    private void Switch()
    {
        
            
            if(right_currentWeapon!=null && left_currentWeapon==null)
            {
                right_hand_Animator.SetTrigger("isDropping");
                left_hand_Animator.SetTrigger("isGrabbing");

                left_currentWeapon = right_currentWeapon;
                right_currentWeapon.transform.parent = null;
                right_currentWeapon = null;

                left_currentWeapon.transform.position = left_equipPosition.position;
                left_currentWeapon.transform.parent = left_equipPosition;
                left_currentWeapon.transform.position = new Vector3(left_currentWeapon.transform.position.x, left_currentWeapon.transform.position.y ,left_currentWeapon.transform.position.z);
                left_tem_tag = right_tem_tag;
                right_tem_tag = null;
            }
            else
            if (left_currentWeapon != null && right_currentWeapon==null)
            {
                left_hand_Animator.SetTrigger("isDropping");
                right_hand_Animator.SetTrigger("isGrabbing");

                right_currentWeapon = left_currentWeapon;
                left_currentWeapon.transform.parent = null;
                left_currentWeapon = null;

                right_currentWeapon.transform.position = right_equipPosition.position;
                right_currentWeapon.transform.parent = right_equipPosition;
                right_currentWeapon.transform.position = new Vector3(right_currentWeapon.transform.position.x, right_currentWeapon.transform.position.y, right_currentWeapon.transform.position.z);
                right_tem_tag=left_tem_tag;
                left_tem_tag = null;
            }
            else
            {
                GameObject temp = null;
                temp = right_currentWeapon;
                

                right_currentWeapon = left_currentWeapon;
               
                left_currentWeapon = temp;
              

                right_currentWeapon.transform.position = left_equipPosition.position;
                right_currentWeapon.transform.parent = left_equipPosition;
                right_currentWeapon.transform.localPosition = new Vector3(0f,0f,0f);

                left_currentWeapon.transform.position = right_equipPosition.position;
                left_currentWeapon.transform.parent = right_equipPosition;
                left_currentWeapon.transform.localPosition= new Vector3(0f, 0f, 0f);

                string tag = null;
                tag = left_tem_tag;
                left_tem_tag = right_tem_tag;
                right_tem_tag = tag;
                Debug.Log(right_tem_tag);
            }
               
        
    }

   
}
