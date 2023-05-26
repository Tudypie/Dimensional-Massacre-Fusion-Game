using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Gun")]
    public AudioClip shotgunShot;

    [Header("Monster")]
    public AudioClip monsterLoop;
    public AudioClip monsterDeath;

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

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
