using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Player")]
    public AudioClip playerDie;

    [Header("Gun")]
    public AudioClip shotgunShot;
    public AudioClip shotgunEquip;

    [Header("Monster")]
    public AudioClip monsterHit;
    public AudioClip monsterDeath;

    [Header("Pickups")]
    public AudioClip healthPickup;
    public AudioClip ammoPickup;

    [Header("Miscellaneous")]
    public AudioClip airRelease;
    public AudioClip explosion;
    

    public static AudioPlayer Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void PlayAudio(AudioClip clip, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }
    public void PlayAudio(AudioClip clip, Transform transform)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
    public void PlayAudio(AudioClip clip, Transform transform, float volume)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volume);
    }


    public void StopAudio()
    {
        audioSource.Stop();
    }

    
}
