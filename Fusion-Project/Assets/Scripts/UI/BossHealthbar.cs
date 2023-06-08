using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Health))]
public class BossHealthbar : MonoBehaviour
{   
    [SerializeField] private Image healthbarImage;
    [SerializeField] private TMP_Text shieldText;
    private Health health;
    private Shield shield;

    private void Start()
    {
        health = GetComponent<Health>();
        shield = GetComponent<Shield>();
    }

    private void Update()
    {
        healthbarImage.fillAmount = health.currentHp / health.totalHp;
        if(shield.enabled)
            shieldText.text = "Shield Active: " + GetComponent<Shield>().shieldHealth.ToString("F0");
    }   

    public void StartShieldPhase()
    {
        healthbarImage.color = Color.blue;
        shieldText.text = "Shield Active: " + GetComponent<Shield>().shieldHealth.ToString("F0");
    }
    public void StopShieldPhase()
    {
        healthbarImage.color = Color.red;
        shieldText.text = "";
    }
}
