using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource calmMusic;
    [SerializeField] private AudioSource agressiveMusic;
    public bool playerIsChased;
    public float agressiveMusicVolume;
    public float fadeInSpeed;
    public float fadeOutSpeed;

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
        {
            agressiveMusic.volume = Mathf.Lerp(agressiveMusic.volume, agressiveMusicVolume, fadeInSpeed * Time.deltaTime);
        }
        else
        {
            agressiveMusic.volume -= fadeOutSpeed * Time.deltaTime;
        }
    }


}
