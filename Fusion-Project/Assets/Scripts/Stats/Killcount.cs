using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Killcount : MonoBehaviour
{
    public static int kills;

    [SerializeField] private TMP_Text killText;

    public static Killcount Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Invoke("LoadKills", 0.5f);
    }

    private void LoadKills()
    {
        kills = PlayerPrefs.GetInt("Kills");
        killText.text = kills.ToString();
    }

    public void AddKill()
    {
        kills++;
        killText.text = kills.ToString();
        PlayerPrefs.SetInt("Kills", kills);
    }
}
