using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//preluat: https://www.youtube.com/watch?v=UjkSFoLxesw&t=267s
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    //0-melee, 1-ranged
    public int type = 0;
    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public bool attacking=false;
    public float damage=1;
    public float shootForce = 1;
    public GameObject enemy_shoot;
    private Transform attack_point;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Health
    public float maxHealth;
    public float currentHealth;
    //animation
    public Animator animator;
    public Animator boss_animator;
    public int xp = 4;
    //flash red when hit
    public float flashTime;
    Color origionalColor;
    SkinnedMeshRenderer render;
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent=GetComponent<NavMeshAgent>();
        currentHealth=maxHealth;

        render=gameObject.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        origionalColor = render.material.color;
        attack_point = transform.Find("attack_point");
    }
    
    // Update is called once per frame
    void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) { AttackPlayer(); attacking = true; }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
        if(gameObject.tag=="boss")
        {
            boss_animator.SetTrigger("walking");
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint=new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z+randomZ);

        if(Physics.Raycast(walkPoint,-transform.up,2f,whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            if (type == 1)
            {
                animator.SetTrigger("isAttacking");
                /*
                GameObject attack = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                attack.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                attack.AddComponent<Rigidbody>();
                attack.AddComponent<SphereCollider>();
                attack.tag = "attack";
                attack.AddComponent<Collide>();
                attack.GetComponent<Collide>().damage = damage;
                attack.transform.position = Vector3.MoveTowards(transform.position, player.position, shootForce * Time.deltaTime);
                //attack.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * shootForce, ForceMode.Impulse);
                
                Debug.Log("monster attack");
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                */
                Ray ray = new Ray(attack_point.position,attack_point.transform.forward);
                RaycastHit hit;

                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit))
                    targetPoint = hit.point;
                else
                    targetPoint = ray.GetPoint(20);

                Vector3 direction = targetPoint - attack_point.position;
                GameObject shoot = Instantiate(enemy_shoot,attack_point.position, Quaternion.identity);
                shoot.transform.forward = direction.normalized;
                shoot.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
  
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
                
                
            }
            else
            { //attack code here
                animator.SetTrigger("isAttacking");
                GameObject attack = new GameObject();
                attack.AddComponent<Rigidbody>();
                attack.AddComponent<BoxCollider>();
                attack.GetComponent<BoxCollider>().isTrigger = true;
                attack.tag = "attack";
                attack.AddComponent<Collide>();
                attack.GetComponent<Collide>().damage = damage;
                attack.transform.position = Vector3.MoveTowards(transform.position, player.position, 100f * Time.deltaTime);

                Debug.Log("monster attack");
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked=false;
        attacking=false;
    }
     
    public void TakeDamage(float amount)
    {
        currentHealth-=amount;
        FlashRed();
        Debug.Log(currentHealth);
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            player.gameObject.GetComponent<PlayerStats>().AddXp(xp);
        }
    }
    void FlashRed()
    {
        render.material.color = Color.red;
        Invoke("ResetColor", flashTime);
    }
    void ResetColor()
    {
        render.material.color = origionalColor;
    }
}
