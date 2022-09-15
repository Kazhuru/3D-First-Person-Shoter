using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public static float damage = 40f;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] float attackSFXVolume;

    private EnemyIA enemyIA;
    private Player playerHealth;
    private AudioSource audioSource;

    void Start()
    {
        enemyIA = GetComponent<EnemyIA>();
        playerHealth = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    public void AttackHitEvent()
    {
        if(!Player.instance.IsDead())
        {
            audioSource.PlayOneShot(attackSFX, attackSFXVolume);
            playerHealth.ReduceHitPoints(damage);
        }
    }
}

