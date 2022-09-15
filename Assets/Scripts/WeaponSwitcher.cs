using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] int currentWeaponId = 0;
    [SerializeField] TextMeshProUGUI weaponInfoUI;

    private Weapon currentWeapon;
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
        foreach (Transform child in this.transform)
            child.GetComponent<Weapon>().GetWeaponAmmo().Init();
    }

    void Update()
    {
        if (weaponInfoUI != null && player != null)
        {
            weaponInfoUI.text = player.GetActiveWeapon().GetWeaponAmmo().currentAmmo.ToString();
        }
    }

    public void SetWeaponActive(int prevWeapon)
    {
        if (prevWeapon != currentWeaponId)
        {
            int weaponIndex = 0;
            foreach (Transform weapon in this.transform)
            {
                if (weaponIndex == currentWeaponId)
                {
                    weapon.gameObject.SetActive(true);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
                weaponIndex++;
            }

            this.OnWeaponSwitch?.Invoke();
        }
    }

    public void SetCurrentWeapon(int changeWeapon)
    {
        if (changeWeapon >= 0 && changeWeapon <= (transform.childCount - 1))
        {
            currentWeaponId = changeWeapon;
        }
    }

    public int GetCurrentWeapon() { return currentWeaponId; }

    public void ChangeWeaponByWheel(float wheelValue)
    {
        
        if (wheelValue < 0f)
        {
            if (currentWeaponId >= transform.childCount - 1)
                currentWeaponId = 0;
            else
                currentWeaponId++;
        }
        if (wheelValue > 0f)
        {
            if (currentWeaponId <= 0)
                currentWeaponId = transform.childCount - 1;
            else
                currentWeaponId--;
        }
    }

    public event Action OnWeaponSwitch;
}
