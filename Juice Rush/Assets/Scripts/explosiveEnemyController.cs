using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class explosiveEnemyController : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //TODO: Area damage/TakeDamage
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
    private bool playerDetected = false;

    //Chase variables
    bool canChase = false;

    //Attack variables
    [SerializeField] float attackRange;
    [SerializeField] float stopRange;
    [SerializeField] float distanceToPlayer;
    [SerializeField] float aimSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float horizontalForce;
    [SerializeField] bool hasJumped;
    [SerializeField] bool grounded;
    //float jumpForceMin = 15f;
    //float jumpForceMax = 20f;
    //float horizontalForceMin = 15f;
    //float horizontalForceMax = 25f;

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
            if (IsPlayerDetected() || playerDetected)
            {
                playerDetected = true;
                Chase();

                if (distanceToPlayer <= attackRange && !hasJumped)
                {
                    agent.enabled = false;
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
        jumpForce = attackRange * 1.7f;//Random.Range(jumpForceMin, jumpForceMax);
        horizontalForce = attackRange * 2f;
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
    bool IsPlayerDetected()
    {
        return IsPlayerInView() && HasLineOfSight();
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
    bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        //LayerMasks which the raycast will ignore
        int bulletLayer = 1 << LayerMask.NameToLayer("Bullet");
        int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        int weaponLayer = 1 << LayerMask.NameToLayer("Weapon");
        int excludingLayerMask = ~(bulletLayer | enemyLayer | weaponLayer);
        //Raycasts towards the player, ignoring specified layers
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, maxDistance, excludingLayerMask))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                //Return true if there is like of sight to the player
                return true;
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