using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletGridUI : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    public void CleanUpAllChildrens()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void InstanciateBulletsUI(Sprite bulletSprite, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject instanceBullet = Instantiate(bulletPrefab);
            instanceBullet.GetComponent<Image>().sprite = bulletSprite;
            instanceBullet.transform.SetParent(gameObject.transform);
        }
    }

    public void RemoveBulletFromUI()
    {
        if (transform.childCount > 0)
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);  
    }
}
