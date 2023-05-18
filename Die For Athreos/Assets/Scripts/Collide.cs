using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    // Start is called before the first frame
    public float damage=1;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag =="Player")
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
