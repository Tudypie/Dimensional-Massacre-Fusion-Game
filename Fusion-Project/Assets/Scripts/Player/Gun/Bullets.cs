using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bullets : MonoBehaviour
{
    public int bullets = 30;
    public int maxBullets = 999;
    public TMP_Text bulletsText;

    void Start()
    {
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
