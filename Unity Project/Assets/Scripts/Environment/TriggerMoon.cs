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

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            script.startMoving = true;
            musicManager.currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
