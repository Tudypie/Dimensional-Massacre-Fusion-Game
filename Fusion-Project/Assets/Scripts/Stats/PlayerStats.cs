using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{   
    [Header("Initial Stats")]
    [SerializeField] private float initialHealth = 100f;
    [SerializeField] private float initialShield = 0f;
    [SerializeField] private int initialBullets = 1;

    [Header("References")]
    [SerializeField] private Health playerHealth;
    [SerializeField] private Shield playerShield;
    [SerializeField] private Bullets playerBullets;
    
    public static int kills;
    public static int deaths;

    public static PlayerStats Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if(PlayerPrefs.GetInt("NewGame") == 0)
        {
            SetInitialStats();
        }
        else
        {
            LoadStats();
        }

    }

    public void NewGame() => PlayerPrefs.SetInt("NewGame", 0);

    public void SetInitialStats()
    {
        PlayerPrefs.SetFloat("Health", initialHealth);
        PlayerPrefs.SetFloat("Shield", initialShield);
        PlayerPrefs.SetInt("Bullets", initialBullets);
        PlayerPrefs.SetInt("Kills", 0);
        PlayerPrefs.SetInt("Deaths", 0);
        PlayerPrefs.SetInt("NewGame", 1);
    }

    public void LoadStats()
    {
        playerHealth.currentHp = PlayerPrefs.GetFloat("Health");
        playerShield.shieldHealth = PlayerPrefs.GetFloat("Shield");
        playerBullets.bullets = PlayerPrefs.GetInt("Bullets");
        kills = PlayerPrefs.GetInt("Kills");
        deaths = PlayerPrefs.GetInt("Deaths");
    }
    
    //called whenever a scene is loaded from SceneLoader.cs
    public void SaveStats()
    {
        PlayerPrefs.SetFloat("Health", playerHealth.currentHp);
        PlayerPrefs.SetFloat("Shield", playerShield.shieldHealth);
        PlayerPrefs.SetInt("Bullets", playerBullets.bullets);
        PlayerPrefs.SetInt("Kills", kills);
        PlayerPrefs.SetInt("Deaths", deaths);
    }

    public void AddKill() => kills++;
    public void AddDeath() => deaths++;
    public void AddWin() => PlayerPrefs.SetInt("TimesWon", PlayerPrefs.GetInt("TimesWon") + 1);
        

    
}
