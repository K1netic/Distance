using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [FMODUnity.EventRef] public string tutoMusic; // TUTO
    [FMODUnity.EventRef] public string dashMusic; // D
    [FMODUnity.EventRef] public string jumpMusic; // J
    [FMODUnity.EventRef] public string dashDashMusic; // DD
    [FMODUnity.EventRef] public string jumpJumpMusic; // JJ
    [FMODUnity.EventRef] public string jumpDashMusic; // JD
    [FMODUnity.EventRef] public string dashJumpMusic; // DJ

    [FMODUnity.EventRef] public string ambientSounds;
    
    public static bool triggerTutoMusic = false;
    public static bool triggerDashMusic = false;
    public static bool triggerJumpMusic = false;
    public static bool triggerDashDashMusic = false;
    public static bool triggerJumpJumpMusic = false;
    public static bool triggerJumpDashMusic = false;
    public static bool triggerDashJumpMusic = false;

    bool tutoMusicPlayed = false;
    bool dashMusicPlayed = false;
    bool jumpMusicPlayed = false;
    bool jumpJumpMusicPlayed = false;
    bool dashDashMusicPlayed = false;
    bool jumpDashMusicPlayed = false;
    bool dashJumpMusicPlayed = false;

    public FMOD.Studio.EventInstance currentInstance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        // Play ambient sounds
        FMODUnity.RuntimeManager.PlayOneShot(ambientSounds);
    }

    void Update()
    {
        // Play TUTO music
        if (triggerTutoMusic && !tutoMusicPlayed)
        {
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(tutoMusic);
            currentInstance.start();
            tutoMusicPlayed = true;
        }

        // Fade out current music and play D music
        if (triggerDashMusic && !dashMusicPlayed)
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(dashMusic);
            currentInstance.start();
            dashMusicPlayed = true;
            tutoMusicPlayed = false;
        }

        // Fadeout current music and play J music
        if (triggerJumpMusic && !jumpMusicPlayed) 
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(jumpMusic);
            currentInstance.start();
            jumpMusicPlayed = true;
            tutoMusicPlayed = false;
        }

        // Fade out current music and play JJ music
        if (triggerJumpJumpMusic && !jumpJumpMusicPlayed)
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(jumpJumpMusic);
            currentInstance.start();
            jumpJumpMusicPlayed = true;
            jumpMusicPlayed = false;
        }

        // Fade out current music and play DD music
        if (triggerDashDashMusic && !dashDashMusicPlayed)
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(dashDashMusic);
            currentInstance.start();
            dashDashMusicPlayed = true;
            dashMusicPlayed = false;
        }

        // Fade out current music and play JD music
        if (triggerJumpDashMusic && !jumpDashMusicPlayed)
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(jumpDashMusic);
            currentInstance.start();
            jumpDashMusicPlayed = true;
            jumpMusicPlayed = false;
        }

        // Fade out current music and play DJ music
        if (triggerDashJumpMusic && !dashJumpMusicPlayed)
        {
            currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentInstance = FMODUnity.RuntimeManager.CreateInstance(dashJumpMusic);
            currentInstance.start();
            dashJumpMusicPlayed = true;
            dashMusicPlayed = false;
        }
    }

}
