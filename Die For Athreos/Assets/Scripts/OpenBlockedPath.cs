using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//done by me
public class OpenBlockedPath : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private bool open = false;
    public AudioSource audioSource=null;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null)
        {
            if (!open)
            {
                animator.SetTrigger("TrOpenBlocked_path");
                open = true;
                audioSource.Play();
            }
        }
    }
}
