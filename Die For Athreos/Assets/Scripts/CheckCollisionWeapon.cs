using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionWeapon : MonoBehaviour
{
    public float weaponDamage;
    public float time = 5f;
    public float timer;
    private int inHandLayer;
    public GameObject player;
    void Start()
    {
        timer = Time.time;
        inHandLayer = LayerMask.NameToLayer("InHand");
   
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider col)
    {
        
        if (gameObject.layer==inHandLayer)
        {
            if (col.gameObject.tag == "enemy" && timer >= time)
            {
                col.gameObject.GetComponent<EnemyAI>().TakeDamage(weaponDamage+ player.GetComponent<PlayerStats>().GetAttackDamage());
                timer = 0;
            }
            

        }
        
    }
}
