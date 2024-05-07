using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    [SerializeField] enemyController controller;
    [SerializeField] Enemy health;
    [SerializeField] Animator animator;
    // Start is called before the first frame update

    private void FixedUpdate()
    {
        animator.SetBool("Aiming", controller.GetPlayerDetected());
        animator.SetBool("Dead", health.dead);
        animator.SetBool("Walking", GetWalking());
    }

    bool GetWalking()
    {
        if (Vector3.Distance(controller.GetAgent().destination, controller.transform.position) > 2f)
        {
            return true;
        }
        return false;
    }
}
