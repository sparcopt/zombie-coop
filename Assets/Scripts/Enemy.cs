﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent agent;
    private Health health;
    private Animator animator;
    private Collider collider;
    [HideInInspector]
    private bool isAttacking;
    [HideInInspector]
    private bool isDead;
    public float Speed = 1.0f;
    public float AngularSpeed = 120;
    public float Damage = 20;

    void Start()
    {
        // TODO: this is heavy, look for a way of caching this
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();   
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        CheckHealth();
        Chase();
        CheckAttack();
    }

    private void CheckAttack()
    {
        if (isDead || isAttacking)
        {
            return;
        }

        var distanceFromTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distanceFromTarget <= 1.8f)
        {
            Attack();
        }
    }

    private void Attack()
    {
        var targetHealth = target.GetComponent<Health>();

        if (targetHealth == null)
        {
            throw new Exception("Target doesn't have health component");
        }

        targetHealth.TakeDamage(Damage);

        agent.speed = 0;
        agent.angularSpeed = 0;
        isAttacking = true;
        animator.SetTrigger("ShouldAttack");

        Invoke("ResetAttacking", 1.5f);
    }

    void ResetAttacking()
    {
        isAttacking = false;
        agent.speed = Speed;
        agent.angularSpeed = AngularSpeed;
    }

    private void Chase()
    {
        if (isDead)
        {
            return;
        }

        agent.destination = target.transform.position;
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
            agent.isStopped = true;
            collider.enabled = false;

            animator.CrossFadeInFixedTime("Death", 0.1f);
            Destroy(gameObject, 3f);
        }
    }
}
