using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public abstract class WeaponBase : MonoBehaviour
{
    protected AudioSource audioSource;
    protected Animator animator;
    protected FirstPersonController controller;
    protected CashSystem CashSystem;
    protected bool fireLock = false;
    protected bool canShoot = false;
    protected bool isReloading = false;

    [Header("Object References")]
    public ParticleSystem MuzzleFlash;
    public Transform ShootPoint;
    public GameObject SparkPrefab;

    [Header("Sound References")]
    public AudioClip FireSound;
    public AudioClip DryFireSound;
    public AudioClip DrawSound;
    public AudioClip MagOutSound;
    public AudioClip MagInSound;
    public AudioClip BoltSound;

    [Header("UI References")]
    public Text WeaponNameText;
    public Text AmmoText;

    [Header("Weapon Attributes")]
    public FireMode FireMode = FireMode.FullAuto;
    public float Damage = 20f;
    public float FireRate = 1.0f;
    public int BulletsLeft;
    public int BulletsInClip;
    public int ClipSize = 12;
    public int MaxAmmo = 100;
    public GameObject BloodPrefab;
    public float Spread = 0.7f;
    public float Recoil = 0.5f;

    void Start()
    {
        var inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        WeaponNameText = inGameUITransform.Find("WeaponNameText").GetComponent<Text>();
        AmmoText = inGameUITransform.Find("AmmoText").GetComponent<Text>();
        
        var player = GameObject.FindGameObjectWithTag("Player"); 

        controller = player.GetComponent<FirstPersonController>();
        audioSource = GetComponent<AudioSource>();   
        animator = GetComponent<Animator>();
        CashSystem = player.GetComponent<CashSystem>();

        BulletsInClip = ClipSize;
        BulletsLeft = MaxAmmo;

        // Wait until weapon can fire (draw animation)
        Invoke("EnableWeapon", 1f);
    }

    public void UpdateTexts()
    {
        WeaponNameText.text = GetWeaponName();
        AmmoText.text = "Ammo: " + BulletsInClip + " / " + BulletsLeft;
    }

    protected abstract string GetWeaponName();

    private void EnableWeapon()
    {
        canShoot = true;
    }

    private void Update()
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

    public void Select()
    {
        isReloading = false;
        Invoke("UpdateTexts", Time.deltaTime);
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

    private void Fire()
    {
        audioSource.PlayOneShot(FireSound);
        fireLock = true;

        DetectHit();
        DoRecoil();

        MuzzleFlash.Stop();
        MuzzleFlash.Play();

        PlayFireAnimation();

        BulletsInClip--;
        UpdateTexts();

        StartCoroutine(ResetFireLock());
    }

    private void DoRecoil()
    {
        controller.MouseLook.Recoil(Recoil);
    }

    private void DetectHit()
    {
        RaycastHit hit;

        if (Physics.Raycast(ShootPoint.position, CalculateSpread(Spread, ShootPoint), out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                var targetHealth = hit.transform.GetComponent<Health>();

                if (targetHealth == null)
                {
                    throw new Exception("Cannot find health component on enemy");
                }

                targetHealth.TakeDamage(Damage);
                CreateBlood(hit.point, hit.transform.rotation);

                // Begin of Messy logic because of the head health
                Transform targetTransform;
                float targetHealthValue;

                if(targetHealth.ParentRef == null)
                {
                    targetTransform = hit.transform;
                    targetHealthValue = targetHealth.HealthValue;
                }
                else
                {
                    targetTransform = targetHealth.ParentRef.transform;
                    targetHealthValue = targetHealth.ParentRef.HealthValue;
                }
                // End of Messy logic because of the head health

                if(targetHealthValue <= 0)
                {
                    var killReward = targetTransform.GetComponent<KillReward>();
                    CashSystem.Cash += killReward.Amount;
                }
            }
            else
            {
                var spark = Instantiate(SparkPrefab, hit.point, hit.transform.rotation);
                Destroy(spark, 1);
            }
        }
    }

    private Vector3 CalculateSpread(float spread, Transform shootPoint)
    {
        return Vector3.Lerp(shootPoint.TransformDirection(Vector3.forward * 100), UnityEngine.Random.onUnitSphere, spread);
    }

    public void CreateBlood(Vector3 position, Quaternion rotation)
    {
        var blood = Instantiate(BloodPrefab, position, rotation);
        Destroy(blood, 1f);
    }

    public virtual void PlayFireAnimation()
    {
        animator.CrossFadeInFixedTime("Fire", 0.1f);
    }

    private void DryFire()
    {
        audioSource.PlayOneShot(DryFireSound);
        fireLock = true;

        StartCoroutine(ResetFireLock());
    }

    private void CheckReload()
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

    private void ReloadAmmo()
    {
        int bulletsToLoad = ClipSize - BulletsInClip;
        int bulletsToSubtract = (BulletsLeft >= bulletsToLoad) ? bulletsToLoad : BulletsLeft;

        BulletsLeft -= bulletsToSubtract;
        BulletsInClip += bulletsToSubtract;

        UpdateTexts();
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

    private void ResetIsReloading()
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
