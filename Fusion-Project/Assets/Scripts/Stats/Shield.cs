using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shield : MonoBehaviour
{
    public float damageReduction = 0.5f;
    public float shieldMaxHealth = 100f;
    public float shieldHealth = 100f;

    [SerializeField] private TMP_Text shieldText;

    private void Update()
    {
        if(shieldText != null)
            shieldText.text = Mathf.RoundToInt((shieldHealth / shieldMaxHealth) * 100) + "%";
    }

    public void DamageShield(float damage)
    {
        shieldHealth = Mathf.Clamp(shieldHealth - damage, 0, shieldMaxHealth);
    }

    public void GetShield(float amount)
    {
        shieldHealth = Mathf.Clamp(shieldHealth + amount, 0, shieldMaxHealth);
    }
    
}
