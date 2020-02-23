using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerRecommendedScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
    }
}
