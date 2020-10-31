﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected AudioSource audioSource;
    protected Animator animator;
    protected bool fireLock = false;
    protected bool canShoot = false;
    protected bool isReloading = false;

    [Header("Object References")]
    public ParticleSystem MuzzleFlash;
    public Transform ShootPoint;

    [Header("Sound References")]
    public AudioClip FireSound;
    public AudioClip DryFireSound;
    public AudioClip DrawSound;
    public AudioClip MagOutSound;
    public AudioClip MagInSound;
    public AudioClip BoltSound;

    [Header("Weapon Attributes")]
    public FireMode FireMode = FireMode.FullAuto;
    public float Damage = 20f;
    public float FireRate = 1.0f;
    public int BulletsLeft;
    public int BulletsInClip;
    public int ClipSize = 12;
    public int MaxAmmo = 100;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
        animator = GetComponent<Animator>();

        BulletsInClip = ClipSize;
        BulletsLeft = MaxAmmo;

        // Wait until weapon can fire (draw animation)
        Invoke("EnableWeapon", 1f);
    }

    void EnableWeapon()
    {
        canShoot = true;
    }

    void Update()
    {
        if(FireMode == FireMode.FullAuto && Input.GetButton("Fire1"))
        {
            CheckFire();
        }
        else if(FireMode == FireMode.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            CheckFire();
        }

        if(Input.GetButtonDown("Reload"))
        {
            CheckReload();
        }
    }

    private void CheckFire()
    {
        if(!canShoot || isReloading || fireLock)
        {
            return;
        }

        if(BulletsInClip > 0)
        {
            Fire();
        }
        else
        {
            DryFire();
        }
    }

    void Fire()
    {
        audioSource.PlayOneShot(FireSound);
        fireLock = true;

        DetectHit();

        MuzzleFlash.Stop();
        MuzzleFlash.Play();

        PlayFireAnimation();

        BulletsInClip--;

        StartCoroutine(ResetFireLock());
    }

    private void DetectHit()
    {
        RaycastHit hit;

        if (Physics.Raycast(ShootPoint.position, ShootPoint.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                var health = hit.transform.GetComponent<Health>(); //todo: should be placed inside enemy.TakeDamage()
                var enemy = hit.transform.GetComponent<Enemy>();

                if (health == null)
                {
                    throw new Exception("Cannot find health component on enemy");
                }

                if (enemy == null)
                {
                    throw new Exception("Cannot find enemy component on enemy");
                }

                health.TakeDamage(Damage);
                enemy.CreateBlood(hit.point, hit.transform.rotation);
            }
        }
    }

    public virtual void PlayFireAnimation()
    {
        animator.CrossFadeInFixedTime("Fire", 0.1f);
    }

    void DryFire()
    {
        audioSource.PlayOneShot(DryFireSound);
        fireLock = true;

        StartCoroutine(ResetFireLock());
    }

    void CheckReload()
    {
        if(BulletsLeft > 0 && BulletsInClip < ClipSize)
        {
            Reload();
        }
    }

    private void Reload()
    {
        if(isReloading)
        {
            return;
        }
        
        isReloading = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
    }

    void ReloadAmmo()
    {
        int bulletsToLoad = ClipSize - BulletsInClip;
        int bulletsToSubtract = (BulletsLeft >= bulletsToLoad) ? bulletsToLoad : BulletsLeft;

        BulletsLeft -= bulletsToSubtract;
        BulletsInClip += bulletsToSubtract;
    }

    public void OnDraw()
    {
        audioSource.PlayOneShot(DrawSound);
    }

    public void OnMagOut()
    {
        audioSource.PlayOneShot(MagOutSound);
    }

    public void OnMagIn()
    {
        ReloadAmmo();
        audioSource.PlayOneShot(MagInSound);
    }

    public void OnBoltForwarded()
    {
        audioSource.PlayOneShot(BoltSound);
        Invoke("ResetIsReloading", 1f);
    }

    void ResetIsReloading()
    {
        isReloading = false;
    }

    IEnumerator ResetFireLock()
    {
        yield return new WaitForSeconds(FireRate);
        fireLock = false;
    }
}

public enum FireMode
{
    SemiAuto,
    FullAuto
}
