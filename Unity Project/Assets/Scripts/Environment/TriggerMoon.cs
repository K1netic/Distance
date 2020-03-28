using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMoon : MonoBehaviour
{
    [SerializeField] GameObject Moon;
    CircularPlatform script;
    MusicManager musicManager;

    void Awake()
    {
        script = Moon.GetComponent<CircularPlatform>();
        musicManager = GameObject.FindObjectOfType<MusicManager>();
    }

    // The script must be put on the exit zone that leads to a board with a moon (choice or ending)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Activate the movement script on the moon object when the player enters the exit zone that preceeds the moon's board
            script.startMoving = true;
            // Stop the current music
            musicManager.currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicManager.currentInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}
