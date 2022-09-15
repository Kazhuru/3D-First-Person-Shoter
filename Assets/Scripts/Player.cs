using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Health Status")]
    [SerializeField] public float startingHP;
    [SerializeField] public float currentHP;
    [SerializeField] public TextMeshProUGUI textStartingHP;
    [SerializeField] public TextMeshProUGUI textCurrentHP;
    [SerializeField] public GameObject hpBar;
    [SerializeField] float hpReductionInterval = 0.1f;
    [SerializeField] public BulletGridUI GridBullets;

    private DeadHandler deadHandler;
    private WeaponZoom weaponZoom;
    private Weapon weapon;
    private WeaponSwitcher weaponSwitcher;
    private AudioSource audioSource;

    private bool isRunning = false;
    private bool isAiming = false;
    private bool isReloading = false;
    private bool isDead = false;
    private float HpBarStartWidth = 0f;
    private RectTransform HpBarRect;

    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHP = startingHP;
        deadHandler = GetComponent<DeadHandler>();
        weaponZoom = GetComponent<WeaponZoom>();
        weapon = FindObjectOfType<Weapon>();
        weaponSwitcher = FindObjectOfType<WeaponSwitcher>();
        weaponSwitcher.OnWeaponSwitch += Player_OnWeaponSwitch;
        audioSource = GetComponent<AudioSource>();
        SetupHitBarComponents();
    }

    private void SetupHitBarComponents()
    {
        HpBarRect = hpBar.GetComponent<RectTransform>();
        HpBarStartWidth = HpBarRect.rect.width;
        textStartingHP.text = startingHP.ToString();
        textCurrentHP.text = currentHP.ToString();
    }

    void FixedUpdate()
    {
        ProcessInputPlayerRunning();
        ProcessInputWeaponZoom();
    }

    void Update()
    {
        ProcessInputWeaponSwitch();
        ProcessInputWeaponFire();
        ProcessInputWeaponReload();
    }

    private void ProcessInputPlayerRunning()
    {
        if (!isDead && !isAiming && !isReloading)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                weapon.GetAnimationHandler().RunAnimation();
            }
            if (!Input.GetKey(KeyCode.LeftShift) && isRunning)
            {
                isRunning = false;
                weapon.GetAnimationHandler().IdleAnimation();
            }
        }
    }

    private void ProcessInputWeaponFire()
    {
        if (!isDead && !isRunning && !isReloading)
        {
            if (Input.GetMouseButton(0))
            {
                weapon.Shoot();
            }
        }
    }

    public void ForceStopForReload()
    {
        isAiming = false;
        weaponZoom.ZoomCameraOut();
        isRunning = false;
        weapon.GetAnimationHandler().IdleAnimation();
    }

    private void ProcessInputWeaponZoom()
    {
        if (!isDead && !isRunning && !isReloading)
        {
            if (weapon.GetHasZoom())
            {
                if (Input.GetMouseButton(1))
                {
                    isAiming = true;
                    weaponZoom.ZoomCameraIn();
                }
                if (!Input.GetMouseButton(1))
                {
                    isAiming = false;
                    weaponZoom.ZoomCameraOut();
                }
            }
        }
    }

    private void ProcessInputWeaponReload()
    {
        if (!isDead && !isRunning && !isAiming && !isReloading)
        {
            if (Input.GetKeyUp(KeyCode.R))
            {
                if(weapon.IsCatridgeFull() == false)
                    weapon.Reload();
            }
        }
    }

    private void ProcessInputWeaponSwitch()
    {
        if (!isDead && !isRunning && !isAiming & !isReloading)
        {
            int prevWeapon = weaponSwitcher.GetCurrentWeapon();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                weaponSwitcher.SetCurrentWeapon((int)WeaponType.Pistol);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                weaponSwitcher.SetCurrentWeapon((int)WeaponType.Shotgun);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                weaponSwitcher.SetCurrentWeapon((int)WeaponType.SubmachineMp5);
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
                weaponSwitcher.ChangeWeaponByWheel(Input.GetAxis("Mouse ScrollWheel"));

            weaponSwitcher.SetWeaponActive(prevWeapon);
        }
    }

    public void ReduceHitPoints(float damage)
    {
        currentHP -= damage;
        StartCoroutine(SmoothBarHpChange());
        if (currentHP > 0)
        {
            textCurrentHP.text = currentHP.ToString();
            Debug.Log("Player recive a hit of " + damage);
        }
        else
        {
            textCurrentHP.text = "0";
            PlayerDead();
        }
    }

    private IEnumerator SmoothBarHpChange()
    {
        bool onChange = true;
        float previousPercentage = currentHP + EnemyAttack.damage;

        while (onChange)
        {
            float resultWidht = (previousPercentage / startingHP) * HpBarStartWidth;
            HpBarRect.sizeDelta = new Vector2(resultWidht, HpBarRect.sizeDelta.y);

            if (previousPercentage <= currentHP)
                onChange = false;
            else
                previousPercentage -= 0.50f;

            yield return new WaitForSeconds(hpReductionInterval);
        }
    }

    public void PlayerDead()
    {
        isDead = true;
        deadHandler.HandleDead();
    }

    public void Player_OnWeaponSwitch()
    {
        weapon = FindObjectOfType<Weapon>();
    }

    public Weapon GetActiveWeapon() { return weapon; }

    public AudioSource GetAudioSource() { return audioSource; }

    public bool IsReloading { get => isReloading; set => isReloading = value; }

    public bool IsDead() { return isDead; }
}
