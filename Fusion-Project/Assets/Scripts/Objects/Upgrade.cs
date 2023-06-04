using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private float speedBoost = 1.5f;
    [SerializeField] private float jumpBoost = 1.5f;
    [SerializeField] private float fireRateBoost = 0.5f;
    [SerializeField] private float damageBoost = 1.5f;

    [SerializeField] AudioClip pickupSound;
    [SerializeField] UnityEvent OnPickupEvent;
    
    private GameObject powerupText;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        powerupText = GameObject.Find("PowerupText");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {       
            AudioPlayer.Instance.PlayAudio(pickupSound);
            powerupText.GetComponent<Animator>().Play("PowerupText");
            OnPickupEvent?.Invoke();

            PlayerStats.Instance.playerMovement.RunSpeed *= speedBoost;
            PlayerStats.Instance.playerJump.jumpStrength *= jumpBoost;
            PlayerStats.Instance.topDownShotgun.FireRate *= fireRateBoost;
            PlayerStats.Instance.cameraShotgun.FireRate *= fireRateBoost;
            PlayerStats.Instance.topDownShotgun.Damage *= damageBoost;
            PlayerStats.Instance.cameraShotgun.Damage *= damageBoost;

            Destroy(gameObject);
        }

    }
}
