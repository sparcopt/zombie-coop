using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    private int currentWeaponIndex = 0;
    private Weapon primaryWeapon = Weapon.PortableMagnum;
    private Weapon secondaryWeapon = Weapon.Police9mm;
    private Weapon currentWeapon;
    private GameObject primaryWeaponObj;
    private GameObject secondaryWeaponObj;
    private GameObject currentWeaponObj;

    private void Awake()
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

    private void Start()
    {
        currentWeapon = secondaryWeapon;
        primaryWeaponObj = FindWeaponObject(primaryWeapon);
        secondaryWeaponObj = FindWeaponObject(secondaryWeapon);

        currentWeaponObj = secondaryWeaponObj;
        
        SelectCurrentWeapon();
    }

    private GameObject FindWeaponObject(Weapon weapon)
    {
        return transform.Find(weapon.ToString()).gameObject;
    }
    
    private void SelectCurrentWeapon()
    {
        currentWeaponObj.SetActive(true);
        currentWeaponObj.GetComponent<WeaponBase>().Select();
    }
    
    private void Update()
    {
        if (primaryWeapon != null && currentWeapon != primaryWeapon && Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = primaryWeapon;
            currentWeaponObj = primaryWeaponObj;
            
            secondaryWeaponObj.SetActive(false);
            SelectCurrentWeapon();
        }
        else if (secondaryWeapon != null && currentWeapon != secondaryWeapon && Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = secondaryWeapon;
            currentWeaponObj = secondaryWeaponObj;
            
            primaryWeaponObj.SetActive(false);
            SelectCurrentWeapon();
        }
    }
}

public enum Weapon
{
    Police9mm,
    PortableMagnum,
    Compact9mm
}
