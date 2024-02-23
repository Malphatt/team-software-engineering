using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    //General variables
    private NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] float healthPoints;
    private bool isAlive;

    //Idle state variables
    [SerializeField] bool isIdle;

    //Chase state variables
    [SerializeField] float chaseSpeed;
    private bool playerDetected = false;

    //Patrol/PlayerSearch state variables
    [SerializeField] List<Transform> waypoints;
    [SerializeField] int currentWaypointIndex = 0;
    [SerializeField] float waypointTolerance;
    [SerializeField] List<int> searchWaypoints;
    [SerializeField] bool isSearching;

    //Player detection variables
    [SerializeField] float maxDistance;
    [SerializeField] float horizontalFOV;
    [SerializeField] float verticalFOV;

    //Attack variables
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;
    [SerializeField] float aimSpeed;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isAlive = true;
        isSearching = false;
    }

    void Update()
    {
        if (isAlive)
        {
            if (playerDetected || IsPlayerInView())
            {
                //Boolean to keep the enemy chasing after it is detected
                playerDetected = true;
                Chase();

            }
            else
            {
                //If enemy is not detected or is out of enemy's fov, patrol
                Patrol();
            }
        }
    }
    //
    void Idle()
    {
        //Stop the enemy agent
        agent.isStopped = true;
    }
    //Patrol state of the enemy
    void Patrol()
    {
        //Checks if the enemy has reached its current waypoint and is not currently calculating the path
        if (agent.remainingDistance <= waypointTolerance && !agent.pathPending)
        {
            //if the searchWaipoints int value matches patrol waypoint index, it will stop and search of the player
            if (searchWaypoints.Contains(currentWaypointIndex))
            {
                if (!isSearching)
                {
                    StartCoroutine(SearchPlayer());
                }
            }
            else
            {
                MoveToWaypoint();
            }
        }
    }
    //Will be changed as not behaving too well
    void Chase()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);
        //Change navmeshagent speed to chaseSpeed
        agent.speed = chaseSpeed;
        //Change agent destination to player position
        agent.destination = player.position;
        agent.isStopped = false;
    }

    void Attack()
    {

    }
    //will be redone
    IEnumerator SearchPlayer()
    {
        // Set the rotationDuration to manipulate the speed of the rotation 
        float rotationDuration = 3f;
        isSearching = true;
        agent.isStopped = true;

        Quaternion originalRotation = transform.rotation;
        Quaternion rightRotation = originalRotation * Quaternion.Euler(0, 60, 0);
        Quaternion leftRotation = rightRotation * Quaternion.Euler(0, -120, 0);

        //Rotate 60 degrees to the right
        for (float i = 0; i < 1; i += Time.deltaTime / rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(originalRotation, rightRotation, i);
            yield return null;
        }

        //Wait for 1 second
        yield return new WaitForSeconds(1f);

        //Rotate 120 degrees to the left from the new orientation
        for (float i = 0; i < 1; i += Time.deltaTime / rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(rightRotation, leftRotation, i);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        //Continue with normal behavior
        isSearching = false;
        agent.isStopped = false;
        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
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
        int bulletLayerMask = 1 << LayerMask.NameToLayer("Bullet");
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int weaponLayerMask = 1 << LayerMask.NameToLayer("Weapon");
        int layerMask = ~(bulletLayerMask | enemyLayerMask | weaponLayerMask);
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, maxDistance, layerMask))
        {
            return hit.transform == player;
        }
        return false;
    }
    //Visualization of the enemy POV
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

        //Draw lines to connect the ends of the rays, forming the edges of the view cone
        //This part is simplified; for a complete view cone, you'd need to calculate and connect all edge points
        Gizmos.DrawLine(origin, origin + horizontalLeftRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + horizontalRightRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + verticalUpRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + verticalDownRotation * transform.forward * maxDistance);
    }

}


