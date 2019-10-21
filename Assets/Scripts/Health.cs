using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float HealthValue = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        HealthValue -= damage;

        if (HealthValue < 0)
        {
            HealthValue = 0;
        }
    }
}
