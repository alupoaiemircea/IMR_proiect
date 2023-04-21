using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float life = 3f;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Player");
        Destroy(gameObject,life);
    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("ENTERED");
        if (col.gameObject.tag == "enemy")
        {
            
            col.gameObject.GetComponent<EnemyAI>().TakeDamage(player.GetComponent<PlayerStats>().attackDamage);
            player.GetComponent<PlayerStats>().IncreaseFrenzy();
            player.GetComponent<PlayerStats>().ResetFrenzyTimer();
            Debug.Log("ENTERED ENEMY");
        }
        Destroy(gameObject);
    }
}
