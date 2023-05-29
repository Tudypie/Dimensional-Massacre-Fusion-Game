using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuitGame : MonoBehaviour
{   
    [SerializeField] private TMP_Text killText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text scoreText;
    private int kills;
    private float health;
    private float shield;
    private int ammo;
    private float totalScore;


    void Start()
    {
        kills = PlayerPrefs.GetInt("Kills");
        health = PlayerPrefs.GetFloat("Health");
        shield = PlayerPrefs.GetFloat("Shield");
        ammo = PlayerPrefs.GetInt("Bullets");

        killText.text = "Kills: " + kills.ToString();
        healthText.text = "Health: " + health.ToString();
        shieldText.text = "Shield: " + shield.ToString();
        ammoText.text = "Ammo: " + ammo.ToString();

        totalScore = (kills * 10) + (health * 10) + (shield * 10) + (ammo * 10);
        scoreText.text = totalScore.ToString();

        Invoke("Restart", 35f);
    }

    private void Restart()
    {   
        PlayerPrefs.DeleteAll();
        SceneLoader.Instance.LoadScene(0);
    }
}
