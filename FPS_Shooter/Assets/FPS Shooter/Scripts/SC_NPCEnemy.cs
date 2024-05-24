using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class SC_NPCEnemy : MonoBehaviour, IEntity
{
    public float attackDistance = 3f;
    public float movementSpeed = 4f;
    public float npcHP = 100;
    //How much damage will npc deal to the player
    public float npcDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;


    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public SC_EnemySpawner es;
    private Animator anim;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    private bool attack;
    private bool death = false;
    private const string saveKey = "enemySave";

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;

        // stopgap measure: Set Rigidbody to Kinematic to prevent hit register bug
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        if (death)
            return;
        if (!attack)
            anim.SetBool("Mov", true);
        else
            anim.SetBool("Mov", false);

        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Attack
                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        attack = true;
                        anim.SetTrigger("Attack");
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                        IEntity player = hit.transform.GetComponent<IEntity>();
                        player.ApplyDamage(npcDamage);
                        attack = false;
                    }
                }
            }
        }
        agent.destination = playerTransform.position;
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
    }

    public void ApplyDamage(float points)
    {
        // avoiding repetition of animation
        if (death)
            return;
        npcHP -= points;
        if(npcHP <= 0)
        {

            death = true;
            movementSpeed = 0f;
            GetComponent<BoxCollider>().enabled = false;
            anim.SetTrigger("Death");
            anim.SetBool("Mov", false);
            es.EnemyEliminated(this);
            Destroy(gameObject, 10);
            es.enemydeathcounter += 1;
        }
    }
}