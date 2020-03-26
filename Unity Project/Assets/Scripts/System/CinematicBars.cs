using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Code by CodeMonkey
public class CinematicBars : MonoBehaviour
{
    RectTransform topBar, bottomBar;
    float changeSizeAmount;
    float targetSize;
    bool isActive;

    void Awake()
    {
        // Top bar creation
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        topBar = gameObject.GetComponent<RectTransform>();
        topBar.anchorMin = new Vector2(0, 1);
        topBar.anchorMax = new Vector2(1, 1);
        topBar.sizeDelta = new Vector2(0, 0);

        // Bottom bar creation
        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        bottomBar = gameObject.GetComponent<RectTransform>();
        bottomBar.anchorMin = new Vector2(0, 0);
        bottomBar.anchorMax = new Vector2(1, 0);
        bottomBar.sizeDelta = new Vector2(0, 0);
    }

    void Update()
    {
        // Move the bars through time
        if (isActive)
        {
            Vector2 sizeDelta = topBar.sizeDelta;
            sizeDelta.y += changeSizeAmount * Time.deltaTime;
            if (changeSizeAmount > 0)
            {
                if (sizeDelta.y >= targetSize)
                {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            }
            else 
            {
                if (sizeDelta.y <= targetSize)
                {
                    sizeDelta.y = targetSize;
                    isActive = false;
                } 
            }
            topBar.sizeDelta = sizeDelta;
            bottomBar.sizeDelta = sizeDelta;
        }
        
    }

    public void Show(float targetSize, float time)
    {
        this.targetSize = targetSize;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }

    public void Hide(float time)
    {
        targetSize = 0f;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }
}
