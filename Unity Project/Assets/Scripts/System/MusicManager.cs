using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [FMODUnity.EventRef] public string tutoMusic;
    [FMODUnity.EventRef] public string dashMusic;
    [FMODUnity.EventRef] public string jumpMusic;

    [FMODUnity.EventRef] public string ambientSounds;
    
    public static bool triggerTutoMusic = false;
    public static bool triggerDashMusic = false;
    public static bool triggerJumpMusic = false;

    bool tutoMusicPlayed = false;
    bool dashMusicPlayed = false;
    bool jumpMusicPlayed = false;

    public FMOD.Studio.EventInstance currentInstance;

    void Start()
    {
        // Play ambient sounds
        FMODUnity.RuntimeManager.PlayOneShot(ambientSounds);
    }

    void Update()
    {
        // Play tuto music
        if (triggerTutoMusic && !tutoMusicPlayed)
        {
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(tutoMusic);
            currentInstance.start();
            tutoMusicPlayed = true;
        }

        // Fade out current music and play dash music
        if (triggerDashMusic && !dashMusicPlayed)
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(dashMusic);
            currentInstance.start();
            dashMusicPlayed = true;
            tutoMusicPlayed = false;
        }

        // Fadeout current music and play jump music
        if (triggerJumpMusic && !jumpMusicPlayed) 
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(jumpMusic);
            currentInstance.start();
            jumpMusicPlayed = true;
            tutoMusicPlayed = false;
        }
    }

}
