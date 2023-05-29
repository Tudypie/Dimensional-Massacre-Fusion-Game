using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deaths : MonoBehaviour
{
    public static int deathsNumber;

    public void ResetStatsOnDeath()
    {
        PlayerPrefs.SetFloat("Health", 100f);
        PlayerPrefs.SetFloat("Shield", 0);
        PlayerPrefs.SetInt("Bullets", 1);
        PlayerPrefs.SetInt("Kills", 0);

        deathsNumber++;
        PlayerPrefs.SetInt("Deaths", deathsNumber);
    }
}
