﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerRecommendedScreen : MonoBehaviour
{
    void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
    }
}
