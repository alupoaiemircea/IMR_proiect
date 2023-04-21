using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=wZ2UUOC17AY&t=314s for shooting projectile adapted
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

    public Camera cam;
    public Transform attackPoint;
    public GameObject projectile;
    public float shootForce;
    public float timeBetweenShooting;

    private bool allowInvoke=true;
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
              if(rightWeapon.tag=="sword")
             { right_hand_Animator.SetTrigger("IsAttacking"); }
              else
              if (rightWeapon.tag == "dagger")
             { 
                right_hand_Animator.SetTrigger("attackDagger");
                Debug.Log("ATTACK DAGGER");
            }
              else
                if(rightWeapon.tag == "scepter")
            {
                ShootProjectile();
            }
              gameObject.GetComponent<PlayerStats>().SetAttacking(true);
                
            }
            if (Input.GetMouseButtonDown(0) && leftWeapon!=null && !fatigue)
            {
              if (leftWeapon.tag == "sword")
              { left_hand_Animator.SetTrigger("IsAttacking"); }
              else
              if (leftWeapon.tag == "dagger")
              { 
                left_hand_Animator.SetTrigger("attackDagger"); 
                 }
            else
                if (rightWeapon.tag == "scepter")
            {
                ShootProjectile();
            }
            gameObject.GetComponent<PlayerStats>().SetAttacking(true);

        }
        // }

    }

    private void ShootProjectile()
    {

        //GameObject attack = new GameObject();
        //attack.AddComponent<Rigidbody>();
        //attack.AddComponent<BoxCollider>();
        //attack.tag = "attack";
        //attack.AddComponent<Collide>();
        //attack.transform.position = Vector3.MoveTowards(transform.position, middle of screen, 100f * Time.deltaTime);
        float x = Screen.width / 2f;
        float y = Screen.height / 2f;

        Ray ray = cam.ViewportPointToRay(new Vector3(x, y, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - attackPoint.position;
        GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = direction.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized*shootForce,ForceMode.Impulse);

        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;
        }
    }
    private void ResetShot()
    {
        allowInvoke = true;
    }
}
