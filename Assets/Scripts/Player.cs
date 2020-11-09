using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Health health;
    [SerializeField] private Animator deathAnimator;
    [HideInInspector] public bool IsDead;

    void Start()
    {
        var inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        deathAnimator = inGameUITransform.Find("Death").GetComponent<Animator>();
        deathAnimator.SetTrigger("Reset");

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

            GameManager.Instance.GameOver();

            
            Invoke("RestartGame", 5);
        }
    }

    private void RestartGame()
    {
        GameManager.Instance.ResetGame();
        Destroy(gameObject);
    }
}
