using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupAmmo : MonoBehaviour
{
    [SerializeField] AmmoType ammoType;
    [SerializeField] int amountOfAmmo;
    [SerializeField] AudioClip pickupClip;
    [SerializeField] float pickupClipVolume;
    [SerializeField] Animator floatingTextAnim;

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            WeaponSwitcher weaponSwitcher = FindObjectOfType<WeaponSwitcher>();
            foreach (Transform child in weaponSwitcher.transform)
            {
                Ammo weaponAmmo = child.GetComponent<Weapon>().GetWeaponAmmo();
                if(ammoType == weaponAmmo.ammoType)
                    weaponAmmo.IncreaseAmmoAmount(amountOfAmmo);
            }
            Player.instance.GetAudioSource().PlayOneShot(pickupClip, pickupClipVolume);
            floatingTextAnim.GetComponent<Text>().text =
                "+" + amountOfAmmo + " " + ammoType.ToString() + " picked up!";
            floatingTextAnim.SetTrigger("startFloat");
            DeletePickup();
        }
    }

    private void DeletePickup()
    {
        Destroy(gameObject);
    }
}
