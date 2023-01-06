using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangramPuzzle : MonoBehaviour
{
    public float distance = 10f;
    public GameObject puzzle;
    private GameObject current_piece;
    public Animator anim;
    public Animator player;
    bool open = false;
    private bool canGrab;
    public UnityEngine.UI.Image crosshair;
    private bool rotate=false;
    public float rotation_speed;
    private float[] solved = { 90f, 135f, 45f, 90f, 180f, 90f, 45f };
    private bool[] sol_check = {false, false, false, false, false, false, false};
    private bool selected = false;
    void Awake()
    {
        crosshair.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        CheckSolution();
        SolveTangram();
        if (canGrab)
        {
            if (Input.GetKeyDown(KeyCode.E) && !current_piece.GetComponent<Outline>().enabled &&!selected)
            {
                selected = true;
                current_piece.GetComponent<Outline>().enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.R)&& current_piece.GetComponent<Outline>().enabled && selected)
            {
                rotate = true;
            }
            if (Input.GetKeyUp(KeyCode.R) && current_piece.GetComponent<Outline>().enabled && selected)
            {
                rotate=false;
            }
            if (rotate)
            { Rotating(); }
            if (Input.GetKeyDown(KeyCode.T) && current_piece.GetComponent<Outline>().enabled && selected)
            {
                current_piece.GetComponent<Outline>().enabled = false;
                Debug.Log("diasble");
                selected = false;
            }
        }

    }

    public void SolveTangram()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if (hit.transform.tag == "TangramPuzzle")
            {
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                gameObject.layer = LayerIgnoreRaycast;
                 crosshair.enabled = true; 
                
                //Debug.Log("puzzle");
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * distance, Color.blue);
            }
            if (hit.transform.name == "big_triangle")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
                
                canGrab = true;
            }
            if (hit.transform.name == "big_triangle_up")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
               
                canGrab = true;
            }
            if (hit.transform.name == "medium_triangle")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
                
                canGrab = true;
            }
            if (hit.transform.name == "romb")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
                
                canGrab = true;
            }
            if (hit.transform.name == "small_triangle_right")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
               
                canGrab = true;
            }
            if (hit.transform.name == "small_triangle")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
               
                canGrab = true;
            }
            if (hit.transform.name == "square")
            {
                current_piece = hit.transform.gameObject;
                //Debug.Log(current_piece.name);
              
                canGrab = true;
            }
            
           
        }
        else

        {
            int Default = LayerMask.NameToLayer("Default");
            gameObject.layer = Default;
            crosshair.enabled = false;
            canGrab = false;
        }
       
    }
    private void DisableOutline()
    {

    }
   private void Rotating()
    {
        float x;
        float y;
        float z;
        x = current_piece.transform.localRotation.eulerAngles.x;
        y = current_piece.transform.localRotation.eulerAngles.y;
        z = current_piece.transform.localRotation.eulerAngles.z;
        current_piece.transform.Rotate(new Vector3(x,y,z+10)*rotation_speed * Time.deltaTime);

    }
    private void CheckSolution()
    {

        int checks = 0;
       for(int i=0;i<7;i++)
        {
            float aux  = puzzle.transform.GetChild(i).localRotation.eulerAngles.z;
            float z;
           
            if(aux>180f)
            { z = 360f - aux; }
             else
            { z = aux; }
            if(i==6)
            { z = z % 45; }
            // Debug.Log(z);
            if ( solved[i]-20 <= z && z <= solved[i] + 20)
            {
                sol_check[i] = true;
            }
           else
            {
                sol_check[i] = false;
            }
           
        }
       for(int i=0;i<7;i++)
        {
            if(!sol_check[i] && checks!=0)
            { 
                checks--;
                break;
            }
            else
            { checks++; }
            //print(sol_check[i]);
        }
        //Debug.Log(checks);
       if(checks==7 && !open)
        {
            anim.SetTrigger("open");
            //Debug.Log("OPEN");
            open = true;
        }
    }
}
      



