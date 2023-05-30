using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float life = 3f;
    private GameObject player;
    public int damage = 1;
    private void Awake()
    {
        player = GameObject.Find("Player");
        Destroy(gameObject,life);
    }
  
    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("ENTERED");
        if (col.gameObject.tag == "enemy")
        {
            
            col.gameObject.GetComponent<EnemyAI>().TakeDamage(player.GetComponent<PlayerStats>().attackDamage+ damage);
            player.GetComponent<PlayerStats>().IncreaseFrenzy();
            player.GetComponent<PlayerStats>().ResetFrenzyTimer();
            //Debug.Log("ENTERED ENEMY");
            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "boss")
        {

            col.gameObject.GetComponent<BossAI>().TakeDamage(player.GetComponent<PlayerStats>().attackDamage+ damage);
            player.GetComponent<PlayerStats>().IncreaseFrenzy();
            player.GetComponent<PlayerStats>().ResetFrenzyTimer();
            //Debug.Log("ENTERED ENEMY");
            Destroy(gameObject);
        }



    }
 
}
