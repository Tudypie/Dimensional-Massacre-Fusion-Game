using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bullets : MonoBehaviour
{
    public int bullets = 30;
    public int maxBullets = 999;

    [Header("ONLY FOR PLAYER")]
    public TMP_Text bulletsText;

    void Start()
    {
        bullets = PlayerPrefs.GetInt("Bullets");
        bulletsText.text = bullets.ToString();
    }

    public void RemoveBullet()
    {
        bullets--;
        bulletsText.text = bullets.ToString();
    }

    public void AddBullets(int amount)
    {
        bullets += amount;
        if (bullets > maxBullets)
            bullets = maxBullets;
        bulletsText.text = bullets.ToString();
    }
}
