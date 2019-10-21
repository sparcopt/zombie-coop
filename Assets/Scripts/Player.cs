using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Health health;
    [SerializeField]
    private Animator deathAnimator;
    [HideInInspector]
    private bool isDead;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (isDead)
        {
            return;
        }

        if (health.HealthValue <= 0)
        {
            isDead = true;

            deathAnimator.SetTrigger("Show");

            Invoke("RestartGame", 2f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
