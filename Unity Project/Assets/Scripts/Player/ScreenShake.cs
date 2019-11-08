using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    float duration = 1.0f;
    float magnitude = 3.0f;
    [HideInInspector] public bool shake = false;

    private static ScreenShake instance;

	public static ScreenShake Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<ScreenShake> ();
			}

			return instance;
		}
	}

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ApplyScreenShake(float length, float strength)
    {
        duration = length;
        magnitude = strength;
        shake = true;
    }

    void Update()
    {
        if (shake)
        {
            StartCoroutine(Shake());
        }
    }
    IEnumerator Shake() {
        
    float elapsed = 0.0f;
    
    Vector3 originalCamPos = Camera.main.transform.position;
    
    while (elapsed < duration) {
        
        elapsed += Time.deltaTime;          
        
        float percentComplete = elapsed / duration;         
        float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
        
        // map value to [-1, 1]
        float x = Random.value * 2.0f - 1.0f;
        float y = Random.value * 2.0f - 1.0f;
        x *= magnitude * damper;
        y *= magnitude * damper;
        
        Camera.main.transform.position = new Vector3(originalCamPos.x + x, originalCamPos.y + y, originalCamPos.z);
            
        yield return null;
    }
    
    Camera.main.transform.position = originalCamPos;
    shake = false;
}
}
