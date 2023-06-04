using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grenades : MonoBehaviour
{
    public int grenades = 3;
    public int maxGrenades = 999;

    [Header("ONLY FOR PLAYER")]
    public TMP_Text grenadesText;

    void Start()
    {
        if(grenadesText != null)
            grenadesText.text = grenades.ToString();
    }

    public void RemoveGrenade()
    {
        grenades--;
        if(grenadesText != null)
            grenadesText.text = grenades.ToString();
    }

    public void AddGrenades(int amount)
    {
        grenades += amount;
        if (grenades > maxGrenades)
            grenades = maxGrenades;
        if (grenadesText != null)
        grenadesText.text = grenades.ToString();
    }
}
