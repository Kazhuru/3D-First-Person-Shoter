using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAttackAnimation(bool state)
    {
        animator.SetBool("attack", state);
    }

    public void StartMoveAnimation()
    {
        animator.SetTrigger("move");
    }

    public void StartHitAnimation()
    {
        animator.SetTrigger("hit");
    }

    public void StartDeadAnimation()
    {
        animator.SetTrigger("dead");
    }
}
