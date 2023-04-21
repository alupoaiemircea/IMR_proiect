using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovesNetwork : MonoBehaviour
{
    NeuralNetwork net;
    int[] layers = new int[3] { 8, 5, 1 };
    string[] activation = new string[2] { "sigmoid", "sigmoid" };

    public GameObject player;
    float multiplier = 200;
    float multiplierHealth = 20;
    float multiplierOutput = 10;
    public float timeRemaining = 0.5f;

    private NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking 
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public bool attacking = false;

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
       
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;

        render = gameObject.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        origionalColor = render.material.color;
    }

    void Start()
    {
        this.net = new NeuralNetwork(layers, activation);
        net.Load("network_brain.txt");
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            float myX = gameObject.transform.position.x / multiplier;
        float myY = gameObject.transform.position.x / multiplier;
        float myZ = gameObject.transform.position.x / multiplier;

        float playerX = player.transform.position.x / multiplier;
        float playerY = player.transform.position.x / multiplier;
        float playerZ = player.transform.position.x / multiplier;

        float playerHealth = player.GetComponent<PlayerStats>().currentHealth / multiplierHealth;
        float playerAttack;
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            playerAttack = 1f;
        else
            playerAttack = 0f;
        float result = net.FeedForward(new float[] { myX, myY, myZ, playerX, playerY, playerZ, playerHealth, playerAttack })[0];
        int action = (int)System.Math.Round(result*10, 0);
            Debug.Log(action);
        switch (action)
        {
            case 1:
                Patrolling();
                break;
            case 2:
                ChasePlayer();
                break;
            case 3:
                AttackPlayer();
                break;
            case 4:
                //call allies
                break;
            case 5:
                //dodge
                break;
            default:
                break;
        }
      }
    }
    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        if (gameObject.tag == "boss")
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

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }
    private void AttackPlayer()
    {
        //make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform);

        if (!alreadyAttacked)
        {
            //attack code here
            animator.SetTrigger("isAttacking");
            GameObject attack = new GameObject();
            attack.AddComponent<Rigidbody>();
            attack.AddComponent<BoxCollider>();
            attack.tag = "attack";
            attack.AddComponent<Collide>();
            attack.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 100f * Time.deltaTime);

            Debug.Log("monster attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        attacking = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        FlashRed();
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            player.GetComponent<PlayerStats>().AddXp(xp);
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
