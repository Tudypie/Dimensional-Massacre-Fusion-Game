using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{   
    public bool isPlayingVoiceLine = false;
    public int randomChanceToPlay = 3;
    [SerializeField] private AudioClip[] voiceLines;

    public static VoiceLines Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayVoiceLine()
    {   
        if(isPlayingVoiceLine)
            return;
            
        int randomClip = Random.Range(0, voiceLines.Length);
        AudioPlayer.Instance.PlayAudio(voiceLines[randomClip]);
        isPlayingVoiceLine = true;
        Invoke("StopVoiceLine", voiceLines[randomClip].length);
    }

    public void StopVoiceLine()
    {
        isPlayingVoiceLine = false;
    }
}
