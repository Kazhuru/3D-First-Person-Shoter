using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;

    private NavMeshAgent navMeshAgent;
    private EnemyAttack enemyAttack;
    private EnemyAnimationHandler animationHandler;

    private float distanceToTarget = Mathf.Infinity;
    private bool isProvoked = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        animationHandler = GetComponent<EnemyAnimationHandler>();
        GetComponent<EnemyHealth>().OnDamageTaken += EnemyAI_OnDamageTaken;
    }

    void Update()
    {
        if (target != null)
        {
            distanceToTarget = GetDistanceToTarget();
            if (isProvoked)
            {
                EngageTarget();
            }
            else if (distanceToTarget <= chaseRange)
            {
                isProvoked = true;
                EngageTarget();
            }
        } 
    }

    private void EngageTarget()
    {
        transform.LookAt(new Vector3(target.position.x, this.transform.position.y, target.position.z));
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
            
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }

    private void ChaseTarget()
    {
        animationHandler.SetAttackAnimation(false);
        animationHandler.StartMoveAnimation();
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        animationHandler.SetAttackAnimation(true);
    }

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(gameObject.transform.position, target.position);
    }

    public void StopMovement()
    {
        navMeshAgent.isStopped = true;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1 , 0.5f);
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void EnemyAI_OnDamageTaken()
    {
        isProvoked = true;
    }

    public Transform GetTarget() { return target;  }
}
