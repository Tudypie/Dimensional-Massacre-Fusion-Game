using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shield : MonoBehaviour
{
    public float damageReduction = 0.5f;
    public float shieldMaxHealth = 400f;
    public float shieldHealth = 100f;

    [Header("ONLY FOR PLAYER")]
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private Image shieldBar;

    private void Start()
    {
        shieldHealth = PlayerPrefs.GetFloat("Shield");
    }
    private void Update()
    {          
        if(shieldText != null)
            shieldText.text = shieldHealth.ToString("F0");

        if(shieldBar != null)
            shieldBar.fillAmount = shieldHealth / shieldMaxHealth;
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
