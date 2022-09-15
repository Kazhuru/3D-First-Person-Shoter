using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] Camera fpCamera;
    [SerializeField] float range;
    [SerializeField] float weaponDamage;
    [SerializeField] float weaponIntervalShoot = 0.25f;
    [SerializeField] Ammo weaponAmmo;
    [SerializeField] bool hasZoom = true;
    [SerializeField] int catridgeSize;
    [SerializeField] Image Reticle;
    [SerializeField] Color ReticleBaseColor = Color.white;
    [SerializeField] Color ReticleInRangeColor = Color.red;
    [SerializeField] ParticleSystem muzzleFlashVFX;
    [SerializeField] ParticleSystem shellsVFX;
    [SerializeField] GameObject hitImpactVFX;
    [SerializeField] AudioClip shotSound;
    [SerializeField] float shotSoundVolume;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] float reloadSoundVolume;
    [SerializeField] Player player;
    [SerializeField] Sprite ammoSprite;
    
    private AnimationHandler animationHandler;

    private bool outOfShootInterval = true;
    private int currentCatridgeAmount;

    public void Start()
    {
        animationHandler = GetComponent<AnimationHandler>();
        currentCatridgeAmount = catridgeSize;
        RefillWeaponGrid();
    }

    void OnEnable()
    {
        StartCoroutine(ShootInterval());
        RefillWeaponGrid();
    }

    void FixedUpdate()
    {
        CheckReticleColor();
    }

    public void Shoot()
    {
        if (weaponAmmo.currentAmmo > 0)
        {
            if (currentCatridgeAmount > 0)
            {
                if (outOfShootInterval)
                {
                    animationHandler.FireAnimation();
                    PlayShootEffects();
                    PlayShootSound();
                    ProcessFireRaycast();
                    ReduceCurrentAmmo();
                    StartCoroutine(ShootInterval());
                }
            }
            else
            {
                //Force stop run and zoom
                player.ForceStopForReload();
                Reload();
            }
        }
    }

    public void Reload()
    {
        if (!weaponAmmo.IsEmpty())
        {
            player.IsReloading = true;
            if (weaponAmmo.currentAmmo >= catridgeSize)
            {
                currentCatridgeAmount = catridgeSize;
            }
            else
            {
                currentCatridgeAmount = weaponAmmo.currentAmmo;
            }
            animationHandler.ReloadAnimation();
            PlayReloadSound();
        }
        else
        {
            //No ammo, maybe told the user
        }
    }

    public bool IsCatridgeFull()
    {
        return currentCatridgeAmount == catridgeSize;
    }

    public void AnimationEventEndReload()
    {
        player.GridBullets.CleanUpAllChildrens();
        RefillWeaponGrid();
        player.IsReloading = false;
    }

    private void ReduceCurrentAmmo()
    {
        currentCatridgeAmount--;
        player.GridBullets.RemoveBulletFromUI();
        weaponAmmo.ReduceAmmoAmount();
    }

    public void RefillWeaponGrid()
    {
        player.GridBullets.CleanUpAllChildrens();
        player.GridBullets.InstanciateBulletsUI(ammoSprite, currentCatridgeAmount);
    }

    private void PlayReloadSound()
    {
        if (reloadSound != null)
            player.GetAudioSource().PlayOneShot(reloadSound, reloadSoundVolume);
    }

    private void PlayShootEffects()
    {
        if (muzzleFlashVFX != null)
            muzzleFlashVFX.Play();
        if (shellsVFX != null)
            shellsVFX.Play();
    }

    private void PlayShootSound()
    {
        if (shotSound != null)
            player.GetAudioSource().PlayOneShot(shotSound, shotSoundVolume);
    }

    private void ProcessFireRaycast()
    {
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out RaycastHit hit, range))
        {
            CreateHitImpactEffect(hit);

            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.ReduceHealth(weaponDamage);
            }
        }
        else
            return;
    }

    private void CreateHitImpactEffect(RaycastHit objectHit)
    {
        if(hitImpactVFX == null) { return; }
        GameObject impact = Instantiate(hitImpactVFX, objectHit.point, Quaternion.LookRotation(objectHit.normal));
        Destroy(impact, 0.1f);
    }

    private void CheckReticleColor()
    {
        if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out RaycastHit objectInRange, range))
            if (objectInRange.transform.GetComponent<EnemyHealth>() != null)
                Reticle.color = ReticleInRangeColor;
            else
                Reticle.color = ReticleBaseColor;
        else
            Reticle.color = ReticleBaseColor;   
    }

    private IEnumerator ShootInterval()
    {
        outOfShootInterval = false;
        yield return new WaitForSeconds(weaponIntervalShoot);
        outOfShootInterval = true;
    }

    private void TestRaycastWeapon()
    {
        //Weapon Range Raycast for Testing:
        Debug.DrawRay(fpCamera.transform.position,
        fpCamera.transform.TransformDirection(Vector3.forward) * 100,
        Color.green);
    }

    public AnimationHandler GetAnimationHandler() { return animationHandler; }
    public Ammo GetWeaponAmmo() { return weaponAmmo; }
    public bool GetHasZoom() { return hasZoom; }

    public Sprite GetAmmoSprite() { return ammoSprite; }
    
    public int GetCurrentCatridgeAmount() { return currentCatridgeAmount; }
}
