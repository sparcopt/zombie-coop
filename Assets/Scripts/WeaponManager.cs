using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    private int currentWeaponIndex = 0;
    private Weapon[] weapons = { Weapon.Police9mm, Weapon.PortableMagnum };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SwitchToCurrentWeapon();
    }

    void Update()
    {
        CheckWeaponSwitch();
    }

    void SwitchToCurrentWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.Find(weapons[currentWeaponIndex].ToString()).gameObject.SetActive(true);
    }

    private void CheckWeaponSwitch()
    {
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        // Forward
        if (mouseWheel > 0)
        {
            SelectPreviousWeapon();
        }
        else if (mouseWheel < 0)
        {
            SelectNextWeapon();
        }
    }

    private void SelectPreviousWeapon()
    {
        if (currentWeaponIndex == 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        else
        {
            currentWeaponIndex--;
        }

        SwitchToCurrentWeapon();
    }

    private void SelectNextWeapon()
    {
        if (currentWeaponIndex >= (weapons.Length - 1))
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex++;
        }

        SwitchToCurrentWeapon();
    }
}

public enum Weapon
{
    Police9mm,
    PortableMagnum
}
