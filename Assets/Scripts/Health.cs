﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthValue = 100f;
    public Health ParentRef;
    public float DamageMultiplier = 1.0f;

    public void TakeDamage(float damage)
    {
        damage *= DamageMultiplier;

        if(ParentRef != null)
        {
            ParentRef.TakeDamage(damage);
            return;
        }

        HealthValue -= damage;

        if (HealthValue < 0)
        {
            HealthValue = 0;
        }
    }
}
