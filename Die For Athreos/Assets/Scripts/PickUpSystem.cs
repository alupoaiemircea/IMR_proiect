using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//adaptat: https://www.youtube.com/watch?v=8kKLUsn7tcg&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=6
public class PickUpSystem : MonoBehaviour
{
    public Transform left_equipPosition;
    public Transform right_equipPosition;
    public float distance = 10f;
    GameObject right_currentWeapon = null;
    GameObject left_currentWeapon = null;
    GameObject wp;
    public GameObject tangram_puzzle;
    public UnityEngine.UI.Image crosshair;
    bool canGrab;

    private int inHandLayer;

    public Animator left_hand_Animator;
    public Animator right_hand_Animator;

    private void Awake()
    {
        inHandLayer= LayerMask.NameToLayer("InHand");
        crosshair.enabled = false;
    }
    private void Update()
    {
        CheckWeapons();
        if(Input.GetKeyUp(KeyCode.R) && right_currentWeapon.tag=="HealthPotion")
        {
            Destroy(right_currentWeapon);
           right_hand_Animator.SetTrigger("isDropping");
            gameObject.GetComponent<PlayerStats>().Heal(5);
        }
        if(canGrab)
        {
            if(left_currentWeapon==null || right_currentWeapon==null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {            
                    PickUp();
                }
            }          
        }
        if(right_currentWeapon != null)
        { if (Input.GetKeyDown(KeyCode.Q))
            {              
                Drop();             
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
              
            }
            if (hit.transform.tag == "scepter")
            {
                
                canGrab = true;
                wp = hit.transform.gameObject;
              

            }
            if (hit.transform.tag == "dagger")
            {

                canGrab = true;
                wp = hit.transform.gameObject;

            }
            if (hit.transform.tag == "HealthPotion")
            {
                Debug.Log("health time");
                canGrab = true;
                wp = hit.transform.gameObject;
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
            right_currentWeapon = wp;
            right_currentWeapon.tag = wp.tag;
            right_currentWeapon.transform.position = right_equipPosition.position;
            right_currentWeapon.transform.parent = right_equipPosition;
            right_currentWeapon.layer = inHandLayer;
            RotateObj(ref right_currentWeapon,true);
            right_currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        if(wp!=right_currentWeapon)
        {
            left_hand_Animator.SetTrigger("isGrabbing");
            left_currentWeapon = wp;
            left_currentWeapon.tag = wp.tag;
            left_currentWeapon.transform.position = left_equipPosition.position;
            left_currentWeapon.transform.parent = left_equipPosition;
            left_currentWeapon.layer = inHandLayer;
            RotateObj(ref left_currentWeapon, false);
            left_currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }

    private void Drop()
    {
        int defalutLayer = LayerMask.NameToLayer("Default");
        right_currentWeapon.layer=defalutLayer;
        if(right_currentWeapon.tag=="scepter")
        {
            crosshair.enabled = false;
            right_currentWeapon.GetComponentInChildren<Rigidbody>().detectCollisions = true;
        }
        if (right_currentWeapon.tag == "HealthPotion")
        {
            right_currentWeapon.GetComponentInChildren<Rigidbody>().detectCollisions = true;
        }
        right_hand_Animator.SetTrigger("isDropping");
        right_currentWeapon.transform.parent = null;
        right_currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        right_currentWeapon.GetComponent<Rigidbody>().useGravity = true;
        right_currentWeapon = null;
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
                left_currentWeapon.transform.localPosition = new Vector3(0f,0f,0f);
                RotateObj(ref left_currentWeapon, false);
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
                right_currentWeapon.transform.localPosition = new Vector3(0f,0f,0f);
                RotateObj(ref right_currentWeapon, true);
            
            }
            else
            {
                GameObject temp = null;
                temp = right_currentWeapon;
                

                right_currentWeapon = left_currentWeapon;
               
                left_currentWeapon = temp;
              

                right_currentWeapon.transform.position = right_equipPosition.position;
                right_currentWeapon.transform.parent = right_equipPosition;
                right_currentWeapon.transform.localPosition = new Vector3(0f,0f,0f);
                RotateObj(ref right_currentWeapon,true);
            

                left_currentWeapon.transform.position = left_equipPosition.position;
                left_currentWeapon.transform.parent = left_equipPosition;
                left_currentWeapon.transform.localPosition= new Vector3(0f, 0f, 0f);
                RotateObj(ref left_currentWeapon, false);



        }
               
        
    }

    private void RotateObj(ref GameObject obj,bool rightHand)
    {
      if (rightHand)
        {
            if (obj.tag == "sword")
            {
                obj.transform.localEulerAngles = new Vector3(0f, 120f, 90f);
                obj.transform.localPosition = new Vector3(0.0072f, 0.0081f, -0.0159f);
            }
            else
              if (obj.tag == "HealthPotion")
            {
                obj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                obj.transform.localPosition = new Vector3(0f, 0.015f, 0f);
                wp.GetComponent<Rigidbody>().detectCollisions = false;
            }
            else
              if (obj.tag == "scepter")
            {
                obj.transform.localEulerAngles = new Vector3(0f, 30f, 90f);
                obj.transform.localPosition = new Vector3(0.0324f, 0.0089f, -0.0266f);
                crosshair.enabled = true;
                wp.GetComponent<Rigidbody>().detectCollisions = false;
            }
            else
              if (obj.tag == "dagger")
            {
                obj.transform.localEulerAngles = new Vector3(160f, 30f, 90f);
                obj.transform.localPosition = new Vector3(-0.0257f, 0.0081f, 0.01f);
            }
            else
            {
                obj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
      else
        {
            if (obj.tag == "sword")
            {
                //obj.transform.localEulerAngles = new Vector3(344.398956f, 304.720123f, 275.388397f);
                obj.transform.localEulerAngles = new Vector3(-6.1f, -57.222f, -101.183f);
                obj.transform.localPosition = new Vector3(0.0056f,0.0011f,-0.0152f);
            }
            else
            if (obj.tag == "HealthPotion")
            {
                obj.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                obj.transform.localPosition = new Vector3(0f, 0.015f, 0f);
                wp.GetComponent<Rigidbody>().detectCollisions = false;
            }
            else
              if (obj.tag == "scepter")
            {
                obj.transform.localEulerAngles = new Vector3(0f, 30f, 90f);
                obj.transform.localPosition = new Vector3(0.0324f, 0.0089f, -0.0266f);
                crosshair.enabled = true;
                wp.GetComponent<Rigidbody>().detectCollisions = false;
            }
            else
              if (obj.tag == "dagger")
            {
                obj.transform.localEulerAngles = new Vector3(160f, 30f, 90f);
                obj.transform.localPosition = new Vector3(-0.0257f, 0.0081f, 0.01f);
            }
            else
            {
                obj.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }
        }
        
    }
}
