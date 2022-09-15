using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float startHitPoints;
    [SerializeField] AudioClip deadSFX;
    [SerializeField] float deadSFXVolume;

    public float currentHitPoints;
    public float GetCurrentHitPoints { get => currentHitPoints; }

    private EnemyAnimationHandler animationHandler;
    private AudioSource audioSource;


    private void Start()
    {
        animationHandler = GetComponent<EnemyAnimationHandler>();
        currentHitPoints = startHitPoints;
        audioSource = GetComponent<AudioSource>();
    }

    public void ReduceHealth(float incomingDamage)
    {
        this.OnDamageTaken?.Invoke();
        currentHitPoints -= incomingDamage;
        if (currentHitPoints <= 0)
            Dead();
    }

    public void Dead()
    {
        audioSource.PlayOneShot(deadSFX, deadSFXVolume);
        animationHandler.StartDeadAnimation();
        GetComponent<EnemyIA>().StopMovement();
        
    }

    public void AnimationEventDead()
    {
        FindObjectOfType<PlayerScore>().AddScoreEnemyDead();
        Destroy(this.gameObject, 0.1f);
    }

    public event Action OnDamageTaken;
}
