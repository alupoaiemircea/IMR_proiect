using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RegisterPlayerMoves : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public GameObject playerWeapon;

    public float timeRemaining=1;

    private string path = "warrior_dataset.txt";
    private int action=-1;
    private bool alliesCalled=false;

    void Start()
    {
        //File.Delete(path);
    }

    // Update is called once per frame
    private string CreateDataSetLine(Vector3 playerPos, Vector3 enemyPos,float playerHealth,bool enemyAttacking, int resultingAction)
    {
        
        float enemyAttack = enemyAttacking ? 1.0f : 0f;
        return playerPos.x.ToString() + " " + playerPos.y.ToString() + " " +playerPos.z.ToString() + " " + 
               enemyPos.x.ToString() + " " + enemyPos.y.ToString() + " " + enemyPos.z.ToString() + " " +
            playerHealth.ToString() +" "+ enemyAttack.ToString() +" "+ resultingAction.ToString()+"\n";
    }
    private bool CheckForAlliesWhoCalled()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 15);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.tag=="allie")
            {
                //if (hitCollider.gameObject.GetComponent<Allie>().currentHealth <= hitCollider.gameObject.GetComponent<Allie>().maxHealth / 3)
                { return true; }
            }
        }
        return false;
    }
    void Update()
    {
        //if(timeRemaining>0)
        //{
        //    timeRemaining-= Time.deltaTime;
        //}
        //else
        //{
        float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            if (distance > 10 && action != 1)
            {
                //wander around 1
                 action = 1;
                string line = CreateDataSetLine(gameObject.transform.position, enemy.transform.position, gameObject.GetComponent<PlayerStats>().currentHealth, enemy.GetComponent<EnemyAI>().attacking, action);
                File.AppendAllText(path, line);
                
            }
            if (distance <= 10 && distance>4 && action !=2 )
            {
                //go towards player 2
              
                action = 2;
                string line = CreateDataSetLine(gameObject.transform.position, enemy.transform.position, gameObject.GetComponent<PlayerStats>().currentHealth, enemy.GetComponent<EnemyAI>().attacking, action);
                File.AppendAllText(path, line);
            }
            if (distance <= 4 && action != 3 && (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) )
            {
                //attack player 3
                
                action=3;
                string line = CreateDataSetLine(gameObject.transform.position, enemy.transform.position, gameObject.GetComponent<PlayerStats>().currentHealth, enemy.GetComponent<EnemyAI>().attacking, action);
                File.AppendAllText(path, line);

                
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
               //dodge 5
               action = 5;
               string line1 = CreateDataSetLine(gameObject.transform.position, enemy.transform.position, gameObject.GetComponent<PlayerStats>().currentHealth, enemy.GetComponent<EnemyAI>().attacking, action);
               File.AppendAllText(path, line1);
             }
            if (gameObject.GetComponent<PlayerStats>().currentHealth<= gameObject.GetComponent<PlayerStats>().maxHealth/3 && !alliesCalled)
            {
                //call allies 4
                alliesCalled=true;
                action = 4;
                string line = CreateDataSetLine(gameObject.transform.position, enemy.transform.position, gameObject.GetComponent<PlayerStats>().currentHealth, enemy.GetComponent<EnemyAI>().attacking, action);
                File.AppendAllText(path, line);
               
            }
            //if(CheckForAlliesWhoCalled() && distance <= 15)
            //{
            //    //go towards player 2
            //    action = 2;
            //    string line = CreateDataSetLine(gameObject.transform.position, enemy.transform.position, gameObject.GetComponent<PlayerStats>().currentHealth, gameObject.GetComponent<PlayerStats>().attacking, enemy.GetComponent<EnemyAI>().attacking, action);
            //    File.AppendAllText(path, line);
            //}
            timeRemaining = 1;
            
        //}

    }
}
