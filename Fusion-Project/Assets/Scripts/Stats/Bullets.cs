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

    void Update()
    {
        if(bulletsText != null)
            bulletsText.text = bullets.ToString();
    }

    public void RemoveBullet()
    {
        bullets--;
        if(bulletsText != null)
            bulletsText.text = bullets.ToString();
    }

    public void AddBullets(int amount)
    {
        bullets += amount;
        if (bullets > maxBullets)
            bullets = maxBullets;
        if (bulletsText != null)
            bulletsText.text = bullets.ToString();
    }
}
