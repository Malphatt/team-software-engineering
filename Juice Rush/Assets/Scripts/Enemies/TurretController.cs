using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    //Rotation variables
    [SerializeField] float rotationDuration;
    [SerializeField] float waitTime;
    bool isSearching;

    //Detection variables
    [SerializeField] float maxDistance;
    [SerializeField] float horizontalFOV;
    [SerializeField] float verticalFOV;
    [SerializeField] Enemy Turret;
    [SerializeField] Transform player;

    //Attack variables
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform gunMuzzle;
    [SerializeField] float fireRate;
    float fireTime;
    [SerializeField] float aimSpeed;
    [SerializeField] float accuracy;
    [SerializeField] float shotForce;

    Coroutine rotateCoroutine;

    void Start()
    {
        rotateCoroutine = StartCoroutine(RotateTurretHead());
        player = Turret.Player.transform;
    }

    void Update()
    {
        if (IsPlayerDetected())
        {
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine); //Stop rotating if player is detected
                rotateCoroutine = null; //Clear the reference
            }
            isSearching = false;
            Attack();
        }
        else
        {
            if (rotateCoroutine == null) //Only start coroutine if it's not already running
            {
                rotateCoroutine = StartCoroutine(RotateTurretHead());
            }
            isSearching = true;
        }
    }
    void Attack()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);

        if (Time.time > fireTime)
        {
            GameObject projectile = Instantiate(projectilePrefab, gunMuzzle.position, gunMuzzle.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            float spreadAngleX = Random.Range(-accuracy, accuracy);
            float spreadAngleY = Random.Range(-accuracy, accuracy);

            Vector3 spreadShotDirection = Quaternion.Euler(spreadAngleX, spreadAngleY, 0) * gunMuzzle.forward;

            rb.AddForce(spreadShotDirection * shotForce, ForceMode.Impulse);
            fireTime = Time.time + fireRate;
        }
    }


    IEnumerator RotateTurretHead()
    {
        Quaternion originalRotation = transform.rotation;
        Quaternion rightRotation = originalRotation * Quaternion.Euler(0, 60, 0);
        Quaternion leftRotation = originalRotation * Quaternion.Euler(0, -60, 0);

        while (isSearching)
        {
            //Rotate 60 degrees to the right
            for (float t = 0; t < 1; t += Time.deltaTime / rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(originalRotation, rightRotation, t);
                yield return null;
            }

            //Wait for waitTime seconds
            yield return new WaitForSeconds(waitTime);

            //Rotate turret back to its initial rotation from the right
            for (float t = 0; t < 1; t += Time.deltaTime / rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(rightRotation, originalRotation, t);
                yield return null;
            }

            //Rotate 60 degrees to the left from the original orientation
            for (float t = 0; t < 1; t += Time.deltaTime / rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(originalRotation, leftRotation, t);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            //Rotate turret back to its initial rotation from the left
            for (float t = 0; t < 1; t += Time.deltaTime / rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(leftRotation, originalRotation, t);
                yield return null;
            }

        }
        //Once the loop ends
        rotateCoroutine = null;
    }

    bool IsPlayerDetected()
    {
        return IsPlayerInView() && HasLineOfSight();
    }

    bool IsPlayerInView()
    {
        Vector3 directionToPlayer = (player.position - gunMuzzle.position).normalized;

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
        Vector3 directionToPlayer = (player.position - gunMuzzle.position).normalized;
        //
        int bulletLayerMask = 1 << LayerMask.NameToLayer("Bullet");
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int hitboxLayerMask = 1 << LayerMask.NameToLayer("Hitbox");
        int layerMask = ~(bulletLayerMask | enemyLayerMask | hitboxLayerMask);
        if (Physics.Raycast(gunMuzzle.position, directionToPlayer, out hit, maxDistance, layerMask))
        {
            return hit.transform == player;
        }
        return false;
    }
    void OnDrawGizmos()
    {
        const int rayCount = 8; //Number of rays used to visualize each FOV

        Vector3 origin = transform.position;
        Gizmos.color = Color.red;

        // Horizontal FOV visualization
        float halfHorizontalFov = horizontalFOV / 2.0f;
        Quaternion horizontalLeftRotation = Quaternion.AngleAxis(-halfHorizontalFov, Vector3.up);
        Quaternion horizontalRightRotation = Quaternion.AngleAxis(halfHorizontalFov, Vector3.up);

        // Vertical FOV visualization
        float halfVerticalFov = verticalFOV / 2.0f;
        Quaternion verticalDownRotation = Quaternion.AngleAxis(-halfVerticalFov, transform.right);
        Quaternion verticalUpRotation = Quaternion.AngleAxis(halfVerticalFov, transform.right);

        for (int i = 0; i <= rayCount; i++)
        {
            // Horizontal
            float horizontalAngle = i * (horizontalFOV / rayCount) - halfHorizontalFov;
            Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
            Vector3 horizontalDirection = horizontalRotation * transform.forward;

            // Vertical
            float verticalAngle = i * (verticalFOV / rayCount) - halfVerticalFov;
            Quaternion verticalRotation = Quaternion.AngleAxis(verticalAngle, transform.right);
            Vector3 verticalDirection = verticalRotation * transform.forward;

            // Draw horizontal rays
            Gizmos.DrawRay(origin, horizontalLeftRotation * horizontalDirection * maxDistance);
            Gizmos.DrawRay(origin, horizontalRightRotation * horizontalDirection * maxDistance);

            // Draw vertical rays
            Gizmos.DrawRay(origin, verticalDownRotation * verticalDirection * maxDistance);
            Gizmos.DrawRay(origin, verticalUpRotation * verticalDirection * maxDistance);
        }
    }
}
