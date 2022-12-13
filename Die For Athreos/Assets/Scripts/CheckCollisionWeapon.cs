using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionWeapon : MonoBehaviour
{
    public float weaponDamage;
    public float time = 5f;
    public float timer;
    void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision col)
    {
        
        if (CompareTag("InHand"))
        {
            if (col.gameObject.tag == "enemy" && timer >= time)
            {
                col.gameObject.GetComponent<EnemyAI>().TakeDamage(weaponDamage);
                timer = 0;
            }
            

        }
        
    }
}
