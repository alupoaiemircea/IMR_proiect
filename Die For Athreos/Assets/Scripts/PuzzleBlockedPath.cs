using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//adaptat: https://www.youtube.com/watch?v=8kKLUsn7tcg&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=6
public class PuzzleBlockedPath : MonoBehaviour
{
    
    public float distance = 10f;
    GameObject apple;
    bool canGrab;
    int nr_of_apples = 0;
    public GameObject blockedPath;

    public float time = 2f;
    public float timer;

    private void Start()
    {
        timer = Time.time;
    }
    private void Update()
    {
        timer+= Time.deltaTime;
        CheckWeapons();
        if (canGrab && timer>=time)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
                timer=0;
            }
        }
    }

    private void CheckWeapons()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance, Color.red);
            if (hit.transform.tag == "PuzzleBlockedPath")
            {
                //Debug.Log("i can grab it");
                canGrab = true;
                apple = hit.transform.gameObject;

            }
        }
        else
        {
            canGrab = false;
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance, Color.blue);
        }
    }
    private void PickUp()
    {
        Destroy(apple);
        nr_of_apples++;
        if (nr_of_apples==3)
        {
            blockedPath.GetComponent<OpenBlockedPath>().enabled = true;
        }
    }

}
