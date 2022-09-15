using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RunAnimation()
    {
        animator.SetBool("idle", false);
        animator.SetBool("run", true);
    }

    public void IdleAnimation()
    {
        animator.SetBool("run", false);
        animator.SetBool("idle", true);
    }

    public void FireAnimation()
    {
        animator.SetBool("idle", false);
        animator.SetBool("run", false);

        animator.SetTrigger("fire");
    }

    internal void ReloadAnimation()
    {
        animator.SetBool("idle", false);
        animator.SetBool("run", false);

        animator.SetTrigger("reload");
    }
}
