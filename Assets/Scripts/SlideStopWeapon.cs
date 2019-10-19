using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideStopWeapon : WeaponBase
{
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
}
