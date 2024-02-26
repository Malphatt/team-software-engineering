using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //Explosive/Shotgun enemy AI scripts will be rewritten/fixed as I realized too late that NavMeshAgent blocks the manual rotation i.e LootAtPlayer, specifically the rotation up/down, which is crucial. Apologies for the delay.
    //TODO: TakeDamage/Fix issue mentioned abov
    //IF this enemy is merged before being fixed. Set maxDistanc and attackRange to higher values and turn off the navmeshagent, to test functionality in stationary mode (it will give errors)
    //IF navmeshagent is not disabled, it will chase/attack the player but will not rotate up/down
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //General variables
    private NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] float healthPoints;
    private bool isAlive;
    Coroutine searchCoroutine;

    //Idle state variables
    [SerializeField] bool isIdle;

    //Chase state variables
    [SerializeField] float chaseSpeed;
    private bool playerDetected = false;

    //Patrol/PlayerSearch state variables
    [SerializeField] List<Transform> waypoints;
    int currentWaypointIndex = 0; 
    [SerializeField] float waypointTolerance;
    [SerializeField] List<int> searchWaypoints;
    [SerializeField] bool isSearching;

    //Player detection variables
    [SerializeField] float maxDistance;
    [SerializeField] float horizontalFOV;
    [SerializeField] float verticalFOV;

    //Attack variables
    [SerializeField] float attackRange;
    [SerializeField] float stopRange;
    [SerializeField] float aimSpeed;
    [SerializeField] float distanceToPlayer;
    [SerializeField] enemyShotgun shotgunScript;
    [SerializeField] GameObject shotgunObj;
    
    //Take damage variables


    void Start()
    {
        shotgunScript = shotgunObj.GetComponent<enemyShotgun>();
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false; //Wouldn't disable it
        isAlive = true;
        isSearching = false;
    }

    void Update()
    {
        //Get distance from enemy to player
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isAlive)
        {
            //Check if the player is detected or within view
            if (playerDetected || IsPlayerDetected())
            {
                playerDetected = true; //Bool set to true, to continously chase/attack the player regardless of whether its in FOV
                //If in attack range, attack
                if (distanceToPlayer <= stopRange)
                {
                    MaintainDistance();
                    if (distanceToPlayer <= attackRange)
                    {
                        Attack();

                    }
                }
                    //If in stopping range stop moving
                else
                {
                    agent.isStopped = false;
                    Chase();
                }
            }
            else
            {
                //If player is not detected, continue patrolling
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
        if (!agent.pathPending && agent.remainingDistance <= waypointTolerance)
        {
            //if the searchWaipoints int value matches patrol waypoint index, it will stop and search of the player
            if (searchWaypoints.Contains(currentWaypointIndex))
            {
                if (!isSearching)
                {
                    if (searchCoroutine != null)
                    {
                        StopCoroutine(searchCoroutine);
                    }
                    searchCoroutine = StartCoroutine(SearchPlayer());
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
        //Rotates the enemy towards the player
        LookAtPlayer();

        //Is the coroutine is running, stop the coroutine
        if (searchCoroutine != null)
        {
            StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }
        //Set navmeshagent speed to chaseSpeed
        agent.speed = chaseSpeed;
        //Set agent destination to player position
        agent.destination = player.position;
        agent.isStopped = false;
    }

    void Attack()
    {
        LookAtPlayer();
        if (searchCoroutine != null)
        {
            StopCoroutine(searchCoroutine);
            searchCoroutine = null;
        }
        //Triggers the shootgun firing method
        shotgunScript.FireShotgun();
    }
    //Maintain the distance of an enemy towards the player
    void MaintainDistance()
    {
        if (distanceToPlayer < stopRange * 0.80)
        {
            Vector3 awayFromPlayer = transform.position - player.position;
            
            Vector3 position = player.position + awayFromPlayer * stopRange;
            agent.SetDestination(position);
        }
    }
    //Coroutine for rotating one place on a SearchWaypoint
    IEnumerator SearchPlayer()
    {
        // Set the rotationDuration to manipulate the speed of the rotation 
        float rotationDuration = 3f;
        isSearching = true;
        agent.isStopped = true;


        Quaternion originalRotation = transform.rotation; //Original rotation
        Quaternion rightRotation = originalRotation * Quaternion.Euler(0, 60, 0);//Set the rotation towards right to 60 degrees
        Quaternion leftRotation = rightRotation * Quaternion.Euler(0, -120, 0);//Set the rotation towards left to -120 degrees

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
        //Sets the destination to the current wapoint
        agent.destination = waypoints[currentWaypointIndex].position;
        //Increments the waypoint index and resets back to 0 when the list of waypoints if finished
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    void LookAtPlayer()
    {
        //Get the direction to player
        Vector3 direction = (player.position - transform.position);
        Debug.Log("Direction to Player: " + direction); //Testing
        //Determines the rotation towards the player 
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        //Rotates towards the player using Slerp method to make it more smooth
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);
    }

    bool IsPlayerDetected()
    {
        return IsPlayerInView() && HasLineOfSight();
    }
    //Method to check if the player is in view
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
    //Method to check if there is a direct line of sight towards the player
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
        Gizmos.DrawLine(origin, origin + horizontalLeftRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + horizontalRightRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + verticalUpRotation * transform.forward * maxDistance);
        Gizmos.DrawLine(origin, origin + verticalDownRotation * transform.forward * maxDistance);
    }

}


