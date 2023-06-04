using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{   
    [Header("Initial Stats")]
    [SerializeField] private float initialHealth = 100f;
    [SerializeField] private float initialShield = 0f;
    [SerializeField] private int initialBullets = 1;
    [SerializeField] private int initialGrenades = 0;

    [Header("References")]
    public Health playerHealth;
    public Shield playerShield;
    public Bullets playerBullets;
    public Grenades playerGrenades;
    public FirstPersonMovement playerMovement;
    public Jump playerJump;
    public FinalShotgun cameraShotgun;
    public FinalShotgun topDownShotgun;
    
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
            LoadStats();
        }
        else
        {
            LoadStats();
        }

    }

    public void NewGame() => PlayerPrefs.SetInt("NewGame", 0);

    public void SetInitialStats()
    {
        Debug.Log("Loaded Initial Stats");
        PlayerPrefs.SetFloat("Health", initialHealth);
        PlayerPrefs.SetFloat("Shield", initialShield);
        PlayerPrefs.SetInt("Bullets", initialBullets);
        PlayerPrefs.SetInt("Grenades", initialGrenades);
        PlayerPrefs.SetFloat("Speed", playerMovement.RunSpeed);
        PlayerPrefs.SetFloat("Jump", playerJump.jumpStrength);
        PlayerPrefs.SetFloat("FireRate", cameraShotgun.FireRate);
        PlayerPrefs.SetFloat("Damage", cameraShotgun.Damage);
        PlayerPrefs.SetInt("Kills", 0);
        PlayerPrefs.SetInt("Deaths", 0);
        PlayerPrefs.SetInt("NewGame", 1);
    }

    public void LoadStats()
    {
        Debug.Log("Loaded Stats");
        playerHealth.currentHp = PlayerPrefs.GetFloat("Health");
        playerShield.shieldHealth = PlayerPrefs.GetFloat("Shield");
        playerBullets.bullets = PlayerPrefs.GetInt("Bullets");
        playerGrenades.grenades = PlayerPrefs.GetInt("Grenades");
        playerMovement.RunSpeed = PlayerPrefs.GetFloat("Speed");
        playerJump.jumpStrength = PlayerPrefs.GetFloat("Jump");
        cameraShotgun.FireRate = PlayerPrefs.GetFloat("FireRate");
        topDownShotgun.FireRate = PlayerPrefs.GetFloat("FireRate");
        cameraShotgun.Damage = PlayerPrefs.GetFloat("Damage");
        topDownShotgun.Damage = PlayerPrefs.GetFloat("Damage");
        kills = PlayerPrefs.GetInt("Kills");
        deaths = PlayerPrefs.GetInt("Deaths");
    }
    
    //called whenever a scene is loaded from SceneLoader.cs
    public void SaveStats()
    {
        Debug.Log("Saved Stats");
        PlayerPrefs.SetFloat("Health", playerHealth.currentHp);
        PlayerPrefs.SetFloat("Shield", playerShield.shieldHealth);
        PlayerPrefs.SetInt("Bullets", playerBullets.bullets);
        PlayerPrefs.SetInt("Grenades", playerGrenades.grenades);
        PlayerPrefs.SetFloat("Speed", playerMovement.RunSpeed);
        PlayerPrefs.SetFloat("Jump", playerJump.jumpStrength);
        PlayerPrefs.SetFloat("FireRate", cameraShotgun.FireRate);
        PlayerPrefs.SetFloat("Damage", cameraShotgun.Damage);
        PlayerPrefs.SetInt("Kills", kills);
        PlayerPrefs.SetInt("Deaths", deaths);
    }

    public void AddKill() => kills++;
    public void AddDeath() => deaths++;
    public void AddWin() => PlayerPrefs.SetInt("TimesWon", PlayerPrefs.GetInt("TimesWon") + 1);
        

    
}
