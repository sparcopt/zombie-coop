using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;
    private bool fireLock = false;
    private bool canShoot = false;

    [Header("Object References")]
    public ParticleSystem MuzzleFlash;

    [Header("Sound References")]
    public AudioClip FireSound;
    public AudioClip DryFireSound;

    [Header("Weapon Attributes")]
    public FireMode FireMode = FireMode.FullAuto;
    public float Damage = 20f;
    public float FireRate = 1.0f;
    public int BulletsLeft = 100;
    public int BulletsInClip = 12;

    void Start()
    {
         audioSource = GetComponent<AudioSource>();   
         animator = GetComponent<Animator>();

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
    }

    private void CheckFire()
    {
        if(!canShoot || fireLock)
        {
            return;
        }

        audioSource.PlayOneShot(FireSound);
        fireLock = true;

        MuzzleFlash.Stop();
        MuzzleFlash.Play();

        animator.CrossFadeInFixedTime("Fire", 0.1f);

        StartCoroutine(ResetFireLock());
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
