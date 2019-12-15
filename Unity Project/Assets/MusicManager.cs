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

    void Update()
    {
        if (triggerTutoMusic && !tutoMusicPlayed)
        {
            FMODUnity.RuntimeManager.PlayOneShot(tutoMusic);
            tutoMusicPlayed = true;
        }

        if (triggerDashMusic && !dashMusicPlayed)
        {
            FMODUnity.RuntimeManager.PlayOneShot(dashMusic);
            dashMusicPlayed = true;
        }

        if (triggerJumpMusic && !jumpMusicPlayed) 
        {
            FMODUnity.RuntimeManager.PlayOneShot(jumpMusic);
            jumpMusicPlayed = true;
        }
    }

}
