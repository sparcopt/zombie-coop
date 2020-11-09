using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Health health;
    public Text HealthText;
    public Animator HitAnimator;

    void Start()
    {
        var inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        HealthText = inGameUITransform.Find("Health").GetComponent<Text>();
        HitAnimator = inGameUITransform.Find("Hit").GetComponent<Animator>();
        
        health = GetComponent<Health>();
        health.OnHit.AddListener(() =>
        {
            HitAnimator.SetTrigger("Show");
            UpdateHealthText();
        });    

        UpdateHealthText(); 
    }

    private void UpdateHealthText()
    {
        HealthText.text = "HP: " + health.HealthValue;
    }
}
