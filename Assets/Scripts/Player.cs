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
    public bool IsDead;

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
        if (IsDead)
        {
            return;
        }

        if (health.HealthValue <= 0)
        {
            IsDead = true;

            deathAnimator.SetTrigger("Show");

            Invoke("RestartGame", 2f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
