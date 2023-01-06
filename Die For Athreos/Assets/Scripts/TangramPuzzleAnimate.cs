using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangramPuzzleAnimate : MonoBehaviour
{
    bool open = false;
    public Animator anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !open)
        {
            anim.SetTrigger("open");
            open = true;
        }
    }
}
