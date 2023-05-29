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

    private void Start()
    {
        health = GetComponent<Health>();
        healthbarImage.color = Color.red;
    }

    private void Update()
    {
        healthbarImage.fillAmount = health.currentHp / health.totalHp;
    }   

    public void ChangeColorToBlue()
    {
        healthbarImage.color = Color.blue;
        shieldText.text = "Shield Health: " + GetComponent<Shield>().shieldHealth.ToString("F0");
    }
    public void ChangeColorToRed()
    {
        healthbarImage.color = Color.red;
        shieldText.text = "";
    }
}
