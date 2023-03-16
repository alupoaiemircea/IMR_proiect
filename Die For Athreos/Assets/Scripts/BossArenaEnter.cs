using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaEnter : MonoBehaviour
{
    
    public GameObject bossHealthBar;

   
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player" && bossHealthBar.activeSelf==false)
        {
           
            bossHealthBar.SetActive(true);
        }
        else
        if (other.gameObject.tag == "Player" && bossHealthBar.activeSelf == true)
        {
            bossHealthBar.SetActive(false);
        }
    }
}
