using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class explosiveEnemyController : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Explosive/Shotgun enemy AI scripts will be rewritten/fixed as I realized too late that NavMeshAgent blocks the manual rotation i.e LootAtPlayer, specifically the rotation up/down, which is crucial. Apologies for the delay.
    //TODO: Area damage/TakeDamage
    //IF this enemy is merged before being fixed. Set maxDistance, attackRange  to higher values and turn off the navmeshagent, to test functionality in stationary mode (it will give errors)
    //IF navmeshagent is not disables, it will chase/attack the player but will not rotate up/down nor be able to jump
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //General variables
    NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] float health;
    bool isAlive;
    Rigidbody rb;
    //Idle variables
    bool isIdle;

    //Patrolling variables
    int currentWaypointIndex = 0;
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float waypointTolerance;
    //AoE variables

    //Player detection variables
    [SerializeField] float maxDistance;
    [SerializeField] float horizontalFOV;
    [SerializeField] float verticalFOV;

    //Chase variables


    //Attack variables
    [SerializeField] float attackRange;
    [SerializeField] float stopRange;
    [SerializeField] float distanceToPlayer;
    [SerializeField] float aimSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float horizontalForce;
    [SerializeField] bool hasJumped;
    [SerializeField] bool grounded;
    float jumpForceMin = 10f;
    float jumpForceMax = 20f;
    float horizontalForceMin = 10f;
    float horizontalForceMax = 20f;

    //Explosion variabes
    [SerializeField] GameObject minionPrefab;
    int numberOfMinions;
    public bool canSpawnMinions = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasJumped)
        {
            Debug.Log("collided");
            Explode();
        }
    }

    void Start()
    {
        isAlive = true;
        hasJumped = false;
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isAlive)
        {
            if (IsPlayerInView())
            {
                Chase();

                if (distanceToPlayer <= attackRange && !hasJumped)
                {
                    Attack();
                    hasJumped = true; // Set the flag so it won't jump again
                }
            }
            else
            {
                Patrol();
            }
        }
    }


    void Patrol()
    {
        //Checks if the enemy has reached its current waypoint and is not currently calculating the path
        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            MoveToWaypoint();
        }
    }


    void MoveToWaypoint()
    {
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    void Chase()
    {
        agent.destination = player.position;
    }

    void Attack()
    {
        jumpForce = Random.Range(jumpForceMin, jumpForceMax);
        horizontalForce = Random.Range(horizontalForceMin, horizontalForceMax);
        Vector3 horizontalDirection = (player.position - transform.position).normalized;
        Vector3 force = horizontalDirection * horizontalForce + Vector3.up * jumpForce;
        rb.AddForce(force, ForceMode.Impulse);
    }
    //On explosion, there will be minions spawned that will have less hp but higher speed (currently in testing mode)
    void Explode()
    {
        numberOfMinions = Random.Range(2, 4);
        if (canSpawnMinions)
        {
            for (int i = 0; i < numberOfMinions; i++)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 5.0f;
                spawnPosition.y = 1f;

                GameObject minion = Instantiate(minionPrefab, spawnPosition, transform.rotation);
                minion.GetComponent<explosiveEnemyController>().canSpawnMinions = false;
                minion.GetComponent<explosiveEnemyController>().player = player;
            }
        }
        //room for animation/effect

        Destroy(gameObject);
    }
    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);
    }
    bool IsPlayerInView()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        //Horizontal FOV check
        float horizontalAngle = Vector3.Angle(transform.forward, directionToPlayer);
        if (horizontalAngle < horizontalFOV)
        {
            //Vertical FOV check
            Vector3 right = transform.right;
            Vector3 up = Vector3.Cross(right, directionToPlayer).normalized;
            Vector3 projectedDirectionToPlayer = Vector3.Cross(up, right).normalized;
            float verticalAngle = Vector3.Angle(transform.forward, projectedDirectionToPlayer);
            //Debug.Log(verticalAngle);
            if (verticalAngle < verticalFOV)
            {
                //Distance check
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer <= maxDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (!enabled) return;

        const int rayCount = 8; //Number of rays used to visualize each FOV plane. Increase for finer visualization.

        Vector3 origin = transform.position;
        Gizmos.color = Color.red;

        //Horizontal FOV visualization
        float halfHorizontalFov = horizontalFOV / 2.0f;
        Quaternion horizontalLeftRotation = Quaternion.AngleAxis(-halfHorizontalFov, Vector3.up);
        Quaternion horizontalRightRotation = Quaternion.AngleAxis(halfHorizontalFov, Vector3.up);

        //Vertical FOV visualization
        float halfVerticalFov = verticalFOV / 2.0f;
        Quaternion verticalDownRotation = Quaternion.AngleAxis(-halfVerticalFov, transform.right);
        Quaternion verticalUpRotation = Quaternion.AngleAxis(halfVerticalFov, transform.right);

        for (int i = 0; i <= rayCount; i++)
        {
            //Horizontal
            float horizontalAngle = i * (horizontalFOV / rayCount) - halfHorizontalFov;
            Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
            Vector3 horizontalDirection = horizontalRotation * transform.forward;

            //Vertical
            float verticalAngle = i * (verticalFOV / rayCount) - halfVerticalFov;
            Quaternion verticalRotation = Quaternion.AngleAxis(verticalAngle, transform.right);
            Vector3 verticalDirection = verticalRotation * transform.forward;

            //Draw horizontal rays
            Gizmos.DrawRay(origin, horizontalLeftRotation * horizontalDirection * maxDistance);
            Gizmos.DrawRay(origin, horizontalRightRotation * horizontalDirection * maxDistance);

            //Draw vertical rays
            Gizmos.DrawRay(origin, verticalDownRotation * verticalDirection * maxDistance);
            Gizmos.DrawRay(origin, verticalUpRotation * verticalDirection * maxDistance);
        }
        Gizmos.DrawLine(origin, origin + horizontalLeftRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + horizontalRightRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + verticalUpRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + verticalDownRotation * transform.forward * maxDistance);
    }
}
