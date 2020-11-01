public class Police9mm : WeaponBase
{
    private const string weaponName = "Police 9mm";
    public override void PlayFireAnimation()
    {
        if(BulletsInClip > 1)
        {
            animator.CrossFadeInFixedTime("Fire", 0.1f);
        }
        else
        {
            animator.CrossFadeInFixedTime("FireLast", 0.1f);
        }
    }

    protected override string GetWeaponName()
    {
        return weaponName;
    }
}