using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource calmMusic;
    [SerializeField] private AudioSource agressiveMusic;
    [SerializeField] public bool playerIsChased;

    public static MusicController Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if(playerIsChased)
            agressiveMusic.mute = false;
        else
            agressiveMusic.mute = true;
    }


}
