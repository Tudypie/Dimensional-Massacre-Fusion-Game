using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    enum PickupType
    {
        Health,
        Shield,
        Ammo
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float amount;
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (pickupType)
            {
                case PickupType.Health:
                    other.gameObject.GetComponent<Health>().GetHealth(amount);
                    AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.healthPickup);
                    break;
                case PickupType.Shield:
                    other.gameObject.GetComponent<Shield>().GetShield(amount);
                    AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.healthPickup);
                    break;
                case PickupType.Ammo:
                    other.gameObject.GetComponent<Bullets>().AddBullets((int)amount);     
                    AudioPlayer.Instance.PlayAudio(AudioPlayer.Instance.ammoPickup);            
                    break;
            }
            Destroy(gameObject);
        }
    }
}
